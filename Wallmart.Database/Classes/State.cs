using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wallmart.Database
{
    public class States : List <State> { };
    
    public class State
    {
        public Int32 stateId { get; set; }

        public string name { get; set; }

        public string country { get; set; }

        public string abbreviation { get; set; }

        public string region { get; set; }

        public override string ToString()
        {
            return string.Format("stateId={0}, name={1}, country={2}, abbreviation={3}, region={4}",
                stateId.ToString(),
                name,
                country,
                abbreviation,
                region);
        }
    }
}
