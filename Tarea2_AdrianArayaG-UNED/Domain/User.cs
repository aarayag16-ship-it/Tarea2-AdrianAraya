using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tarea2_AdrianArayaG_UNED.Domain
{
    public class User
    {
        [Required, StringLength(40)] public string UserName { get; set; }
        [Required, StringLength(120)] public string DisplayName { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public List<string> Roles { get; set; } = new List<string> { "Estudiante" };
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}