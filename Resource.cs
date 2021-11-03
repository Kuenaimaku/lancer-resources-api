using System;
using System.Collections.Generic;

namespace lancer_resources_backend
{
    public class Resource
    {
        public int ResourceId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string URL { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public List<string> Tags { get; set; } = new List<string>();

    }
}
