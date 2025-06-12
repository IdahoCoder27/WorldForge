let draggedItem = null;

document.addEventListener("DOMContentLoaded", () => {
    // Setup all draggable items
    document.querySelectorAll(".draggable-character, .draggable-note").forEach(el => {
        el.addEventListener("dragstart", e => {
            draggedItem = {
                id: el.dataset.id,
                type: el.classList.contains("draggable-character") ? "character" : "worldnote",
                title: el.dataset.title
            };

            el.classList.add("dragging");

            // Visual ghost clone
            const ghost = el.cloneNode(true);
            ghost.style.position = "absolute";
            ghost.style.top = "-9999px";
            ghost.style.left = "-9999px";
            ghost.style.opacity = "0.75";
            document.body.appendChild(ghost);
            e.dataTransfer.setDragImage(ghost, 0, 0);
            setTimeout(() => document.body.removeChild(ghost), 0);

            e.dataTransfer.setData("text/plain", JSON.stringify(draggedItem));
        });

        el.addEventListener("dragend", () => {
            el.classList.remove("dragging");
        });
    });

    // Unified drop handler
    document.querySelectorAll(".drop-target").forEach(target => {
        target.addEventListener("dragover", e => {
            e.preventDefault();
            target.classList.add("border-primary");
        });

        target.addEventListener("dragleave", () => {
            target.classList.remove("border-primary");
        });

        target.addEventListener("drop", async e => {
            e.preventDefault();
            target.classList.remove("border-primary");

            if (!draggedItem) return;

            const bookIdAttr = target.getAttribute("data-book-id");
            const bookTitle = target.getAttribute("data-book-title") || "";
            const isSeries = target.getAttribute("data-series") === "true";
            const isUnassign = bookIdAttr === "0";
            const endpoint = isUnassign ? "/Associations/Unlink" : "/Associations/Link";

            // Build payload
            let payload = {
                id: parseInt(draggedItem.id),
                type: draggedItem.type
            };

            if (!isUnassign) {
                payload.bookId = parseInt(bookIdAttr);

                // Ask if user wants to apply to the entire series
                if (isSeries) {
                    const confirmSeries = confirm(`"${bookTitle}" is part of a series. Associate this ${draggedItem.type} with the entire series?`);
                    if (confirmSeries) {
                        payload.series = true;
                    }
                }
            } else {
                payload.bookId = parseInt(bookIdAttr); // Unassign still needs BookId
            }

            if (isUnassign) {
                payload.bookId = parseInt(bookIdAttr); // required for unlink
            } else {
                payload.bookId = parseInt(bookIdAttr);

                if (isSeries) {
                    const applyToSeries = confirm(`"${bookTitle}" is part of a series. Associate this ${draggedItem.type} with the entire series?`);
                    if (applyToSeries) {
                        payload.series = true;
                    }
                }
            }

            console.log("Payload:", JSON.stringify(payload));

            // Send the request
            const response = await fetch(endpoint, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(payload)
            });

            if (response.ok) {
                location.reload(); // TODO: Replace with dynamic UI update
            } else {
                const error = await response.text();
                alert(`Failed to ${isUnassign ? "unassign" : "assign"}: ${error}`);
            }

            draggedItem = null;
        });
    });
});
