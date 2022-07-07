function ShowImg(upLoad, preView) {
    if (upLoad.files && upLoad.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $(preView).attr('src', e.target.result);
        }
        reader.readAsDataURL(upLoad.files[0])
    }
}

function Showvideo(upLoad1, preViewvideo) {
    if (upLoad1.files && upLoad1.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $(preViewvideo).attr('src', e.target.result);
        }
        reader.readAsDataURL(upLoad1.files[0])
    }
}