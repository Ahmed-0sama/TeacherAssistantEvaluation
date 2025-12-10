export async function saveElementAsPdf(elementId, fileName) {
    const original = document.getElementById(elementId);
    if (!original) {
        alert("Element not found: " + elementId);
        return;
    }

    // 1. Clone modal EXACTLY
    const clone = original.cloneNode(true);

    // ✅ CRITICAL FIX: Copy all select element values from original to clone
    const originalSelects = original.querySelectorAll('select');
    const clonedSelects = clone.querySelectorAll('select');

    originalSelects.forEach((originalSelect, index) => {
        if (clonedSelects[index]) {
            // Set the value
            clonedSelects[index].value = originalSelect.value;

            // Mark the correct option as selected
            Array.from(clonedSelects[index].options).forEach(option => {
                option.selected = option.value === originalSelect.value;
            });
        }
    });

    // ✅ CRITICAL FIX: Copy all input/textarea values
    const originalInputs = original.querySelectorAll('input, textarea');
    const clonedInputs = clone.querySelectorAll('input, textarea');

    originalInputs.forEach((originalInput, index) => {
        if (clonedInputs[index]) {
            if (originalInput.type === 'checkbox' || originalInput.type === 'radio') {
                clonedInputs[index].checked = originalInput.checked;
            } else {
                clonedInputs[index].value = originalInput.value;
            }
        }
    });

    // 2. Remove modal constraints completely
    clone.style.position = "absolute";
    clone.style.left = "0";
    clone.style.top = "0";
    clone.style.width = original.scrollWidth + "px";
    clone.style.maxWidth = "none";
    clone.style.height = "auto";
    clone.style.maxHeight = "none";
    clone.style.overflow = "visible";
    clone.style.background = "#ffffff";
    clone.style.zIndex = "-1";
    clone.style.padding = "20px";

    // 3. Append to DOM
    document.body.appendChild(clone);

    // Allow rendering
    await new Promise(r => setTimeout(r, 300));

    // 4. Load libs
    if (typeof html2canvas === "undefined") {
        await import("https://cdnjs.cloudflare.com/ajax/libs/html2canvas/1.4.1/html2canvas.min.js");
    }
    if (typeof jspdf === "undefined") {
        await import("https://cdnjs.cloudflare.com/ajax/libs/jspdf/2.5.1/jspdf.umd.min.js");
    }

    // 5. Render full clone
    const canvas = await html2canvas(clone, {
        scale: 2,
        useCORS: true,
        backgroundColor: "#ffffff",
        logging: false,
        allowTaint: true,
        foreignObjectRendering: false
    });

    const imgData = canvas.toDataURL("image/jpeg", 1.0);
    const pdf = new jspdf.jsPDF("p", "mm", "a4");
    const pageWidth = pdf.internal.pageSize.getWidth();
    const pageHeight = pdf.internal.pageSize.getHeight();
    const imgWidth = pageWidth;
    const imgHeight = (canvas.height * imgWidth) / canvas.width;
    let heightLeft = imgHeight;
    let position = 0;

    // First page
    pdf.addImage(imgData, "JPEG", 0, position, imgWidth, imgHeight);
    heightLeft -= pageHeight;

    // Additional pages
    while (heightLeft > 0) {
        position -= pageHeight;
        pdf.addPage();
        pdf.addImage(imgData, "JPEG", 0, position, imgWidth, imgHeight);
        heightLeft -= pageHeight;
    }

    // Remove clone
    document.body.removeChild(clone);
    pdf.save(fileName || "file.pdf");
}