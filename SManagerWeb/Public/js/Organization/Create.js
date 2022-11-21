const imageLogo = document.getElementById("file-preview");
function showPreview(evt) {
    var files = evt.target.files;
    var file = files[0];

    var fileReader = new FileReader();

    fileReader.onload = function (progressEvent) {
        var url = fileReader.result;
        imageLogo.src = url;
        $("#file-preview").css("display", "block");
        $(".labeltest").css("display", "none");
    }

    fileReader.readAsDataURL(file);

    
}