using GeoAPI.Geometries;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using ShapefileReaders.ShapeSimilarity.Algorithm.Utils.Line;
using ShapefileReaders.ShapeSimilarity.Algorithm.Utils.PolygonV;
using ShapefileReaders.ShapeSimilarity.Algorithm.Utils.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ShapefileReaders.ShapeSimilarity.Work.Output
{
    class LineStringOutput
    {
        public static void linesOuput(IList<IFeature> features)
        {
            IList<ILineString> lines = new List<ILineString>();
            for (int i = 0; i < features.Count; i++)
            {
                foreach (var line in new AngleProperty(features[i]).Lines)
                {
                    lines.Add(line);
                }
            }
            string path1 = @"H:\test\结果\lines实验.shp";
            var shapefileWriter = new ShapefileWriter(path1, ShapeGeometryType.LineString);
            foreach (var line in lines)
            {
                shapefileWriter.Write(line);
            }

            string path2 = @"H:\test\结果\lines实验.dbf";
            ShapefileWriter.WriteDummyDbf(path2, lines.Count);
            shapefileWriter.Close();
            Console.WriteLine("生成成功");
            Console.ReadKey();
        }

        /// <summary>
        /// 线与内圆相交的多边形
        /// </summary>
        /// <param name="features"></param>
        public static void intPolygonOuput(IList<IFeature> features)
        {
            IList<IGeometry> inPolygons = new List<IGeometry>();
            IList<IGeometry> inPolygon = new List<IGeometry>();
            for (int i = 0; i < features.Count; i++)
            {
                //  intPolygon intP = new Algorithm.Utils.Public.intPolygon();
                //intP.getOutPolygons(features[i]);

                inPolygons= intPolygon.getintPolygons(features[i]);
                foreach (var polygon in inPolygons)
                {
                    inPolygon.Add(polygon);
                }
            }
            string path1 = @"H:\test\结果\intpolygon1.shp";
            var shapefileWriter = new ShapefileWriter(path1, ShapeGeometryType.Polygon);
            foreach (var p in inPolygon)
            {
                shapefileWriter.Write(p);
            }
            string path2 = @"H:\test\结果\intpolygon1.dbf";
            ShapefileWriter.WriteDummyDbf(path2, inPolygon.Count);
            shapefileWriter.Close();
            Console.WriteLine("生成成功");
            Console.ReadKey();
        }

       /// <summary>
       /// 线与外圆相交的多边形
       /// </summary>
       /// <param name="features"></param>
        public static void outPolygonOuput(IList<IFeature> features)
        {
            IList<IGeometry> outPolygons = new List<IGeometry>();
            IList<IGeometry> outPolygon = new List<IGeometry>();
            for (int i = 0; i < features.Count; i++)
            {
                //  intPolygon intP = new Algorithm.Utils.Public.intPolygon();
                //intP.getOutPolygons(features[i]);

                outPolygons = intPolygon.getOutPolygons(features[i]);
                foreach (var polygon in outPolygons)
                {
                    outPolygon.Add(polygon);
                }
            }
            string path1 = @"H:\test\结果\outpolygon实验测试1.shp";
            var shapefileWriter = new ShapefileWriter(path1, ShapeGeometryType.Polygon);
            foreach (var p in outPolygon)
            {
                shapefileWriter.Write(p);
            }
            string path2 = @"H:\test\结果\outpolygon实验测试1.dbf";
            ShapefileWriter.WriteDummyDbf(path2, outPolygon.Count);                          
            shapefileWriter.Close();
            Console.WriteLine("生成成功");
            Console.ReadKey();
        }


        /// <summary>
        /// 线与多边形相交的多边形
        /// </summary>
        /// <param name="features"></param>
        public static void minPolygonOuput(IList<IFeature> features)
        {
            IList<IGeometry> minPolygons = new List<IGeometry>();
            IList<IGeometry> minPolygon = new List<IGeometry>();
            for (int i = 0; i < features.Count; i++)
            {
                //  intPolygon intP = new Algorithm.Utils.Public.intPolygon();
                //intP.getOutPolygons(features[i]);

                minPolygons = intPolygon.getMinPolygons(features[i]);
                foreach (var polygon in minPolygons)
                {
                    minPolygon.Add(polygon);
                }
            }
            string path1 = @"H:\test\结果\minpolygon4.shp";
            var shapefileWriter = new ShapefileWriter(path1, ShapeGeometryType.Polygon);
            foreach (var p in minPolygon)
            {
                shapefileWriter.Write(p);
            }
            string path2 = @"H:\test\结果\minpolygon4.dbf";
            ShapefileWriter.WriteDummyDbf(path2, minPolygon.Count);
            shapefileWriter.Close();
            Console.WriteLine("生成成功");
            Console.ReadKey();
        }

    


        public static void maxLineOutput(IList<IFeature> features)
        {
            IList<ILineString> maxLines = new List<ILineString>();

            for (int i = 0; i < features.Count; i++)
            {
                maxLines.Add(MaxPoints.getMaxVector(features[i]));
            }


            string path1 = @"H:\test\结果\MaxLine实验测试1.shp";
            var shapefileWriter = new ShapefileWriter(path1, ShapeGeometryType.LineString);
            foreach (var maxLine in maxLines)
            {
                shapefileWriter.Write(maxLine);
            }
            string path2 = @"H:\test\结果\MaxLine实验测试1.dbf";
            ShapefileWriter.WriteDummyDbf(path2, features.Count);
            shapefileWriter.Close();
            Console.WriteLine("生成成功");
            Console.ReadKey();
        }
        public static void minLineOutput(IList<IFeature> features)
        {
            IList<ILineString> minLines = new List<ILineString>();

            for (int i = 0; i < features.Count; i++)
            {
                minLines.Add(MinPoints.getMinVector(features[i]));
            }

            string path1 = @"H:\test\结果\MinLine1.shp";
            var shapefileWriter = new ShapefileWriter(path1, ShapeGeometryType.LineString);
            foreach (var minLine in minLines)
            {
                shapefileWriter.Write(minLine);
            }


            string path2 = @"H:\test\结果\MinLine1.dbf";
            ShapefileWriter.WriteDummyDbf(path2, features.Count);
            shapefileWriter.Close();
            Console.WriteLine("生成成功");
            Console.ReadKey();
        }
        public static void maxCircleOutput(IList<IFeature> features)
        {
            IList<ILineString> circles = new List<ILineString>();
            for (int i = 0; i < features.Count; i++)
            {
                ILineString maxLine = MaxPoints.getMaxVector(features[i]);
                ILineString circle = Circles.getCircle(maxLine.Coordinates[0], maxLine.Coordinates[1]);
                circles.Add(circle);
            }

            string path1 = @"H:\test\结果\MaxCircle实验测试1.shp";
            var shapefileWriter = new ShapefileWriter(path1, ShapeGeometryType.LineString);
            foreach (var circle in circles)
            {
                shapefileWriter.Write(circle);
            }


            string path2 = @"H:\test\结果\MaxCircle实验测试1.dbf";
            ShapefileWriter.WriteDummyDbf(path2, features.Count);
            shapefileWriter.Close();
            Console.WriteLine("生成成功");
            Console.ReadKey();
        }

        public static void minCircleOutput(IList<IFeature> features)
        {
            IList<ILineString> circles = new List<ILineString>();
            for (int i = 0; i < features.Count; i++)
            {
                ILineString minLine = MinPoints.getMinVector(features[i]);
                ILineString circle = Circles.getCircle(minLine.Coordinates[0], minLine.Coordinates[1]);
                circles.Add(circle);
            }

            string path1 = @"H:\test\结果\MinCircle1.shp";
            var shapefileWriter = new ShapefileWriter(path1, ShapeGeometryType.LineString);
            foreach (var circle in circles)
            {
                shapefileWriter.Write(circle);
            }


            string path2 = @"H:\test\结果\MinCircle1.dbf";
            ShapefileWriter.WriteDummyDbf(path2, features.Count);
            shapefileWriter.Close();
            Console.WriteLine("生成成功");
            Console.ReadKey();
        }


        public static void interBoundaryOuput(IList<IFeature> features)
        {
            IList<IGeometry> interBoundarys = new List<IGeometry>();
            IList<IGeometry> interBoundary = new List<IGeometry>();
            for (int i = 0; i < features.Count; i++)
            {
                //  intPolygon intP = new Algorithm.Utils.Public.intPolygon();
                //intP.getOutPolygons(features[i]);

                interBoundarys = intPolygon.getInterBoundaryPolygons(features[i]);
                foreach (var polygon in interBoundarys)
                {
                    interBoundary.Add(polygon);
                }
            }
            string path1 = @"H:\test\结果\interBoundary1.shp";
            var shapefileWriter = new ShapefileWriter(path1, ShapeGeometryType.LineString);
            foreach (var p in interBoundary)
            {
                shapefileWriter.Write(p);
            }
            string path2 = @"H:\test\结果\interBoundary1.dbf";
            ShapefileWriter.WriteDummyDbf(path2, interBoundary.Count);
            shapefileWriter.Close();
            Console.WriteLine("生成成功");
            Console.ReadKey();
        }

    }
}
