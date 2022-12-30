console.log($("#avatar"))
var avatar_input = document.getElementById("avatar");
$('#avatar').change(function (e) {
    console.log(avatar_input.files);
    var url = URL.createObjectURL(avatar_input.files[0]);
    $('#img_avatar').attr('src', url);
    $(".div-add-avatar").attr("style",`background-image: url("${url}")`)
})