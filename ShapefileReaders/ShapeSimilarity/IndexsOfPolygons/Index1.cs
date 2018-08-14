using GeoAPI.Geometries;
using NetTopologySuite.Features;
using RDotNet;
using ShapefileReaders.ShapeSimilarity.Algorithm.Utils.Line;
using ShapefileReaders.ShapeSimilarity.Algorithm.Utils.PolygonV;
using ShapefileReaders.ShapeSimilarity.Algorithm.Utils.Public;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapefileReaders.ShapeSimilarity.IndexsOfPolygons
{
    class Index1
    {
        public Index1(IFeature feature)
        {
            setMaxCha(feature);
            setMinCha(feature);
            //setMidCha(feature);
            setMinMax();
            setPolygen(feature);
            setAreaRatio(feature);
            setMinMaxArea();
            setTitio(feature);
            setPointNum(feature);
         
            setRows();
            setRows1();

            MaxLine = MaxPoints.getMaxVector(feature);
            MinLine = MinPoints.getMinVector(feature);
        }

        public ILineString MaxLine { get; set; }
        public ILineString MinLine { get; set; }
        public IList<double> MaxCha = new List<double>();
        public IList<double> MinCha = new List<double>();
        public IList<double> MidCha = new List<double>();
        public IList<double> MinMax = new List<double>();
        public IList<Index> MinMax1 = new List<Index>();
        public IList<double> Polygen = new List<double>();
        public IList<double> outAr = new List<double>();
        public IList<double> MinMaxArea = new List<double>();
        public IList<Index> MinMaxArea1 = new List<Index>();
        public IList<double> Titio = new List<double>();
        public IList<Index> Titio1 = new List<Index>();
        public IList<double> pointNum = new List<double>();
        public IList<Index> pointNum1 = new List<Index>();
        

        public IList<IList<double>> Rows = new List<IList<double>>();
        public IList<IList<Index>> Rows1 = new List<IList<Index>>();



        private void setMaxCha(IFeature feature)
        {
            ILineString maxLine = MaxPoints.getMaxVector(feature);
            AngleProperty property = new AngleProperty(feature);
            foreach (var line in property.Lines)
            {
                //double index = (maxLine.Coordinates[1].Distance(maxLine.Coordinates[0]) - line.Coordinates[1].Distance(line.Coordinates[0]));
                double index = (maxLine.Length - line.Length);
                MaxCha.Add(index);//外圆与外边界差
            }
        }
        private void setMinCha(IFeature feature)
        {
            ILineString minLine = MinPoints.getMinVector(feature);
            AngleProperty property = new AngleProperty(feature);
            foreach (var line in property.Lines)
            {
                //int le = line.Intersection(feature.Geometry.Boundary).NumPoints;
                //if (le>1)
                //{
                //    for (int i = 0; i < le-1; i++)
                //    {
                //        double index = (line.Coordinates[i].Distance(line.Coordinates[i+1]) );
                //        /* minLine.Coordinates[1]*/
                //        //-line.Intersection(feature.Geometry).Coordinates[0].Distance(line.Coordinates[1])
                //        MinCha.Add(index);//最小差
                //    }
                    
                //}
                //else 
                //{
                    double index = (line.Length );
                /* line.Coordinates[1].Distance(line.Coordinates[0]) - line.Intersection(feature.Geometry).Coordinates[0].Distance(line.Coordinates[0])*/
                /* minLine.Coordinates[1]*/
                MinCha.Add(index);//最小差
                //}
              
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
                double index = 0;
                Index index1 = new Index(-1, "最大最小比");
                //if (MinCha[i] != 0)
                //{
                    double a = MaxCha[i] / MinCha[i];
                    index = a;
                    index1.Val = a;
                //}

                MinMax.Add(index);
                MinMax1.Add(index1);
            }
        }
 
        private void setPolygen(IFeature feature)
        {
            AngleProperty property = new AngleProperty(feature);
            foreach (var polygen in property.Polygens)
            {
                double index = (polygen.Area);//"线与多边形相交面积"
                Polygen.Add(index);
            }
        }
        private void setAreaRatio(IFeature feature)
        {
            AngleProperty property = new AngleProperty(feature);
            foreach (var oA in property.outArea)
            {
                double index = oA;//"外交面积"
                outAr.Add(index);
            }
        }
        private void setMinMaxArea()
        {
            for (int i = 0; i < Polygen.Count; i++)
            {
                double index = 0;
                Index index1 = new Index(-1,"面积比");
                //if (Polygen[i] != 0)     //若存在outAr[i]==0的情况，会报错
                //{
                    double a = Polygen[i] / outAr[i];
                    index = a;
                    index1.Val = a;
                //}

                MinMaxArea.Add(index);
                MinMaxArea1.Add(index1);
            }
        }
        private void setTitio(IFeature feature)
        {
            AngleProperty property = new AngleProperty(feature);
            foreach (var titio in property.Titio)
            {
                double index = titio;//"三角形比率"
                Index index1 = new Index(titio,"三角形比");
                Titio.Add(index);
                Titio1.Add(index1);
            }
        }
        private void setPointNum(IFeature feature)
        {
            IList<IGeometry> property = new List<IGeometry>();
            property = intPolygon.getInterBoundaryPolygons(feature);
            double a = 0;
            
            for (int i = 0; i < property.Count; i++)
            {
                a = property[i].Length / feature.Geometry.Length;

                double index = a;//"每段多边形边长与周长比"
                Index index1 = new Index(a,"周长");
                pointNum.Add(index);
                pointNum1.Add(index1);
            }
        }

      
        private void setRows()
        {
            //Rows.Add(MinCha);
            //Rows.Add(MaxCha);
            //Rows.Add(test);
            Rows.Add(MinMax);
            //Rows.Add(Polygen);
            //Rows.Add(outAr);
            Rows.Add(MinMaxArea);
            Rows.Add(Titio);
            Rows.Add(pointNum);


        }

        private void setRows1()
        {
         
            Rows1.Add(MinMax1);
            //Rows1.Add(MinMaxArea1);
            //Rows1.Add(Titio1);
            //Rows1.Add(pointNum1);

        }

        public static string rowToString(IList<double> row)
        {
            string str = "";
            foreach (var index in row)
            {
                str = str + index + ",";
            }
            str.TrimEnd(',');
            return str;
        }
        public static string rowToString1(IList<Index> row)
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


        public IList<double> writeToCSV1(int num, int symbol)//num是第几个小班，symbol是取第几个参数
        {
            //StreamWriter write = new StreamWriter(path, true);
            IList<double> row = new List<double>();//row是行
            if (symbol == 0)
            {
                row = Rows[0];
            }
            else if (symbol == 1)
            {
                row = Rows[1];
            }
            else if (symbol == 2)
            {
                row = Rows[2];
            }
            else if (symbol == 3)
            {
                row = Rows[3];
            }
            else if (symbol == 4)
            {
                row = Rows[4];
            }
            else
            {
                Console.WriteLine("参数有误");

            }
            return row;

        }
        public void writeToCSV2(string path)
        {
            StreamWriter write = new StreamWriter(path, true);
            for (int i = 0; i < Polygen.Count; i++)
            {
                IList<Index> row = new List<Index>();            //row是行
                for (int j = 0; j < Rows1.Count; j++)            //把每一行的属性放到一起
                {
                    IList<Index> column = Rows1[j];
                    row.Add(column[i]);
                }

                if (i == 0)                                      //如果i=0，即第一行，输出列名
                {
                    write.WriteLine(heads(row));
                }
                write.WriteLine(rowToString1(row));              //把每一行的值加到一起，输出
            }
            write.Close();
        }
    }

}



