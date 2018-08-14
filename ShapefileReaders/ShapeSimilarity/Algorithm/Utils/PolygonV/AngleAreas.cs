using GeoAPI.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapefileReaders.ShapeSimilarity.Algorithm.Utils.PolygonV
{
    class AngleAreas
    {
        private IList<double> areas = new List<double>();
        public IList<double> Areas = new List<double>();

        public AngleAreas(IList<ILineString> line)
        {
            setAreas(line);
        }
        private void setAreas(IList<ILineString> lines)
        {
            if (lines.Count <= 3)
            {
                Console.WriteLine("此图形可能不是个面");
            }
            else
            {
                lines.Add(lines[0]);
                for (int i = 0; i < lines.Count - 1; i++)
                {
                    double area = 0;
                    double a = lines[i].Coordinates[0].Distance(lines[i].Coordinates[1]);
                    double b = lines[i + 1].Coordinates[0].Distance(lines[i + 1].Coordinates[1]);
                    area = 0.5 * a * b * Math.Sin(5 * Math.PI / 180);
                    Areas.Add(area);
                }
            }

         
        } 
    }
}
