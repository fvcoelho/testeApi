using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wallmart.Database
{
    public class Cities : List<City> { }

    public class City
    {
        public Int32 cityId { get; set; }

        public State state { get; set; }

        public string name { get; set; }

        public bool isCapital { get; set; }
    }
}
