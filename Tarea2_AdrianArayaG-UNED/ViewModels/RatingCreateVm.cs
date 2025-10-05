using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Tarea2_AdrianArayaG_UNED.ViewModels
{
    public class RatingCreateVm
    {
        [Range(1, 5)] public int Score { get; set; }
        [Required, StringLength(800)] public string Comment { get; set; }
        [Required] public string TaskId { get; set; }
    }
}