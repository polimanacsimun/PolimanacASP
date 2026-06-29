using System.Security.Claims;
using InventoryManagement.DAL;
using InventoryManagement.Domain.Enums;
using InventoryManagement.Domain.Models;
using InventoryManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly InventoryManagementDbContext _dbContext;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            InventoryManagementDbContext dbContext,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new AppUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true,
                FirstName = model.FirstName,
                LastName = model.LastName,
                OIB = model.OIB,
                JMBG = model.JMBG
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                if (await _userManager.IsInRoleAsync(user, "Manager") == false)
                {
                    await _userManager.AddToRoleAsync(user, "Manager");
                }

                await EnsureBusinessUserExistsAsync(user, UserRole.Customer);

                await _signInManager.SignInAsync(user, isPersistent: false);

                TempData["ToastMessage"] = $"Welcome, {user.FirstName}! Your account was created successfully.";
                _logger.LogInformation("Local account registered and signed in for {Email}.", user.Email);

                return RedirectToAction("Index", "Home");
            }

            _logger.LogWarning(
                "Local account registration failed for {Email}. Errors: {Errors}",
                model.Email,
                string.Join("; ", result.Errors.Select(error => error.Code)));

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                model.Email,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var appUser = await _userManager.FindByEmailAsync(model.Email);
                var displayName = appUser == null
                    ? model.Email
                    : appUser.FirstName;

                TempData["ToastMessage"] = $"Welcome back, {displayName}! You signed in successfully.";
                _logger.LogInformation("Local login succeeded for {Email}.", model.Email);

                if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            _logger.LogWarning("Local login failed for {Email}.", model.Email);

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string? returnUrl = null)
        {
            if (string.IsNullOrWhiteSpace(provider))
            {
                _logger.LogWarning("External login request was rejected because provider was missing.");
                return RedirectToAction(nameof(Login), new { returnUrl });
            }

            var redirectUrl = Url.Action(
                nameof(ExternalLoginCallback),
                "Account",
                new { returnUrl });

            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return Challenge(properties, provider);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
        {
            if (!string.IsNullOrWhiteSpace(remoteError))
            {
                _logger.LogWarning("External login returned an error: {RemoteError}.", remoteError);
                ModelState.AddModelError(string.Empty, $"External login error: {remoteError}");
                ViewData["ReturnUrl"] = returnUrl;
                return View(nameof(Login));
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                _logger.LogWarning("External login information could not be loaded.");
                ModelState.AddModelError(string.Empty, "External login information could not be loaded.");
                ViewData["ReturnUrl"] = returnUrl;
                return View(nameof(Login));
            }

            var signInResult = await _signInManager.ExternalLoginSignInAsync(
                info.LoginProvider,
                info.ProviderKey,
                isPersistent: false,
                bypassTwoFactor: true);

            if (signInResult.Succeeded)
            {
                var existingExternalUser = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
                var existingName = existingExternalUser?.FirstName ?? info.Principal.FindFirstValue(ClaimTypes.GivenName) ?? "User";

                TempData["ToastMessage"] = $"Welcome back, {existingName}! You signed in with {info.LoginProvider}.";
                _logger.LogInformation(
                    "External login succeeded for existing user through {Provider}.",
                    info.LoginProvider);

                return RedirectToLocal(returnUrl);
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrWhiteSpace(email))
            {
                _logger.LogWarning("External login through {Provider} did not return an email address.", info.LoginProvider);
                ModelState.AddModelError(string.Empty, "External provider did not return an email address.");
                ViewData["ReturnUrl"] = returnUrl;
                return View(nameof(Login));
            }

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                var firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName);
                var lastName = info.Principal.FindFirstValue(ClaimTypes.Surname);
                var fullName = info.Principal.FindFirstValue(ClaimTypes.Name);

                if (string.IsNullOrWhiteSpace(firstName) && !string.IsNullOrWhiteSpace(fullName))
                {
                    var nameParts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    firstName = nameParts.FirstOrDefault();
                    lastName = nameParts.Length > 1 ? string.Join(' ', nameParts.Skip(1)) : "External";
                }

                user = new AppUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    FirstName = firstName ?? "External",
                    LastName = lastName ?? "User",
                    OIB = "00000000000",
                    JMBG = "0000000000000"
                };

                var createResult = await _userManager.CreateAsync(user);

                if (!createResult.Succeeded)
                {
                    _logger.LogWarning(
                        "External account creation failed for {Email} through {Provider}. Errors: {Errors}",
                        email,
                        info.LoginProvider,
                        string.Join("; ", createResult.Errors.Select(error => error.Code)));

                    foreach (var error in createResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    ViewData["ReturnUrl"] = returnUrl;
                    return View(nameof(Login));
                }

                await _userManager.AddToRoleAsync(user, "Manager");
                await EnsureBusinessUserExistsAsync(user, UserRole.Customer);
            }

            var addLoginResult = await _userManager.AddLoginAsync(user, info);

            if (!addLoginResult.Succeeded)
            {
                _logger.LogWarning(
                    "External login association failed for {Email} through {Provider}. Errors: {Errors}",
                    email,
                    info.LoginProvider,
                    string.Join("; ", addLoginResult.Errors.Select(error => error.Code)));

                foreach (var error in addLoginResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                ViewData["ReturnUrl"] = returnUrl;
                return View(nameof(Login));
            }

            await _signInManager.SignInAsync(user, isPersistent: false);

            TempData["ToastMessage"] = $"Welcome, {user.FirstName}! You signed in with {info.LoginProvider}.";
            _logger.LogInformation(
                "External login succeeded for {Email} through {Provider}.",
                email,
                info.LoginProvider);

            return RedirectToLocal(returnUrl);
        }

        private async Task EnsureBusinessUserExistsAsync(AppUser appUser, UserRole role)
        {
            if (string.IsNullOrWhiteSpace(appUser.Email))
            {
                return;
            }

            var email = appUser.Email.Trim();
            var normalizedEmail = email.ToUpperInvariant();
            var exists = await _dbContext.BusinessUsers
                .AnyAsync(u => u.Email.ToUpper() == normalizedEmail);

            if (exists)
            {
                return;
            }

            _dbContext.BusinessUsers.Add(new User
            {
                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                Email = email,
                Role = role,
                RegistrationDate = DateTime.Today,
                IsActive = true
            });

            await _dbContext.SaveChangesAsync();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            var userName = User.Identity?.Name ?? "unknown";
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User {UserName} signed out.", userName);

            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private IActionResult RedirectToLocal(string? returnUrl)
        {
            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
