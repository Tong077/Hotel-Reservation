document.addEventListener('DOMContentLoaded', () => {

    document.addEventListener('submit', async function (e) {
        const form = e.target;

        // Only work with forms that explicitly use AJAX
        if (!form.matches('form[data-ajax="true"]')) return;

        e.preventDefault();

        const updateTarget = form.getAttribute("data-ajax-update");    // update part of page
        const redirectTarget = form.getAttribute("data-ajax-redirect"); // load redirect URL into a container

        const formData = new FormData(form);

        try {
            // Send AJAX request
            const response = await fetch(form.action, {
                method: form.method || "POST",
                body: formData
            });

            const rawText = await response.text(); // Could be JSON OR HTML

            // Try JSON first
            let json = null;
            try {
                json = JSON.parse(rawText);
            } catch {
                // Not JSON → treat as HTML
            }

            // -----------------------------------
            // 1️⃣ JSON RESPONSE HANDLING
            // -----------------------------------
            if (json) {

                // Success = true
                if (json.success) {

                    // A. Update a section with HTML from controller
                    if (json.html && updateTarget) {
                        document.querySelector(updateTarget).innerHTML = json.html;
                        return;
                    }

                    // B. AJAX Redirect (Load page into a container)
                    if (json.redirectUrl && redirectTarget) {
                        const newPage = await fetch(json.redirectUrl);
                        const html = await newPage.text();
                        document.querySelector(redirectTarget).innerHTML = html;
                        return;
                    }

                    // C. Full browser redirect (if no redirectTarget provided)
                    if (json.redirectUrl) {
                        window.location.href = json.redirectUrl;
                        return;
                    }

                    // If success but no redirect/update → show message (optional)
                    if (json.message) {
                        alert(json.message);
                    }

                    return;
                }

                // json.success === false
                alert(json.message || "Operation failed.");
                return;
            }

            // -----------------------------------
            // 2️⃣ RAW HTML RESPONSE HANDLING (Partial View)
            // -----------------------------------
            if (updateTarget) {
                document.querySelector(updateTarget).innerHTML = rawText;
                return;
            }

        } catch (error) {
            console.error("AJAX ERROR:", error);
            alert("Unexpected error occurred.");
        }
    });

});
