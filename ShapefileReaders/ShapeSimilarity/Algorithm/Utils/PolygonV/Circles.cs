using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapefileReaders.ShapeSimilarity.Algorithm.Utils.PolygonV
{
    class Circles
    {
        public static LineString getCircle(Coordinate center, Coordinate point)
        {
            double dis = center.Distance(point);
            List<double> X = new List<double>();
            double count = 0;
            IList<Coordinate> POINTS = new List<Coordinate>();            
            double x = center.X - dis;
            while (count < dis * 2)
            {
                X.Add(x);
                count += 0.1;
                x += 0.1;
            }
            for (int i = 0; i < X.Count; i++)
            {
                Coordinate coor = new Coordinate();             
                double Y = 0;
                Y = Math.Sqrt(Math.Abs(Math.Pow(dis, 2) - Math.Pow(X[i] - center.X, 2))) + center.Y;
                coor.X = X[i];
                coor.Y = Y;
                POINTS.Add(coor);
            }
            for (int i = X.Count - 2; i > 0; i--)
            {
                Coordinate coor = new Coordinate();
                double Y = 0;
                Y = -Math.Sqrt(Math.Abs( Math.Pow(dis, 2) - Math.Pow(X[i] - center.X, 2) )) + center.Y;
                coor.X = X[i];
                coor.Y = Y;
                POINTS.Add(coor);
            }
            POINTS.Add(POINTS[0]);
            Coordinate[] coordinates = POINTS.ToArray<Coordinate>();
            LineString ring = new LinearRing(coordinates);          
            return ring;
        }
    }
}
