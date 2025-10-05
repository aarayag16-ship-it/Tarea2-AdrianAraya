using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tarea2_AdrianArayaG_UNED.Domain;

namespace Tarea2_AdrianArayaG_UNED.ViewModels
{
    public class TaskListVm
    {
        public IEnumerable<TaskItem> Items { get; set; }
        public FilterStateVm Filter { get; set; }
    }
}