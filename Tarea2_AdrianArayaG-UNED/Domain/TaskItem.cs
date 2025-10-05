using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Tarea2_AdrianArayaG_UNED.Domain
{
    public class TaskItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required, StringLength(120)] public string Title { get; set; }
        [Required, StringLength(2000)] public string Description { get; set; }
        [Required] public List<string> Languages { get; set; } = new List<string>();
        [Url] public string RepoUrl { get; set; }
        public string AttachmentFileName { get; set; }
        [Required] public TaskCategory Category { get; set; }
        public string AuthorUserName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public List<Rating> Ratings { get; set; } = new List<Rating>();
        public bool IsDeleted { get; set; }


        public double AverageScore => Ratings.Count == 0 ? 0 : Math.Round((double)Ratings.Sum(r => r.Score) / Ratings.Count, 2);
    }
}