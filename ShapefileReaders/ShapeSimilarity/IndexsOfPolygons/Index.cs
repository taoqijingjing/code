using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapefileReaders.ShapeSimilarity.IndexsOfPolygons
{
    class Index
    {
        public Index(double val)
        {
            this.Val = val;
        }

        public Index(double val, string head)
        {
            this.Val = val;
            this.Head = head;
        }
        public double Val { get; set; }
        public string Head { get; set; }
    }
}
