using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tarea2_AdrianArayaG_UNED.Domain;

namespace Tarea2_AdrianArayaG_UNED.ViewModels
{
    public class FilterStateVm
    {
        public string Query { get; set; }
        public TaskCategory? Category { get; set; }
        public List<string> Languages { get; set; }
        public string SortBy { get; set; } // date|avg|title
        public int Page { get; set; } = 1;
    }
}