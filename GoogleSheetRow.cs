using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lancer_resources_backend
{
    public class GoogleSheetResponse
    {
        public string Range { get; set; }
        public string MajorDimension { get; set; }
        public List<List<string>> Values { get; set; } = new List<List<string>>();
    }

    public class GoogleSheetRow
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string URL { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Tags { get; set; } 
    }
}
