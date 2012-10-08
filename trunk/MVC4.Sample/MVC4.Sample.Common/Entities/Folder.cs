using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVC4.Sample.Common.Entities
{
    public class RecursiveFolder
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
    }

    public class Folder
    {
        public string Name { get; set; }
        public List<Folder> Children { get; set; } 
    }
}
