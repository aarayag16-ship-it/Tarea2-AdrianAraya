window.AppState = (function () {
    const KEY = 'rt.filters';
    function get() { try { return JSON.parse(localStorage.getItem(KEY) || '{}'); } catch (e) { return {}; } }
    function set(v) { localStorage.setItem(KEY, JSON.stringify(v || {})); }
    function initFilters() {
        // Cargar de servidor (Session) y luego aplicar cliente
        $.getJSON('/State/GetFilter', function (server) {
            const client = get();
            const f = Object.assign({}, server || {}, client || {});
            // Pintar en el form
            const $f = $('#filterForm'); if (!$f.length) return;
            if (f.Query) $f.find('[name=Query]').val(f.Query);
            if (f.Category) $f.find('[name=Category]').val(f.Category);
            if (f.Languages && Array.isArray(f.Languages)) $f.find('[name=Languages]').val(f.Languages.join(','));
            if (f.SortBy) $f.find('[name=SortBy]').val(f.SortBy);
        });
    }
    function saveFromForm() {
        const $f = $('#filterForm');
        const data = {
            Query: $f.find('[name=Query]').val() || '',
            Category: $f.find('[name=Category]').val() || null,
            Languages: ($f.find('[name=Languages]').val() || '').split(',').map(s => s.trim()).filter(Boolean),
            SortBy: $f.find('[name=SortBy]').val() || 'date'
        };
        set(data);
        $.post('/State/SaveFilter', data);
        return data;
    }
    return { initFilters, saveFromForm };
})();


window.Recent = (function () {
    const KEY = 'rt.recent'; const MAX = 8;
    function list() { try { return JSON.parse(localStorage.getItem(KEY) || '[]'); } catch (e) { return []; } }
    function save(arr) { localStorage.setItem(KEY, JSON.stringify(arr)); }
    function add(id, title) {
        let arr = list().filter(x => x.id !== id);
        arr.unshift({ id, title, ts: Date.now() });
        if (arr.length > MAX) arr = arr.slice(0, MAX);
        save(arr);
    }
    function render() {
        const ul = document.getElementById('recentList'); if (!ul) return;
        ul.innerHTML = '';
        list().forEach(x => {
            const li = document.createElement('li');
            li.innerHTML = `<a href="/Tasks/Details/${x.id}">${$('<div>').text(x.title).html()}</a>`;
            ul.appendChild(li);
        });
    }
    function init() { render(); }
    return { add, init };
})();