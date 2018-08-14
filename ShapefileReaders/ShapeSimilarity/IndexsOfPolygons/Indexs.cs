using GeoAPI.Geometries;
using NetTopologySuite.Features;
using ShapefileReaders.ShapeSimilarity.Algorithm.Utils.Line;
using ShapefileReaders.ShapeSimilarity.Algorithm.Utils.Public;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapefileReaders.ShapeSimilarity.IndexsOfPolygons
{
    class Indexs
    {
        public Indexs(IFeature feature)
        {
            setMaxCha(feature);
            setMinCha(feature);
            //setMidCha(feature);
            setMinMax();
            setPolygen(feature);
            setAreaRatio(feature);
            setTitio(feature);
            setPointNum(feature);
            setRows();

            MaxLine = MaxPoints.getMaxVector(feature);
            MinLine = MinPoints.getMinVector(feature);
        }

        public ILineString MaxLine { get; set; }
        public ILineString MinLine { get; set; }
        public IList<Index> MaxCha = new List<Index>();
        public IList<Index> MinCha = new List<Index>();
        public IList<Index> MidCha = new List<Index>();
        public IList<Index> MinMax = new List<Index>();
        public IList<Index> Polygen = new List<Index>();
        public IList<Index> outAr = new List<Index>();
        public IList<Index> Titio = new List<Index>();
        public IList<Index> pointNum = new List<Index>();
    

        public IList<IList<Index>> Rows = new List<IList<Index>>();


        private void setMaxCha(IFeature feature)
        {
            ILineString maxLine = MaxPoints.getMaxVector(feature);
            AngleProperty property = new AngleProperty(feature);
            foreach (var line in property.Lines)
            {
                Index index = new Index(maxLine.Coordinates[1].Distance(maxLine.Coordinates[0]) - line.Coordinates[1].Distance(line.Coordinates[0]), "外圆与外边界差");
                MaxCha.Add(index);
            }
        }
        private void setMinCha(IFeature feature)
        {
            ILineString minLine = MinPoints.getMinVector(feature);
            AngleProperty property = new AngleProperty(feature);
            foreach (var line in property.Lines)
            {
                Index index = new Index(line.Coordinates[1].Distance(line.Coordinates[0]) - minLine.Coordinates[1].Distance(minLine.Coordinates[0]), "最小差");
                MinCha.Add(index);
            }
            #region
            //ILineString minLine = MinPoints.getMinVector(feature);
            //AngleProperty property = new AngleProperty(feature);
            //foreach (var line in property.Lines)
            //{
            //    if (line.Intersection(feature.Geometry.Boundary).NumPoints == 2)
            //    {
            //        line.Intersection(feature.Geometry.Boundary).Coordinates[0].X -= feature.Geometry.Centroid.Coordinate.X;
            //        line.Intersection(feature.Geometry.Boundary).Coordinates[0].Y -= feature.Geometry.Centroid.Coordinate.Y;
            //        line.Intersection(feature.Geometry.Boundary).Coordinates[1].X -= feature.Geometry.Centroid.Coordinate.X;
            //        line.Intersection(feature.Geometry.Boundary).Coordinates[1].Y -= feature.Geometry.Centroid.Coordinate.Y;
            //        if (AngleProperty.getAngle(line.Intersection(feature.Geometry.Boundary).Coordinates[0]) > 90 && AngleProperty.getAngle(line.Intersection(feature.Geometry.Boundary).Coordinates[0]) < 270)
            //        {
            //            Index index1 = new Index(line.Coordinates[0].Distance(line.Intersection(feature.Geometry.Boundary).Coordinates[1]) - minLine.Coordinates[1].Distance(minLine.Coordinates[0]), "最小差");
            //            MinCha.Add(index1);
            //        }
            //       else if (AngleProperty.getAngle(line.Intersection(feature.Geometry.Boundary).Coordinates[0]) > 0 && AngleProperty.getAngle(line.Intersection(feature.Geometry.Boundary).Coordinates[0]) < 90||AngleProperty.getAngle(line.Intersection(feature.Geometry.Boundary).Coordinates[0])>270&& AngleProperty.getAngle(line.Intersection(feature.Geometry.Boundary).Coordinates[0])<360)
            //        {
            //            Index index1 = new Index(line.Coordinates[0].Distance(line.Intersection(feature.Geometry.Boundary).Coordinates[0]) - minLine.Coordinates[1].Distance(minLine.Coordinates[0]), "最小差");
            //            MinCha.Add(index1);
            //        }

            //    }
            //    else if (line.Intersection(feature.Geometry.Boundary).NumPoints == 1 || line.Intersection(feature.Geometry.Boundary).NumPoints == 0)
            //    {
            //        Index index = new Index(line.Coordinates[1].Distance(line.Coordinates[0]) - minLine.Coordinates[1].Distance(minLine.Coordinates[0]), "最小差");
            //        MinCha.Add(index);
            //    }
            //    //else
            //    //{
            //    //    Console.WriteLine("交点超过三个，请检查小班");
            //    //}
            //}
            #endregion

        }
        #region
        //private void setMidCha(IFeature feature)
        //{

        //    AngleProperty property = new AngleProperty(feature);
        //    foreach (var line in property.Lines)
        //    {
        //        if (line.Intersection(feature.Geometry.Boundary).NumPoints == 2)
        //        {
        //            Index index1 = new Index(line.Coordinates[1].Distance(line.Coordinates[0]) - line.Coordinates[0].Distance(line.Intersection(feature.Geometry.Boundary).Coordinates[0]), "中间差");
        //            MidCha.Add(index1);
        //        }
        //        else if (line.Intersection(feature.Geometry.Boundary).NumPoints == 1 || line.Intersection(feature.Geometry.Boundary).NumPoints == 0)
        //        {
        //            Index index = new Index(1, "中间差");
        //            MidCha.Add(index);
        //        }
        //        else if (line.Intersection(feature.Geometry.Boundary).NumPoints > 2)
        //        {
        //            double a = 1;
        //            for (int i = 0; i < line.Intersection(feature.Geometry.Boundary).Coordinates.Length - 1; i++)
        //            {
        //                Index index = new Index(line.Intersection(feature.Geometry.Boundary).Coordinates[i].Distance(line.Intersection(feature.Geometry.Boundary).Coordinates[i + 1]), "中间差");
        //                double b = index.Val;
        //                a = a * b;
        //            }
        //            Index index1 = new Index(a);
        //            MidCha.Add(index1);
        //        }
        //    }
        //}
        #endregion

        private void setMinMax()
        {
            for (int i = 0; i < MaxCha.Count; i++)
            {
                Index index = new Index(-1, "最大最小比");
                if (MaxCha[i].Val != 0)
                {
                    double a = MaxCha[i].Val / MinCha[i].Val;
                    index.Val = a;
                }

                MinMax.Add(index);
            }
        }

        private void setPolygen(IFeature feature)
        {
            AngleProperty property = new AngleProperty(feature);
            foreach (var polygen in property.Polygens)
            {
                Index index = new Index(polygen.Area, "线与多边形相交面积");
                Polygen.Add(index);
            }
        }
        private void setAreaRatio(IFeature feature)
        {
            AngleProperty property = new AngleProperty(feature);
            foreach (var oA in property.outArea)
            {
                Index index = new Index(oA, "外交面积");
                outAr.Add(index);
            }
        }
        private void setTitio(IFeature feature)
        {
            AngleProperty property = new AngleProperty(feature);
            foreach (var titio in property.Titio)
            {
                Index index = new Index(titio, "三角形比率");
                Titio.Add(index);
            }
        }
        private void setPointNum(IFeature feature)
        {
            IList<IGeometry> property = new List<IGeometry>();
            property = intPolygon.getInterBoundaryPolygons(feature);
            double a = 0;
            for (int i = 0; i < property.Count; i++)
            {
                a = property[i].Coordinates.Length;

                Index index = new Index(a, "每段多边形点的个数");
                pointNum.Add(index);
            }
        }

     
        private void setRows()
        {
            Rows.Add(MinCha);
            Rows.Add(MaxCha);
            Rows.Add(MinMax);
            Rows.Add(Polygen);
            Rows.Add(outAr);
            Rows.Add(Titio);
            Rows.Add(pointNum);
         
        }


        public static string rowToString(IList<Index> row)
        {
            string str = "";
            foreach (var index in row)
            {
                str = str + index.Val + ",";
            }
            str.TrimEnd(',');
            return str;
        }
        public static string heads(IList<Index> row)
        {
            string str = "";
            foreach (var index in row)
            {
                str = str + index.Head + ",";
            }
            str.TrimEnd(',');
            return str;
        }

        public void writeToCSV(string path)
        {
            StreamWriter write = new StreamWriter(path, true);
            for (int i = 0; i < Polygen.Count; i++)
            {
                IList<Index> row = new List<Index>();            //row是行
                for (int j = 0; j < Rows.Count; j++)            //把每一行的属性放到一起
                { 
                    IList<Index> column = Rows[j];
                    row.Add(column[i]);
                   
                }

                if (i == 0)                                      //如果i=0，即第一行，输出列名
                {
                    write.WriteLine(heads(row));
                }
                write.WriteLine(rowToString(row));              //把每一行的值加到一起，输出
            }
            write.Close();
        }
        public void writeToCSV1(string path)
        {
            StreamWriter write = new StreamWriter(path, true);
            for (int i = 0; i < 1; i++)
            {
                IList<Index> row = new List<Index>();            //row是行
                //for (int j = 0; j < Rows.Count; j++)            //把每一行的属性放到一起
                //{
                    row = Rows[0];
            //row.Add(column[i]);
            //}

            //if (i == 0)                                      //如果i=0，即第一行，输出列名
            //{
            //    write.WriteLine(heads(row));
            //}
            write.WriteLine(rowToString(row));              //把每一行的值加到一起，输出
            }
            write.Close();
        }
    }

}
