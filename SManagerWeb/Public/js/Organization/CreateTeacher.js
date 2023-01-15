console.log($("#avatar"))
var avatar_input = document.getElementById("avatar");
$('#avatar').change(function (e) {
   
    var url = URL.createObjectURL(avatar_input.files[0]);
    $('#img_avatar').attr('src', url);
    $(".div-add-avatar").attr("style",`background-image: url("${url}")`)
})

window.onload = function () {
    let src = $('#img_avatar').attr('src');
    if (src.length > 0) {
        $(".div-add-avatar").attr("style", `background-image: url("${src}")`)
    }
}