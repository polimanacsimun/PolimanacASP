$(document).ready(function() {
    // Initialize autocomplete behavior for all inputs marked with data-autocomplete="true"
    const initAutocomplete = function() {
        $('[data-autocomplete="true"]').each(function() {
            const $input = $(this);
            const searchUrl = $input.data('search-url');
            const hiddenInputId = $input.data('hidden-input-id');
            const $hiddenInput = $('#' + hiddenInputId);
            const suggestionsContainerId = $input.attr('id') + '_suggestions';
            const $suggestions = $('#' + suggestionsContainerId);

            // Handle input event
            $input.on('input', function() {
                const term = $input.val().trim();

                // Clear hidden input if visible input was manually changed
                if ($hiddenInput.val() && term !== $hiddenInput.data('last-text')) {
                    $hiddenInput.val('');
                }

                // Only search if term is at least 2 characters
                if (term.length < 2) {
                    $suggestions.empty().hide();
                    return;
                }

                // Show loading state
                $suggestions.html('<div class="list-group-item text-muted"><small>Loading...</small></div>').show();

                // Send AJAX request
                $.ajax({
                    url: searchUrl,
                    data: { term: term },
                    method: 'GET',
                    dataType: 'json',
                    timeout: 5000,
                    success: function(results) {
                        renderSuggestions(results);
                    },
                    error: function() {
                        $suggestions.html('<div class="list-group-item text-danger"><small>Error loading results</small></div>').show();
                    }
                });
            });

            // Handle suggestion selection
            $(document).on('click', '#' + suggestionsContainerId + ' .list-group-item', function() {
                const $item = $(this);
                const itemId = $item.data('item-id');
                const itemText = $item.data('item-text');

                $input.val(itemText);
                $hiddenInput.val(itemId).data('last-text', itemText);
                $suggestions.empty().hide();

                // Trigger validation on hidden input
                $hiddenInput.trigger('blur').trigger('change');
            });

            // Handle blur event - hide suggestions with slight delay
            $input.on('blur', function() {
                setTimeout(function() {
                    $suggestions.fadeOut(200);
                }, 200);
            });

            // Handle Escape key
            $input.on('keydown', function(e) {
                if (e.key === 'Escape') {
                    $suggestions.empty().hide();
                }
            });

            // Helper function to render suggestions
            function renderSuggestions(results) {
                if (!results || results.length === 0) {
                    $suggestions.html('<div class="list-group-item text-muted"><small>No results found</small></div>').show();
                    return;
                }

                let html = '';
                results.forEach(function(item) {
                    html += `<button type="button" class="list-group-item list-group-item-action" data-item-id="${item.id}" data-item-text="${item.text}">
                        ${escapeHtml(item.text)}
                    </button>`;
                });

                $suggestions.html(html).show();
            }

            // Helper function to escape HTML special characters
            function escapeHtml(text) {
                const div = document.createElement('div');
                div.textContent = text;
                return div.innerHTML;
            }
        });
    };

    // Initialize on page load
    initAutocomplete();

    // Re-initialize if dynamic content is added (e.g., via AJAX)
    $(document).on('ajaxStop', function() {
        initAutocomplete();
    });
});
