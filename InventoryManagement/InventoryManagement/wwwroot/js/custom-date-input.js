(function() {
    'use strict';

    /**
     * Detects browser language
     */
    function getBrowserLanguage() {
        return navigator.language || navigator.userLanguage || 'en-US';
    }

    /**
     * Checks if browser language is Croatian
     */
    function isCroatian() {
        return getBrowserLanguage().startsWith('hr');
    }

    /**
     * Formats an ISO datetime string to display format
     * ISO format: yyyy-MM-dd or yyyy-MM-ddTHH:mm
     */
    function formatDateForDisplay(isoString, includeTime) {
        if (!isoString || !isoString.trim()) {
            return '';
        }

        try {
            let parts;
            let day, month, year, hours = 0, minutes = 0;

            if (includeTime && isoString.includes('T')) {
                // Format: yyyy-MM-ddTHH:mm
                const [datePart, timePart] = isoString.split('T');
                const [y, m, d] = datePart.split('-');
                const [h, min] = timePart.split(':');

                day = parseInt(d, 10);
                month = parseInt(m, 10);
                year = parseInt(y, 10);
                hours = parseInt(h, 10);
                minutes = parseInt(min, 10);
            } else {
                // Format: yyyy-MM-dd
                const [y, m, d] = isoString.split('-');
                day = parseInt(d, 10);
                month = parseInt(m, 10);
                year = parseInt(y, 10);
            }

            const dayStr = String(day).padStart(2, '0');
            const monthStr = String(month).padStart(2, '0');
            const hoursStr = String(hours).padStart(2, '0');
            const minutesStr = String(minutes).padStart(2, '0');

            if (isCroatian()) {
                return includeTime
                    ? `${dayStr}.${monthStr}.${year}. ${hoursStr}:${minutesStr}`
                    : `${dayStr}.${monthStr}.${year}.`;
            } else {
                return includeTime
                    ? `${monthStr}/${dayStr}/${year} ${hoursStr}:${minutesStr}`
                    : `${monthStr}/${dayStr}/${year}`;
            }
        } catch (e) {
            return '';
        }
    }

    /**
     * Parses display format input to ISO format
     * Handles both Croatian and English formats with optional time
     */
    function parseDisplayToIso(displayString, includeTime) {
        if (!displayString || !displayString.trim()) {
            return '';
        }

        const input = displayString.trim();
        let day, month, year, hours = 0, minutes = 0;

        try {
            if (isCroatian()) {
                // Croatian format: dd.MM.yyyy. [HH:mm]
                const regex = includeTime
                    ? /^(\d{1,2})\.(\d{1,2})\.(\d{4})\.?\s+(\d{1,2}):(\d{1,2})$/
                    : /^(\d{1,2})\.(\d{1,2})\.(\d{4})\.?$/;

                const match = input.match(regex);
                if (!match) return '';

                day = parseInt(match[1], 10);
                month = parseInt(match[2], 10);
                year = parseInt(match[3], 10);
                if (includeTime) {
                    hours = parseInt(match[4], 10);
                    minutes = parseInt(match[5], 10);
                }
            } else {
                // English format: MM/dd/yyyy [HH:mm]
                const regex = includeTime
                    ? /^(\d{1,2})\/(\d{1,2})\/(\d{4})\s+(\d{1,2}):(\d{1,2})$/
                    : /^(\d{1,2})\/(\d{1,2})\/(\d{4})$/;

                const match = input.match(regex);
                if (!match) return '';

                month = parseInt(match[1], 10);
                day = parseInt(match[2], 10);
                year = parseInt(match[3], 10);
                if (includeTime) {
                    hours = parseInt(match[4], 10);
                    minutes = parseInt(match[5], 10);
                }
            }

            // Validate date values
            if (month < 1 || month > 12 || day < 1 || day > 31) {
                return '';
            }

            if (year < 1900 || year > 2100) {
                return '';
            }

            // Validate actual day exists in month
            const testDate = new Date(year, month - 1, day);
            if (testDate.getDate() !== day || testDate.getMonth() !== month - 1) {
                return '';
            }

            // Validate time values
            if (hours < 0 || hours > 23 || minutes < 0 || minutes > 59) {
                return '';
            }

            // Format as ISO
            const dayStr = String(day).padStart(2, '0');
            const monthStr = String(month).padStart(2, '0');
            const hoursStr = String(hours).padStart(2, '0');
            const minutesStr = String(minutes).padStart(2, '0');

            if (includeTime) {
                return `${year}-${monthStr}-${dayStr}T${hoursStr}:${minutesStr}`;
            } else {
                return `${year}-${monthStr}-${dayStr}`;
            }
        } catch (e) {
            return '';
        }
    }

    /**
     * Initializes all custom date/time inputs on the page
     */
    function initCustomDateInputs() {
        const inputs = document.querySelectorAll('input[data-date-input="true"]');

        inputs.forEach(function(visibleInput) {
            const hiddenInputId = visibleInput.getAttribute('data-hidden-input-id');
            const includeTimeStr = visibleInput.getAttribute('data-include-time');
            const includeTime = includeTimeStr === 'true';

            if (!hiddenInputId) {
                return;
            }

            const hiddenInput = document.getElementById(hiddenInputId);
            if (!hiddenInput) {
                return;
            }

            // On page load, format the visible input from hidden value
            if (hiddenInput.value) {
                visibleInput.value = formatDateForDisplay(hiddenInput.value, includeTime);
            }

            // On blur, parse visible input and update hidden input
            visibleInput.addEventListener('blur', function() {
                const displayValue = visibleInput.value;

                if (displayValue.trim() === '') {
                    // Empty input is allowed
                    hiddenInput.value = '';
                    visibleInput.classList.remove('is-invalid');
                } else {
                    const isoValue = parseDisplayToIso(displayValue, includeTime);

                    if (isoValue) {
                        // Valid parse
                        hiddenInput.value = isoValue;
                        visibleInput.classList.remove('is-invalid');
                        // Trigger change and validation events
                        hiddenInput.dispatchEvent(new Event('change', { bubbles: true }));
                    } else {
                        // Invalid parse
                        visibleInput.classList.add('is-invalid');
                    }
                }
            });

            // On input, remove invalid class while user is typing
            visibleInput.addEventListener('input', function() {
                if (visibleInput.value.trim() === '') {
                    visibleInput.classList.remove('is-invalid');
                }
            });
        });
    }

    // Initialize on DOM ready
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', initCustomDateInputs);
    } else {
        // DOM already loaded
        initCustomDateInputs();
    }

    // Also support if loaded after DOM is ready (e.g., dynamic content)
    if (typeof jQuery !== 'undefined') {
        jQuery(document).ready(initCustomDateInputs);
    }
})();
