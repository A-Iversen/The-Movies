using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Movies.Model
{
    public class Cinema
    {
        public string Name { get; set; } = string.Empty;
        public List<Hall> Halls { get; set; } = new();
        public override string ToString() => Name;



        public Cinema(string name)
        {
            Name = name;
            Halls = new List<Hall>();
        }



    }

}
