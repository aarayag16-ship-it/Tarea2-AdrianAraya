window.toast = function (msg) {
    const $t = $('#toast');
    $t.stop(true, true).text(msg).fadeIn(200).delay(1800).fadeOut(400);
};


window.Tasks = (function () {
    $('#filterForm').on('submit', function (ev) {
        ev.preventDefault();
        const data = AppState.saveFromForm();
        $.ajax({ url: '/Tasks/ListFiltered', method: 'POST', data: data })
            .done(html => { $('#taskList').html(html); })
            .fail(() => toast('Error aplicando filtros'));
    });
}


// Autosave borrador
const DKEY = 'rt.taskDraft';
function draftGet() { try { return JSON.parse(localStorage.getItem(DKEY) || '{}'); } catch (e) { return {}; } }
function draftSet(v) { localStorage.setItem(DKEY, JSON.stringify(v || {})); }
function fillFromDraft() {
    const d = draftGet();
    if (d.Title) $('[name="Title"]').val(d.Title);
    if (d.Description) $('[name="Description"]').val(d.Description);
    if (d.RepoUrl) $('[name="RepoUrl"]').val(d.RepoUrl);
    if (d.Category) $('[name="Category"]').val(d.Category);
    if (d.Languages) $('#langs').val((d.Languages || []).join(','));
}
function trackInputs() {
    $('#createForm input, #createForm textarea, #createForm select').on('input change', function () {
        const d = {
            Title: $('[name="Title"]').val(),
            Description: $('[name="Description"]').val(),
            RepoUrl: $('[name="RepoUrl"]').val(),
            Category: $('[name="Category"]').val(),
            Languages: ($('#langs').val() || '').split(',').map(s => s.trim()).filter(Boolean)
        };
        draftSet(d);
    });
}


function initCreate() {
    fillFromDraft(); trackInputs();
    $('#createForm').on('submit', function (e) {
        e.preventDefault();
        const form = this;
        // Validación simple JS
        if (!$('[name="Title"]').val() || !$('[name="Description"]').val() || !$('[name="Category"]').val()) {
            toast('Completa los campos obligatorios'); return;
        }
        // Mapear lenguajes al post
        $('<input>').attr({ type: 'hidden', name: 'Languages[0]' }).val('dummy').appendTo(form); // evita null binding
        const langs = ($('#langs').val() || '').split(',').map(s => s.trim()).filter(Boolean);
        $(form).find('input[name^="Languages"]').remove();
        langs.forEach((l, i) => $('<input>').attr({ type: 'hidden', name: `Languages[${i}]` }).val(l).appendTo(form));


        const fd = new FormData(form);
        fd.append('__RequestVerificationToken', antif());
        $.ajax({ url: form.action, method: 'POST', data: fd, processData: false, contentType: false })
            .done(res => { if (res.ok) { localStorage.removeItem(DKEY); window.location = res.redirect; } else { toast('Error: ' + (res.errors || 'validación')); } })
            .fail(() => toast('Fallo al guardar'));
    });
}


function initEdit() {
    $('#editForm').on('submit', function (e) {
        e.preventDefault();
        const form = this;
        const langs = ($('#langs').val() || '').split(',').map(s => s.trim()).filter(Boolean);
        $(form).find('input[name^="Languages"]').remove();
        langs.forEach((l, i) => $('<input>').attr({ type: 'hidden', name: `Languages[${i}]` }).val(l).appendTo(form));
        $.ajax({ url: form.action, method: 'POST', data: $(form).serialize() + `&__RequestVerificationToken=${antif()}` })
            .done(res => { if (res.ok) window.location = res.redirect; else toast('Error al guardar'); })
            .fail(() => toast('Fallo al guardar'));
    });
}


function bindDeleteAjax() {
    $('#delForm').on('submit', function (e) {
        e.preventDefault();
        const form = this;
        $.ajax({ url: form.action, method: 'POST', data: $(form).serialize() })
            .done(res => { if (res.ok) window.location = res.redirect; else toast('No se pudo eliminar'); })
            .fail(() => toast('Fallo al eliminar'));
    });
}


window.toast = function (msg) { $('#toast').text(msg).fadeIn(200).delay(1500).fadeOut(400); };


return { initCreate, initEdit, bindFilterAjax, bindDeleteAjax };
}) ();