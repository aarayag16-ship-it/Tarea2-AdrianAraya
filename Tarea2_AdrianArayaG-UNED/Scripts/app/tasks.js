window.toast = function (msg) {
    const $t = $('#toast');
    $t.stop(true, true).text(msg).fadeIn(200).delay(1800).fadeOut(400);
};
