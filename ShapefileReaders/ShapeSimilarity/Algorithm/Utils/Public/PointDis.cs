using GeoAPI.Geometries;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapefileReaders.ShapeSimilarity.Algorithm.Utils.Public
{
    class PointDis
    {
        public static List<Coordinate> getMaxPoints(IFeature feature)
        {
            double max = 0;
            List<Coordinate> points = new List<Coordinate>();
            Coordinate point = new Coordinate();
            for (int i = 1; i < feature.Geometry.NumPoints; i++)
            {
                double dis = feature.Geometry.Centroid.Coordinate.Distance(feature.Geometry.Coordinates[i]);
                if (max < dis)
                {
                    max = dis;
                    point = feature.Geometry.Coordinates[i];
                }
            }
            points.Add(point);

            for (int i = 1; i < feature.Geometry.NumPoints; i++)
            {
                double dis = feature.Geometry.Centroid.Coordinate.Distance(feature.Geometry.Coordinates[i]);
                if (max == dis)
                {
                    points.Add(feature.Geometry.Coordinates[i]);
                }
            }

            return points;
        }  //获取一个小班距重心最远点

        public static Coordinate getMinPoints(IFeature feature)
        {
            List<Coordinate> coors =  new List<Coordinate>(feature.Geometry.Coordinates);
            coors.Add(feature.Geometry.Coordinates[0]);
            double min = 1000000;
            Coordinate score = new Coordinate();
            LineString minLine = null;

            for (int i = 0; i < coors.Count - 1; i++)
            {
                double dis = 0;
                LineString line1 = new LineString(new Coordinate[] {coors[i], coors[i + 1] });
                Coordinate pedal = getPedal(line1, feature.Geometry.Centroid.Coordinate);
                LineString line2 = new LineString(new Coordinate[] { feature.Geometry.Centroid.Coordinate, pedal });
                if (line1.Intersects(line2))
                {
                    dis = feature.Geometry.Centroid.Coordinate.Distance(pedal);
                    if (min > dis)
                    {
                        min = dis;
                        score = pedal;
                        minLine = line1;
                        continue;
                    }
                }

            }

            return score;
        }

        public static Coordinate getMinPoints_CZD(IFeature feature)
        {
            List<Coordinate> coors = new List<Coordinate>(feature.Geometry.Coordinates);
            coors.Add(feature.Geometry.Coordinates[0]);
            double mindis = 1000000;
            LineString minLine = null;
            for (int i = 0; i < coors.Count - 1; i++)
            {
                LineString line1 = new LineString(new Coordinate[] { coors[i], coors[i + 1] });
                Coordinate pedal = getPedal(line1, feature.Geometry.Centroid.Coordinate);
                Coordinate minPoint = getMinPointOnLine(line1, pedal, feature.Geometry.Centroid.Coordinate);

                double dis = getDis(minPoint, feature.Geometry.Centroid.Coordinate);
                if (mindis > dis)
                {
                    mindis = dis;
                    minLine = line1;
                    continue;
                }
            }

           
            //C: (x-a)2+(y-b)2 = r2
            //L: y=kx+c

            //IGeometry buf_c = feature.Geometry.Centroid.Buffer(mindis);
            //if (buf_c.Intersects(feature.Geometry))
            //    Console.WriteLine("in");
            //else
            //    Console.WriteLine("error");
            //Console.ReadLine();
            double a = feature.Geometry.Centroid.X;
            double b = feature.Geometry.Centroid.Y;
            double r = mindis;


            double k = (minLine.Coordinates[0].Y - minLine.Coordinates[1].Y) / (minLine.Coordinates[0].X - minLine.Coordinates[1].X);
            double c = minLine.Coordinates[0].Y - k * minLine.Coordinates[0].X;

            Coordinate score = new Coordinate();
            score.X = (a - c * k + b * k) / (1 + k * k);
            score.Y = k * score.X + c;

            return score;
        }


        public static Coordinate getMinPoints_ZYM(IFeature feature)
        {
            List<Coordinate> coors = new List<Coordinate>(feature.Geometry.Coordinates);
            coors.Add(feature.Geometry.Coordinates[0]);
            double mindis = 1000000;
            Coordinate score = new Coordinate();
            for (int i = 0; i < coors.Count - 1; i++)
            {
                LineString line1 = new LineString(new Coordinate[] { coors[i], coors[i + 1] });
                Coordinate pedal = getPedal(line1, feature.Geometry.Centroid.Coordinate);
                Coordinate minPoint = getMinPointOnLine(line1, pedal, feature.Geometry.Centroid.Coordinate);

                double dis = getDis(minPoint, feature.Geometry.Centroid.Coordinate);
                if (mindis > dis)
                {
                    mindis = dis;
                    score = minPoint;
                    continue;
                }
            }


            return score;
        }

        public static Coordinate getPedal(LineString line, Coordinate coor)
        {
            double x1 = line.Coordinates[0].X;
            double y1 = line.Coordinates[0].Y;
            double x2 = line.Coordinates[1].X;
            double y2 = line.Coordinates[1].Y;


            double b = coor.Y - coor.X * (x1 - x2) / (y2 - y1);
            double k = (x1 - x2) / (y2 - y1);
            Coordinate score = new Coordinate();
            score.X = ((b - y2) * x1 + (y1 - b) * x2) / (k * (x1 - x2) + y1 - y2);
            score.Y = k * score.X + b;



            return score;

        }



        public static Coordinate getMinPointOnLine(LineString line, Coordinate pedal, Coordinate centroied)
        {
            LineString line2 = new LineString(new Coordinate[] {pedal, centroied});
            if (intersects(line, pedal))
            {
                return pedal;
            }

            double dis1 = centroied.Distance(line.Coordinates[0]);
            double dis2 = centroied.Distance(line.Coordinates[1]);
            if (dis1 > dis2)
            {
                return line.Coordinates[1];
            }
            else
            {
                return line.Coordinates[0];
            }
        }

        public static bool intersects(LineString line, Coordinate pedal)
        {
            double x1 = line.Coordinates[0].X;
            double x2 = line.Coordinates[1].X;
            if (x1 < x2)
            {
                if (pedal.X > x1 && pedal.X < x2)
                {
                    return true;
                }
                else
                    return false;
            }

            else
            {
                if (pedal.X > x2 && pedal.X < x1)
                {
                    return true;
                }
                else
                    return false;
            }
        }

        public static double getDis(Coordinate pedal, Coordinate centroid)
        {
            return pedal.Distance(centroid);
            
        }
    }
}
