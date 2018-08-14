using GeoAPI.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapefileReaders.ShapeSimilarity.Algorithm.Utils.Public
{
    class Vector
    {
        public static Coordinate addVector(IList<Coordinate> points, Coordinate origin)
        {
            Coordinate score = new Coordinate();
            if (points.Count == 1)
            {
                score = points[0];
            }
            else
            {
                for (int i = 0; i < points.Count; i++)
                {
                    score.X += points[i].X - origin.X;
                    score.Y += points[i].Y - origin.Y; 
                }
                score.X += origin.X;
                score.Y += origin.Y;
            }
            return score;
        }
    }
}
