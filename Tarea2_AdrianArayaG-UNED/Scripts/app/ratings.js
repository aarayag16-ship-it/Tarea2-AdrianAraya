window.Ratings = (function () {
    function antif() { return $('input[name=__RequestVerificationToken]').val(); }
    function bind(taskId) {
        $('#rateForm').on('submit', function (e) {
            e.preventDefault();
            const data = $(this).serialize() + `&__RequestVerificationToken=${antif()}`;
            $.post('/Ratings/Create', data).done(res => {
                if (res.ok) {
                    $('#avg').text(Number(res.avg).toFixed(2));
                    // Prepend last comment quickly
                    const score = $('[name=Score]').val();
                    const comment = $('<div>').text($('[name=Comment]').val()).html();
                    $('#ratingList').prepend(`<li><b>${score} ★</b> — ${comment}<div class="meta">por tú · ahora</div></li>`);
                    $('#rateForm')[0].reset();
                    toast('¡Gracias por tu calificación!');
                } else { toast('Error en calificación'); }
            }).fail(() => toast('Fallo al enviar'));
        });
    }
    return { bind };
})();