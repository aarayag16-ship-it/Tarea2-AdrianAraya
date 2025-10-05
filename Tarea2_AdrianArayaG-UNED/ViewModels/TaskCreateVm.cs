using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Tarea2_AdrianArayaG_UNED.Domain;

namespace Tarea2_AdrianArayaG_UNED.ViewModels
{
    public class TaskCreateVm
    {
        [Required, StringLength(120)] public string Title { get; set; }
        [Required, StringLength(2000)] public string Description { get; set; }
        [Required] public List<string> Languages { get; set; }
        [Url] public string RepoUrl { get; set; }
        [Required] public TaskCategory Category { get; set; }
    }
}