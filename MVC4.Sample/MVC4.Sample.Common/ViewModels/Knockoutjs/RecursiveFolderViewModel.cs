using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MVC4.Sample.Common.Entities;

namespace MVC4.Sample.Common.ViewModels.Knockoutjs
{
    public class RecursiveFolderViewModel
    {
        public List<RecursiveFolder> Folders { get; set; }
        public RecursiveFolder Template { get; set; }
    }
}
