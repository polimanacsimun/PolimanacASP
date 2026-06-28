using Microsoft.AspNetCore.Razor.TagHelpers;

namespace InventoryManagement.TagHelpers
{
    [HtmlTargetElement(Attributes = AttributeName)]
    public class AuthorizeRoleTagHelper : TagHelper
    {
        private const string AttributeName = "asp-authorize-role";

        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthorizeRoleTagHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HtmlAttributeName(AttributeName)]
        public string Roles { get; set; } = string.Empty;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var allowedRoles = Roles.Split(
                ',',
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            if (user?.Identity?.IsAuthenticated != true ||
                !allowedRoles.Any(user.IsInRole))
            {
                output.SuppressOutput();
            }
        }
    }
}
