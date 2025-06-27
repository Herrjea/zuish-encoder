mergeInto(LibraryManager.library, {
    DownloadFile: function(arrayPtr, length, fileNamePtr) {
        var bytes = new Uint8Array(Module.HEAPU8.buffer, arrayPtr, length);
        var blob = new Blob([bytes], { type: "image/png" });
        var link = document.createElement("a");

        var fileName = UTF8ToString(fileNamePtr);
        link.href = URL.createObjectURL(blob);
        link.download = fileName;
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    }
});
