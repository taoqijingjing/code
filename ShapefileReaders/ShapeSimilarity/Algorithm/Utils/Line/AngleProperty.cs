using GeoAPI.Geometries;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using ShapefileReaders.ShapeSimilarity.Algorithm.Utils.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapefileReaders.ShapeSimilarity.Algorithm.Utils.Line
{
    class AngleProperty
    {
        public AngleProperty(IFeature feature)
        {
            getAngleLines(feature);
            getPolygons(feature);
            getOutArea(feature);
            getTitio(feature);
        }
        public IList<ILineString> Lines = new List<ILineString>();
        public IList<IGeometry> Polygens = new List<IGeometry>();
        public IList<IGeometry> Polygens1 = new List<IGeometry>();
        public IList<double> outArea = new List<double>();
        public IList<double> Titio = new List<double>();

        private void getAngleLines(IFeature feature)
        {
            IList<ILineString> lines = new List<ILineString>();
            Coordinate centroid = feature.Geometry.Centroid.Coordinate;
            Coordinate origin = new Coordinate();
            origin.X = MaxPoints.getMaxVector(feature).Coordinates[1].X - centroid.X;
            origin.Y = MaxPoints.getMaxVector(feature).Coordinates[1].Y - centroid.Y;    //最大点平移到自定义坐标系

            for (int i = 5; i <= 361; i += 5)
            {
                double angle = getAngle(origin);
                double k = 0;
                angle += i;

                if (angle > 360)  //若角大于360，减去360
                {
                    angle -= 360;
                }

                k = Math.Tan(angle * Math.PI / 180);


                Coordinate point = new Coordinate();  //判定每隔5度的点落在1.4象限，2，3象限，或者X轴上
                if (angle > 90 && angle < 270)
                {
                    point.X = -100000.0;
                    point.Y = k * point.X;
                }
                else if ((angle >= 0 && angle < 90) || (angle > 270 && angle <= 360))
                {
                    point.X = 100000.0;
                    point.Y = k * point.X;
                }
                else if (angle == 90)
                {
                    point.X = 0;
                    point.Y = 100000.0;
                }
                else if (angle == 270)
                {
                    point.X = 0;
                    point.Y = -100000.0;
                }
                point.X += centroid.X; //将点移动回到原空间投影坐标系
                point.Y += centroid.Y;




                Coordinate[] points = new Coordinate[] { centroid, point };
                LineString line = new LineString(points);
                if (!line.Intersects(feature.Geometry.Boundary)) //如果所得线段与边界不相交，跳出当前循环，进入下个循环
                {
                    continue;
                }

                IGeometry intersectPoints = line.Intersection(feature.Geometry.Boundary);

                if (intersectPoints.Coordinates.Length > 1)
                {
                    if (points[1].X > points[0].X)
                    {
                        line.Coordinates[1] = intersectPoints.Coordinates[intersectPoints.Coordinates.Length - 1];
                    }
                    else if (points[1].X < points[0].X)
                    {
                        line.Coordinates[1] = intersectPoints.Coordinates[0];
                    }
                    else if (points[1].X == points[0].X)
                    {
                        if (points[1].Y > points[0].Y)
                        {
                            line.Coordinates[1] = intersectPoints.Coordinates[intersectPoints.Coordinates.Length - 1];
                        }
                        else if (points[1].Y < points[0].Y)
                        {
                            line.Coordinates[1] = intersectPoints.Coordinates[0];
                        }
                    }
                }
                else
                {
                    line.Coordinates[1] = intersectPoints.Coordinate;
                }
                lines.Add(line);
            }
            Lines = lines;
        }


        public static double getAngle(Coordinate point)
        {
            double angle = 0;
            if (point.X > 0 && point.Y >= 0)
            {
                angle = Math.Atan2(point.Y, point.X) * 180 / Math.PI;
            }
            else if (point.X < 0 && point.Y > 0)
            {
                angle = Math.Atan2(point.Y, -point.X) * 180 / Math.PI;
                angle = 180 - angle;
            }
            else if (point.X < 0 && point.Y <= 0)
            {
                angle = Math.Atan2(-point.Y, -point.X) * 180 / Math.PI;
                angle += 180;
            }
            else
            {
                angle = Math.Atan2(-point.Y, point.X) * 180 / Math.PI;
                angle = 360 - angle;
            }
            return angle;
        }


        private void getPolygons(IFeature feature)
        {
            IList<ILineString> lines = new List<ILineString>();
            Coordinate centroid = feature.Geometry.Centroid.Coordinate;
            Coordinate origin = new Coordinate();
            origin.X = MaxPoints.getMaxVector(feature).Coordinates[1].X - centroid.X;
            origin.Y = MaxPoints.getMaxVector(feature).Coordinates[1].Y - centroid.Y;    //最大点平移到自定义坐标系
            for (int i = 5; i <= 361; i += 5)
            {
                double angle = getAngle(origin);
                double k = 0;
                angle += i;
                if (angle > 360)  //若角大于360，减去360
                {
                    angle -= 360;
                }
                k = Math.Tan(angle * Math.PI / 180);
                Coordinate point = new Coordinate();  //判定每隔5度的点落在1.4象限，2，3象限，或者X轴上
                if (angle > 90 && angle < 270)
                {
                    point.X = -100000.0;
                    point.Y = k * point.X;
                }
                else if ((angle >= 0 && angle < 90) || (angle > 270 && angle <= 360))
                {
                    point.X = 100000.0;
                    point.Y = k * point.X;
                }
                else if (angle == 90)
                {
                    point.X = 0;
                    point.Y = 100000.0;
                }
                else if (angle == 270)
                {
                    point.X = 0;
                    point.Y = -100000.0;
                }
                point.X += centroid.X; //将点移动回到原空间投影坐标系
                point.Y += centroid.Y;
                Coordinate[] points = new Coordinate[] { centroid, point };
                LineString line = new LineString(points);
                if (!line.Intersects(feature.Geometry.Boundary)) //如果所得线段与边界不相交，跳出当前循环，进入下个循环
                {
                    continue;
                }
                lines.Add(line);
            }
            lines.Add(lines[0]);
            for (int i = 0; i < lines.Count - 1; i++)
            {
                Coordinate[] points = new Coordinate[] { centroid, lines[i].Coordinates[1], lines[i + 1].Coordinates[1], centroid };
                ILinearRing triangle = new LinearRing(points);
                Polygon angleArea = new Polygon(triangle);
                IGeometry area = angleArea.Intersection(feature.Geometry);
                Polygens.Add(area);   //两条线与多边形交出的多边形面积     
            }
        }

        private void getOutArea(IFeature feature)
        {
            IList<double> outP = new List<double>();
            IList<double> outMin = new List<double>();
            IList<IGeometry> outPolygons = new List<IGeometry>();
            outPolygons = intPolygon.getOutPolygons(feature);
            IList<IGeometry> minPolygons = new List<IGeometry>();
            minPolygons = intPolygon.getMinPolygons(feature);
            foreach (var item in outPolygons)
            {
                outP.Add(item.Area);
            }
            foreach (var item in minPolygons)
            {
                outMin.Add(item.Area);
            }
            for (int i = 0; i < outP.Count; i++)
            {
                outArea.Add(outP[i] - outMin[i]);
            }
        }

        private void getTitio(IFeature feature)
        {
            IList<ILineString> lines = new List<ILineString>();
            Coordinate centroid = feature.Geometry.Centroid.Coordinate;
            Coordinate origin = new Coordinate();
            origin.X = MaxPoints.getMaxVector(feature).Coordinates[1].X - centroid.X;
            origin.Y = MaxPoints.getMaxVector(feature).Coordinates[1].Y - centroid.Y;    //最大点平移到自定义坐标系

            for (int i = 5; i <= 361; i += 5)
            {
                double angle = getAngle(origin);
                double k = 0;
                angle += i;

                if (angle > 360)  //若角大于360，减去360
                {
                    angle -= 360;
                }

                k = Math.Tan(angle * Math.PI / 180);


                Coordinate point = new Coordinate();  //判定每隔5度的点落在1.4象限，2，3象限，或者X轴上
                if (angle > 90 && angle < 270)
                {
                    point.X = -100000.0;
                    point.Y = k * point.X;
                }
                else if ((angle >= 0 && angle < 90) || (angle > 270 && angle <= 360))
                {
                    point.X = 100000.0;
                    point.Y = k * point.X;
                }
                else if (angle == 90)
                {
                    point.X = 0;
                    point.Y = 100000.0;
                }
                else if (angle == 270)
                {
                    point.X = 0;
                    point.Y = -100000.0;
                }
                point.X += centroid.X; //将点移动回到原空间投影坐标系
                point.Y += centroid.Y;




                Coordinate[] points = new Coordinate[] { centroid, point };
                LineString line = new LineString(points);
                if (!line.Intersects(feature.Geometry.Boundary)) //如果所得线段与边界不相交，跳出当前循环，进入下个循环
                {
                    continue;
                }

                IGeometry intersectPoints = line.Intersection(feature.Geometry.Boundary);

                if (intersectPoints.Coordinates.Length > 1)
                {
                    if (points[1].X > points[0].X)
                    {
                        line.Coordinates[1] = intersectPoints.Coordinates[intersectPoints.Coordinates.Length - 1];
                    }
                    else if (points[1].X < points[0].X)
                    {
                        line.Coordinates[1] = intersectPoints.Coordinates[0];
                    }
                    else if (points[1].X == points[0].X)
                    {
                        if (points[1].Y > points[0].Y)
                        {
                            line.Coordinates[1] = intersectPoints.Coordinates[intersectPoints.Coordinates.Length - 1];
                        }
                        else if (points[1].Y < points[0].Y)
                        {
                            line.Coordinates[1] = intersectPoints.Coordinates[0];
                        }
                    }
                }
                else
                {
                    line.Coordinates[1] = intersectPoints.Coordinate;
                }
                lines.Add(line);
            }

            lines.Add(lines[0]);


            double titio = 0;
            for (int i = 0; i < lines.Count - 1; i++)
            {
                double b = centroid.Distance(lines[i + 1].Coordinates[1]);
                double a = centroid.Distance(lines[i].Coordinates[1]);
                double h = b * Math.Sin(5 * Math.PI / 180);
                titio = 1 - h / (h + a);
                Titio.Add(titio);
            }

        }
        private void getMaxMinAngle(IFeature feature)
        {
            IList<ILineString> lines = new List<ILineString>();
            Coordinate centroid = feature.Geometry.Centroid.Coordinate;
            Coordinate origin = new Coordinate();
            origin.X = MaxPoints.getMaxVector(feature).Coordinates[1].X - centroid.X;
            origin.Y = MaxPoints.getMaxVector(feature).Coordinates[1].Y - centroid.Y;    //最大点平移到自定义坐标系

            for (int i = 5; i <= 361; i += 5)
            {
                double angle = getAngle(origin);
                double k = 0;
                angle += i;

                if (angle > 360)  //若角大于360，减去360
                {
                    angle -= 360;
                }

                k = Math.Tan(angle * Math.PI / 180);


                Coordinate point = new Coordinate();  //判定每隔5度的点落在1.4象限，2，3象限，或者X轴上
                if (angle > 90 && angle < 270)
                {
                    point.X = -100000.0;
                    point.Y = k * point.X;
                }
                else if ((angle >= 0 && angle < 90) || (angle > 270 && angle <= 360))
                {
                    point.X = 100000.0;
                    point.Y = k * point.X;
                }
                else if (angle == 90)
                {
                    point.X = 0;
                    point.Y = 100000.0;
                }
                else if (angle == 270)
                {
                    point.X = 0;
                    point.Y = -100000.0;
                }
                point.X += centroid.X; //将点移动回到原空间投影坐标系
                point.Y += centroid.Y;




                Coordinate[] points = new Coordinate[] { centroid, point };
                LineString line = new LineString(points);
                if (!line.Intersects(feature.Geometry.Boundary)) //如果所得线段与边界不相交，跳出当前循环，进入下个循环
                {
                    continue;
                }

                IGeometry intersectPoints = line.Intersection(feature.Geometry.Boundary);

                if (intersectPoints.Coordinates.Length > 1)
                {
                    if (points[1].X > points[0].X)
                    {
                        line.Coordinates[1] = intersectPoints.Coordinates[intersectPoints.Coordinates.Length - 1];
                    }
                    else if (points[1].X < points[0].X)
                    {
                        line.Coordinates[1] = intersectPoints.Coordinates[0];
                    }
                    else if (points[1].X == points[0].X)
                    {
                        if (points[1].Y > points[0].Y)
                        {
                            line.Coordinates[1] = intersectPoints.Coordinates[intersectPoints.Coordinates.Length - 1];
                        }
                        else if (points[1].Y < points[0].Y)
                        {
                            line.Coordinates[1] = intersectPoints.Coordinates[0];
                        }
                    }
                }
                else
                {
                    line.Coordinates[1] = intersectPoints.Coordinate;
                }
                lines.Add(line);
            }

            lines.Add(lines[0]);


            double titio = 0;
            for (int i = 0; i < lines.Count - 1; i++)
            {
                double b = centroid.Distance(lines[i + 1].Coordinates[1]);
                double a = centroid.Distance(lines[i].Coordinates[1]);
                double h = b * Math.Sin(5 * Math.PI / 180);
                titio = 1 - h / (h + a);
                Titio.Add(titio);
            }

        }
    }
}
