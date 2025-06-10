let draggedItem = null;

document.addEventListener("DOMContentLoaded", () => {
    // Set up draggable items
    document.querySelectorAll(".draggable-character, .draggable-note").forEach(el => {
        el.addEventListener("dragstart", e => {
            draggedItem = {
                id: el.dataset.id,
                type: el.classList.contains("draggable-character") ? "character" : "worldnote",
                title: el.dataset.title
            };

            el.classList.add("dragging");

            // Clone as ghost image for better dragging visuals
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

    // Set up drop targets
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

            const bookId = target.getAttribute("data-book-id");
            const bookTitle = target.getAttribute("data-book-title");
            const isSeries = target.getAttribute("data-series") === "true";
            const { id, type } = draggedItem;

            let applyToSeries = false;
            if (isSeries) {
                applyToSeries = confirm(`"${bookTitle}" is part of a series. Associate this ${type} with the entire series?`);
            }

            const payload = applyToSeries
                ? { id: parseInt(id), type, bookId: parseInt(bookId), series: true }
                : { id: parseInt(id), type, bookId: parseInt(bookId) };

            const response = await fetch('/Associations/Link', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(payload)
            });

            if (response.ok) {
                location.reload(); // TODO: Replace with dynamic UI update
            } else {
                const error = await response.text();
                alert("Failed to associate: " + error);
            }

            draggedItem = null;
        });
    });

});

document.querySelectorAll(".drop-target").forEach(target => {
    target.addEventListener("dragover", e => e.preventDefault());

    target.addEventListener("drop", async e => {
        e.preventDefault();
        if (!draggedItem) return;

        const bookId = target.getAttribute("data-book-id");
        const bookTitle = target.getAttribute("data-book-title");
        const { id, type, title } = draggedItem;

        const isUnassign = !bookId;
        const endpoint = isUnassign ? '/Associations/Unlink' : '/Associations/Link';

        const payload = isUnassign
            ? {
                id: parseInt(draggedItem.id),
                bookId: parseInt(draggedItem.bookId),
                type: draggedItem.type
            }
            : {
                id: parseInt(draggedItem.id),
                bookId: parseInt(bookId),
                type: draggedItem.type
            };

        const response = await fetch(endpoint, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(payload)
        });

        if (response.ok) {
            location.reload(); // TODO: dynamic update later
        } else {
            const error = await response.text();
            alert("Failed to " + (isUnassign ? "unassign" : "assign") + ": " + error);
        }

        draggedItem = null;
    });
});

