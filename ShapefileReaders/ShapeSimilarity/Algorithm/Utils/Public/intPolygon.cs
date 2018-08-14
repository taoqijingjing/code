using GeoAPI.Geometries;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using ShapefileReaders.ShapeSimilarity.Algorithm.Utils.PolygonV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapefileReaders.ShapeSimilarity.Algorithm.Utils.Public
{
    public static class intPolygon
    {

        public static IList<IGeometry> getintPolygons(IFeature feature)
        {
            IList<IGeometry> intPolygons = new List<IGeometry>();
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
            lines.Add(lines[0]);//某个多边形每个5度的线

            for (int i = 0; i < lines.Count - 1; i++)
            {
                Coordinate[] points = new Coordinate[] { centroid, lines[i].Coordinates[1], lines[i + 1].Coordinates[1], centroid };
                ILinearRing triangle = new LinearRing(points);
                Polygon angleArea = new Polygon(triangle);

                ILineString minLine = MinPoints.getMinVector(feature);
                LineString mincircle = Circles.getCircle(minLine.Coordinates[0], minLine.Coordinates[1]);
                ILinearRing minCir = new LinearRing(mincircle.Coordinates);
                Polygon minC = new Polygon(minCir);
                IGeometry area = angleArea.Intersection(minC);
                intPolygons.Add(area);   //线与内圆交出的多边形                 
            }
            return intPolygons;
        }

        public static IList<IGeometry> getOutPolygons(IFeature feature)
        {
            IList<IGeometry> outPolygons = new List<IGeometry>();
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
            lines.Add(lines[0]);//某个多边形每个5度的线

            for (int i = 0; i < lines.Count - 1; i++)
            {
                Coordinate[] points = new Coordinate[] { centroid, lines[i].Coordinates[1], lines[i + 1].Coordinates[1], centroid };
                ILinearRing triangle = new LinearRing(points);
                Polygon angleArea = new Polygon(triangle);

                ILineString maxLine = MaxPoints.getMaxVector(feature);
                LineString maxcircle = Circles.getCircle(maxLine.Coordinates[0], maxLine.Coordinates[1]);
                ILinearRing maxCir = new LinearRing(maxcircle.Coordinates);
                Polygon maxC = new Polygon(maxCir);
                IGeometry area = angleArea.Intersection(maxC);
                outPolygons.Add(area);   //线与外圆交出的多边形                 
            }
            return outPolygons;
        }

        public static IList<IGeometry> getMinPolygons(IFeature feature)
        {
            IList<IGeometry> minPolygons = new List<IGeometry>();
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
            lines.Add(lines[0]);//某个多边形每个5度的线

            for (int i = 0; i < lines.Count - 1; i++)
            {
                Coordinate[] points = new Coordinate[] { centroid, lines[i].Coordinates[1], lines[i + 1].Coordinates[1], centroid };
                ILinearRing triangle = new LinearRing(points);
                Polygon angleArea = new Polygon(triangle);

                //ILineString maxLine = MaxPoints.getMaxVector(feature);
                //LineString maxcircle = Circles.getCircle(maxLine.Coordinates[0], maxLine.Coordinates[1]);
                //ILinearRing maxCir = new LinearRing(maxcircle.Coordinates);
                //Polygon maxC = new Polygon(maxCir);
                IGeometry area = angleArea.Intersection(feature.Geometry);
                minPolygons.Add(area);   //线与多边形交出的多边形                 
            }
            return minPolygons;
        }

        public static IList<IGeometry> getInterBoundaryPolygons(IFeature feature)
        {
            IList<IGeometry> minPolygons = new List<IGeometry>();
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
            lines.Add(lines[0]);//某个多边形每个5度的线

            for (int i = 0; i < lines.Count - 1; i++)
            {
                Coordinate[] points = new Coordinate[] { centroid, lines[i].Coordinates[1], lines[i + 1].Coordinates[1], centroid };
                ILinearRing triangle = new LinearRing(points);
                Polygon angleArea = new Polygon(triangle);

                //ILineString maxLine = MaxPoints.getMaxVector(feature);
                //LineString maxcircle = Circles.getCircle(maxLine.Coordinates[0], maxLine.Coordinates[1]);
                //ILinearRing maxCir = new LinearRing(maxcircle.Coordinates);
                //Polygon maxC = new Polygon(maxCir);
                IGeometry area = angleArea.Intersection(feature.Geometry.Boundary);
                minPolygons.Add(area);   //线与多边形交出的多边形                 
            }
            return minPolygons;
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
    }
}
