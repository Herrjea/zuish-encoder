mergeInto(LibraryManager.library, {
  UploadTextFile: function(callbackId) {
    // Create a file input element
    var input = document.createElement('input');
    input.type = 'file';
    input.accept = '.txt';
    input.style.display = 'none';

    input.onchange = function(event) {
      var file = event.target.files[0];
      if (!file) return;

      var reader = new FileReader();
      reader.onload = function(e) {
        var content = e.target.result;
        // Send the file content back to Unity
        SendMessage("Manager", "OnFileSelected", content);
      };
      reader.readAsText(file);
    };

    document.body.appendChild(input);
    input.click();
    document.body.removeChild(input);
  }
});