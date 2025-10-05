using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tarea2_AdrianArayaG_UNED.Domain;
using Tarea2_AdrianArayaG_UNED.ViewModels;


namespace Tarea2_AdrianArayaG_UNED.Services
{
    public class TaskService
    {
        public IEnumerable<TaskItem> ApplyFilter(IEnumerable<TaskItem> all, FilterStateVm f)
        {
            if (f == null)
                return all.OrderByDescending(x => x.CreatedAt);

            if (!string.IsNullOrWhiteSpace(f.Query))
            {
                var q = f.Query.ToLower();
                all = all.Where(t =>
                    t.Title.ToLower().Contains(q) ||
                    t.Description.ToLower().Contains(q) ||
                    t.Languages.Any(l => l.ToLower().Contains(q))
                );
            }

            if (f.Category.HasValue)
                all = all.Where(t => t.Category == f.Category.Value);

            if (f.Languages != null && f.Languages.Any())
                all = all.Where(t =>
                    t.Languages.Intersect(f.Languages, System.StringComparer.OrdinalIgnoreCase).Any()
                );

            // CORRECCIÓN: switch clásico en lugar de switch expression
            switch (f.SortBy)
            {
                case "avg":
                    all = all.OrderByDescending(t => t.AverageScore);
                    break;
                case "title":
                    all = all.OrderBy(t => t.Title);
                    break;
                default:
                    all = all.OrderByDescending(t => t.CreatedAt);
                    break;
            }

            return all;
        }
    }
}