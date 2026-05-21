/**
 * ui-polish.js
 * Adds subtle UI enhancements to the CRUD application:
 * - Delete confirmation dialogs
 * - Bootstrap toast auto-display
 * - Row highlighting from URL parameters
 * - Page load animation trigger
 */

document.addEventListener('DOMContentLoaded', function() {
    // Add marker class for animation triggers
    document.body.classList.add('js-enhanced');

    // Initialize delete confirmations
    initDeleteConfirmations();

    // Initialize toasts
    initToasts();

    // Initialize row highlighting
    initRowHighlight();
});

/**
 * Delete Confirmation
 * Attaches confirm dialog to elements with data-confirm-delete="true"
 */
function initDeleteConfirmations() {
    const confirmElements = document.querySelectorAll('[data-confirm-delete="true"]');

    confirmElements.forEach(element => {
        element.addEventListener('click', function(e) {
            // Get custom message or use default
            const message = element.getAttribute('data-confirm-message') ||
                'Are you sure you want to delete this item? This action cannot be undone.';

            if (!confirm(message)) {
                e.preventDefault();
                e.stopPropagation();
                return false;
            }
        });

        // Handle form submissions
        if (element.tagName === 'FORM') {
            element.addEventListener('submit', function(e) {
                const message = element.getAttribute('data-confirm-message') ||
                    'Are you sure you want to delete this item? This action cannot be undone.';

                if (!confirm(message)) {
                    e.preventDefault();
                    return false;
                }
            });
        }
    });
}

/**
 * Bootstrap Toast Support
 * Auto-displays toast with id="appToast" on page load
 */
function initToasts() {
    const toastElement = document.getElementById('appToast');
    if (toastElement) {
        // Bootstrap 5 Toast API
        const toast = new bootstrap.Toast(toastElement, {
            autohide: true,
            delay: 5000
        });
        toast.show();
    }
}

/**
 * Row Highlighting
 * Highlights rows based on URL parameter highlightId
 * Looks for element with data-row-id matching the parameter value
 */
function initRowHighlight() {
    // Get highlightId from URL query parameters
    const params = new URLSearchParams(window.location.search);
    const highlightId = params.get('highlightId');

    if (highlightId) {
        // Find element with matching data-row-id
        const rowElement = document.querySelector(`[data-row-id="${highlightId}"]`);

        if (rowElement) {
            // Add highlight class
            rowElement.classList.add('table-warning');

            // Scroll into view smoothly
            rowElement.scrollIntoView({
                behavior: 'smooth',
                block: 'center'
            });

            // Remove highlight after 2500ms
            setTimeout(() => {
                rowElement.classList.remove('table-warning');
            }, 2500);
        }
    }
}
