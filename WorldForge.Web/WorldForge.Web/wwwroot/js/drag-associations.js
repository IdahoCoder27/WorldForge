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

            const bookId = target.dataset.bookId;

            const response = await fetch('/Associations/Link', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    id: draggedItem.id,
                    type: draggedItem.type,
                    bookId: bookId
                })
            });

            if (response.ok) {
                location.reload(); // Later, change to dynamic update
            } else {
                const error = await response.text();
                alert("Failed to associate: " + error);
            }

            draggedItem = null;
        });
    });
});
