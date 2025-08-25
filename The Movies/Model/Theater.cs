using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Movies.Model
{
    internal class Theater
    {
        public string Name { get; set; } = string.Empty;
        public List<string> Halls { get; set; } = new();
        public override string ToString() { return Name; }
    }
}
