using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tarea2_AdrianArayaG_UNED.Domain
{
    public class Rating
    {
        [Range(1, 5)] public int Score { get; set; }
        [Required, StringLength(800)] public string Comment { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}