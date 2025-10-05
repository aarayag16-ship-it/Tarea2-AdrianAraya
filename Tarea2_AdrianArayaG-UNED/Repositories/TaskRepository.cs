using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tarea2_AdrianArayaG_UNED.Domain;

namespace Tarea2_AdrianArayaG_UNED.Repositories
{
    public class TaskRepository
    {
        private readonly JsonFileStore<TaskItem> _store = new JsonFileStore<TaskItem>("~/App_Data/tasks.json");


        public IEnumerable<TaskItem> All() => _store.Load().Where(t => !t.IsDeleted);
        public TaskItem Get(Guid id) => All().FirstOrDefault(t => t.Id == id);
        public void Add(TaskItem item)
        {
            var all = _store.Load();
            all.Add(item);
            _store.Save(all);
        }
        public void Update(TaskItem item)
        {
            var all = _store.Load();
            var ix = all.FindIndex(t => t.Id == item.Id);
            if (ix >= 0) { all[ix] = item; _store.Save(all); }
        }
        public void SoftDelete(Guid id, string byUser)
        {
            var all = _store.Load();
            var it = all.FirstOrDefault(t => t.Id == id);
            if (it != null && it.AuthorUserName == byUser) { it.IsDeleted = true; _store.Save(all); }
        }
        public void AddRating(Guid taskId, Rating r)
        {
            var all = _store.Load();
            var it = all.FirstOrDefault(t => t.Id == taskId);
            if (it == null) return;
            // Evitar doble rating del mismo usuario: actualiza si existe
            var existing = it.Ratings.FirstOrDefault(x => x.UserName == r.UserName);
            if (existing != null) { existing.Score = r.Score; existing.Comment = r.Comment; existing.CreatedAt = DateTime.UtcNow; }
            else it.Ratings.Add(r);
            _store.Save(all);
        }
    }
}