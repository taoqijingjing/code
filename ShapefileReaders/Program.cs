using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using GeoAPI.Geometries;
using NetTopologySuite.IO;
using NetTopologySuite.Geometries;
using NetTopologySuite.Features;
using NetTopologySuite.Operation.Distance;
using NetTopologySuite.Algorithm;
using NetTopologySuite.Algorithm.Distance;
//using ShapefileReaders.ShapeSimilarity.IndexsOfPolygon;
using ShapefileReaders.ShapeSimilarity.Algorithm;
using ShapefileReaders.ShapeSimilarity.Work;
using ShapefileReaders.ShapeSimilarity.Work.Input;
using ShapefileReaders.ShapeSimilarity.Algorithm.Utils.Public;
using System.Collections.ObjectModel;
using NetTopologySuite.Operation.Buffer;
using ShapefileReaders.ShapeSimilarity.Algorithm.Utils.PolygonV;
using ShapefileReaders.ShapeSimilarity.Algorithm.Utils.Line;
using System.IO;
using ShapefileReaders.ShapeSimilarity.IndexsOfPolygons;
using ShapefileReaders.ShapeSimilarity.Work.Output;
using RDotNet;

namespace ShapefileReaders
{
    class Program
    {
        static void Main(string[] args)
        {

            string path = @"F:\邹静\各门课资料\论文\毕业论文\论文\第二次试验过程\数据库.shp";
            string path1 = @"F:\邹静\各门课资料\论文\毕业论文\论文\先简化后缩放\简化5米\A5\A5.shp";

            ////string path = @"H:\test\质心在外_终.shp";
            ////string path1 = @"H:\test\简化_0501.shp";
            Collection<IFeature> features = new SuperShpReader(path).ReadAll();
            Collection<IFeature> features1 = new SuperShpReader(path1).ReadAll();
            //////////////最大线
            ////LineStringOutput.maxLineOutput(features1);
            ////////////////所有线
            //////////LineStringOutput.linesOuput(features);
            //////////////线与外圆相交后的多边形
            ////LineStringOutput.outPolygonOuput(features1);
            ////////////// 最大圆
            ////LineStringOutput.maxCircleOutput(features1);

            REngine.SetEnvironmentVariables(); // <-- May be omitted; the next line would call it.
            REngine engine = REngine.GetInstance();
            engine.Initialize();
            for (int j = 0; j < 1; j++)
            {
                Console.WriteLine("第{0}个参数", j);
                int symbol = j;
                IList<int> count = new List<int>();
                IList<int> count1 = new List<int>();
                IList<double> rowRes = new List<double>();
                IList<double> rowRes0 = new List<double>();
                IList<ILineString> maxLine = new List<ILineString>();
                IList<ILineString> maxLine1 = new List<ILineString>();
                IList<double> maxLines = new List<double>();
                //int w = 0;
                for (int q = 0; q < features.Count; q++)
                {
                    //maxLine.Add(MaxPoints.getMaxVector(features[q]));
                    //double circle = Circles.getCircle(maxLine[q].Coordinates[0], maxLine[q].Coordinates[1]).Length;
                    //maxLines.Add(MaxPoints.getMaxVector(features[q]).Length / circle);

                    Index1 row1 = new Index1(features[q]);
                    int ab = 0;
                    foreach (var item in row1.writeToCSV1(q, symbol))
                    {
                        rowRes.Add(item);
                        ab++;
                    }
                    count1.Add(ab);
                    count.Add(ab);
                }
                #region
                //    //for (int w = 0; w < count[q]; w++)
                //    //{
                //    for (int v = q; v < count.Count; v++)
                //    {
                //            count[v] = count[v] +w;
                //        double max = rowRes[w];
                //        double min = rowRes[w];
                //        for (int L = w; L < count[v]; L++)
                //        {
                //            if (max < rowRes[L])
                //            {
                //                max = rowRes[L];
                //            }
                //            if (min > rowRes[L])
                //            {
                //                min = rowRes[L];
                //            }

                //        }
                //        for (int k = w; k < count[v]; k++)
                //        {
                //            if (rowRes[k] != 0)
                //            {
                //                rowRes[k] = (rowRes[k] - min) / (max - min);
                //            }
                //        }
                //        w = count[v];
                //    }


                //}
                //double maxLength = 0;
                #endregion
                for (int i = 0; i < features1.Count; i++)
                {
                    //maxLine1.Add(MaxPoints.getMaxVector(features1[i]));
                    //double circle1 = Circles.getCircle(maxLine1[i].Coordinates[0], maxLine1[i].Coordinates[1]).Length;
                    //maxLength = MaxPoints.getMaxVector(features1[i]).Length / circle1;

                    IList<double> rowRes1 = new List<double>();
                    Index1 row = new Index1(features1[i]);
                    foreach (var item in row.writeToCSV1(i, symbol))
                    {
                        rowRes1.Add(item);
                    }
                    //double max = rowRes1[0];
                    //double min = rowRes1[0];
                    //for (int k = 1; k < rowRes1.Count; k++)
                    //{
                    //    if (max < rowRes1[k])
                    //    {
                    //        max = rowRes1[k];
                    //    }
                    //    if (min > rowRes1[k])
                    //    {
                    //        min = rowRes1[k];
                    //    }
                    //}
                    //for (int k = 0; k < rowRes1.Count; k++)
                    //{

                    //        rowRes1[k] = (rowRes1[k] - min) / (max - min);

                    //}
                    RDotNet.NumericVector V1 = engine.CreateNumericVector(rowRes1);
                    engine.SetSymbol("V1", V1);
                    int a = 0;
                    IList<double> pValue = new List<double>();
                    for (int m = 0; m < features.Count; m++)
                    {
                        //if (maxLines[m] - 1 <= maxLength && maxLength <= maxLines[m] + 1)
                        //{
                        IList<double> rowRes2 = new List<double>();
                        int n = 0;
                        for (int b = m; b < count1[m] + m; b++)
                        {
                            rowRes2.Add(rowRes[b + a]); //要把第一个的去掉
                            n++;
                        }
                        a = a + n - 1;
                        #region
                        //double max1 = rowRes2[0];
                        //double min1 = rowRes2[0];
                        //for (int c = 0; c < rowRes2.Count; c++)
                        //{
                        //    if (max1 < rowRes2[c])
                        //    {
                        //        max1 = rowRes2[c];
                        //    }
                        //    if (min1 > rowRes2[c])
                        //    {
                        //        min1 = rowRes2[c];
                        //    }
                        //}
                        //for (int k1 = 0; k1 < rowRes2.Count; k1++)
                        //{
                        //     rowRes2[k1] = (rowRes2[k1] - min1) / (max1 - min1);

                        //}
                        #endregion
                        RDotNet.NumericVector V2 = engine.CreateNumericVector(rowRes2);
                        engine.SetSymbol("V2", V2);
                        //GenericVector testRes = engine.Evaluate("wilcox.test(V1,V2, paired = FALSE)").AsList();

                        if (V1.Length == V2.Length)
                        {
                            //GenericVector testRes = engine.Evaluate("cor.test(V1,V2)").AsList();
                            //double p = testRes["p.value"].AsNumeric().First();
                            ////using (StreamWriter sw = new StreamWriter(@"C:\Users\Administrator\Desktop\result\shiyan0.txt", true))
                            ////{
                            ////    sw.Write("{0}与{1}的P-Value={2}\r\n", i, m, p);
                            ////}
                            double sum = 0;
                            double v1Power = 0;
                            double v2Power = 0;
                            for (int d = 0; d < V1.Length; d++)
                            {
                                sum += V1[d] * V2[d];
                                v1Power += Math.Pow(V1[d], 2);
                                v2Power += Math.Pow(V2[d], 2);
                            }
                            double p = sum / (Math.Sqrt(v1Power) * Math.Sqrt(v2Power));

                            ////////string path_result= string.Format(@"H:\test\experiment\result\simplify2_result{0}.txt", i);
                            //输出所有P值
                            using (StreamWriter sw = new StreamWriter(@"F:\邹静\各门课资料\论文\毕业论文\论文\先简化后缩放\简化5米\A5\A5.txt", true))
                            {
                                sw.Write("{0}与{1}的P-Value={2}\r\n", i, m, p);
                                //pValue.Add(p);
                            }
                            #region  //输出V1，V2
                            //using (StreamWriter sw = new StreamWriter(@"H:\test\experiment\v1ceshi.txt", true))
                            //{
                            //    foreach (var item in V1)
                            //    {
                            //        sw.Write(item + "\r\n");

                            //    }
                            //    sw.Write("第{0}个参数换\r\n", j);
                            //}
                            //using (StreamWriter sw = new StreamWriter(@"H:\test\experiment\v2ceshi.txt", true))
                            //{
                            //    foreach (var item in V2)
                            //    {
                            //        sw.Write(item + "\r\n");
                            //    }
                            //    sw.Write("第{0}个参数换\r\n", j);
                            //}
                            #endregion
                        }
                        else
                        {
                            continue;
                        }
                        //}
                        //else
                        //{
                        //    a += count[m] - 1;
                        //}

                    }
                    #region
                    //double max = pValue[0];
                    //for (int aa = 0; aa < pValue.Count; aa++)
                    //{

                    //    if (pValue[aa] > max)
                    //    {
                    //        max = pValue[aa];
                    //    }
                    //}
                    //using (StreamWriter sw = new StreamWriter(@"F:\邹静\各门课资料\论文\毕业论文\论文\先简化后缩放\简化3米\A1\A1_最大值.txt", true))
                    //{
                    //    sw.Write("{0}与{1}的最大值={2}\r\n", i,m, max);

                    //}
                    #endregion
                }
            }
            Console.WriteLine("成功");

            engine.Dispose();
            #region    //实验二

            //REngine.SetEnvironmentVariables(); // <-- May be omitted; the next line would call it.
            //REngine engine = REngine.GetInstance();
            ////////engine.Initialize();
            //for (int j = 1; j < 4; j++)
            //{
            //    Console.WriteLine("第{0}个参数", j);
            //    int symbol = j;
            //    IList<int> count = new List<int>();
            //    IList<double> rowRes = new List<double>();
            //    IList<ILineString> maxLine = new List<ILineString>();
            //    IList<double> maxLines = new List<double>();
            //    for (int q = 0; q < features.Count; q++)
            //    {
            //        maxLine.Add(MaxPoints.getMaxVector(features[q]));
            //        double circle = Circles.getCircle(maxLine[q].Coordinates[0], maxLine[q].Coordinates[1]).Length;
            //        maxLines.Add(MaxPoints.getMaxVector(features[q]).Length / circle);

            //        Index1 row1 = new Index1(features[q]);
            //        int a = 0;
            //        foreach (var item in row1.writeToCSV1(q, symbol))
            //        {
            //            rowRes.Add(item);
            //            a++;
            //        } 
            //        count.Add(a);

            //        double max = 0;
            //        double min = 0;
            //        for (int l = 1; l < rowRes.Count; l++)
            //        {
            //            if (max < rowRes[l])
            //            {
            //                max = rowRes[l];
            //            }
            //            if (min > rowRes[l])
            //            {
            //                min = rowRes[l];
            //            }
            //        }
            //        for (int k = 0; k < rowRes.Count; k++)
            //        {
            //            if (rowRes[k] != 0)
            //            {
            //                rowRes[k] = (rowRes[k] - min) / (max - min);
            //            }
            //        }
            //    }
            //    double maxLength = 0;
            //    for (int i = 0; i < features1.Count; i++)
            //    {
            //        maxLine.Add(MaxPoints.getMaxVector(features1[i]));
            //        double circle = Circles.getCircle(maxLine[i].Coordinates[0], maxLine[i].Coordinates[1]).Length;
            //        maxLength = MaxPoints.getMaxVector(features1[i]).Length / circle;

            //        IList<double> rowRes1 = new List<double>();
            //        Index1 row = new Index1(features1[i]);
            //        foreach (var item in row.writeToCSV1(i, symbol))
            //        {
            //            rowRes1.Add(item);
            //        }
            //        double max = rowRes1[0];
            //        double min = rowRes1[0];
            //        for (int k = 1; k < rowRes1.Count; k++)
            //        {
            //            if (max < rowRes1[k])
            //            {
            //                max = rowRes1[k];
            //            }
            //            if (min > rowRes1[k])
            //            {
            //                min = rowRes1[k];
            //            }
            //        }
            //        for (int k = 0; k < rowRes1.Count; k++)
            //        {
            //            if (rowRes1[k] != 0)
            //            {
            //                rowRes1[k] = (rowRes1[k] - min) / (max - min);
            //            }
            //        }
            //        RDotNet.NumericVector V1 = engine.CreateNumericVector(rowRes1);
            //        engine.SetSymbol("V1", V1);
            //        int a = 0;

            //        for (int m = 0; m < features.Count; m++)
            //        {
            //            if (maxLines[m] - 1 <= maxLength && maxLength <= maxLines[m] + 1)
            //            {
            //                IList<double> rowRes2 = new List<double>();
            //                int n = 0;
            //                for (int b = m; b < count[m] + m; b++)
            //                {
            //                    rowRes2.Add(rowRes[b + a]); //要把第一个的去掉
            //                    n++;
            //                }
            //                a = a + n - 1;

            //                RDotNet.NumericVector V2 = engine.CreateNumericVector(rowRes2);
            //                engine.SetSymbol("V2", V2);
            //                //GenericVector testRes = engine.Evaluate("wilcox.test(V1,V2, paired = FALSE)").AsList();
            //                using (StreamWriter sw = new StreamWriter(@"H:\test\V1.txt", true))
            //                {
            //                    foreach (var item in V1)
            //                    {
            //                        sw.Write(item);

            //                    }
            //                    sw.Write("第{0}个参数换\r\n", j);
            //                }
            //                using (StreamWriter sw = new StreamWriter(@"H:\test\V2.txt", true))
            //                {
            //                    foreach (var item in V2)
            //                    {
            //                        sw.Write(item);
            //                    }
            //                    sw.Write("第{0}个参数换\r\n", j);
            //                }
            //                if (V1.Length == V2.Length)
            //                {
            //                    GenericVector testRes = engine.Evaluate("t.test(V1,V2, paired = TRUE)").AsList();
            //                    double p = testRes["p.value"].AsNumeric().First();
            //                    //using (StreamWriter sw = new StreamWriter(@"C:\Users\Administrator\Desktop\result\shiyan0.txt", true))
            //                    //{
            //                    //    sw.Write("{0}与{1}的P-Value={2}\r\n", i, m, p);
            //                    //}
            //                    using (StreamWriter sw = new StreamWriter(@"H:\test\shiyan0.txt", true))
            //                    {
            //                        sw.Write("{0}与{1}的P-Value={2}\r\n", i, m, p);
            //                    }
            //                }
            //                else
            //                {
            //                    continue;
            //                }
            //            }
            //            else
            //            {
            //                a += count[m] - 1;
            //            }

            //        }
            //    }
            //}
            //Console.WriteLine("成功");
            #endregion

            ////最小线
            //LineStringOutput.minLineOutput(features);
            //////最大线
            //LineStringOutput.maxLineOutput(features);
            //LineStringOutput.maxLineOutput(features1);
            ////所有线
            //LineStringOutput.linesOuput(features);
            //////最小圆
            ////LineStringOutput.minCircleOutput(features);
            ////最大圆
            //LineStringOutput.maxCircleOutput(features);
            ////线与内圆相交后的多边形
            //LineStringOutput.intPolygonOuput(features);
            ////线与外圆相交后的多边形
            //LineStringOutput.outPolygonOuput(features1);
            ////线与小班相交后的多边形
            //LineStringOutput.minPolygonOuput(features);
            ////输出每隔五度时的每段多边形
            //LineStringOutput.interBoundaryOuput(features);



            //// 输出.csv
            //string path3 = @"H:\test\结果\ads3.csv";
            //int a = 0;
            //for (int i = 0; i < features1.Count; i++)
            //{
            //    a++;
            //    path3 = @"H:\test\";
            //    path = path3 + "\\" + a + ".csv";
            //    Index1 indexs = new Index1(features1[i]);
            //    indexs.writeToCSV2(path);
            //}


            #region
            //REngine.SetEnvironmentVariables(); // <-- May be omitted; the next line would call it.
            //REngine engine = REngine.GetInstance();
            ////////engine.Initialize();
            //for (int j = 3; j < 4; j++)
            //{
            //    Console.WriteLine("第{0}个参数", j);
            //    int symbol = j;
            //    for (int i = 0; i < features.Count; i++)
            //    {
            //        Index1 row = new Index1(features[i]);

            //        IList<double> rowRes = new List<double>();

            //        foreach (var item in row.writeToCSV1(i, symbol))
            //        {
            //            rowRes.Add(item);
            //        }
            //        for (int k = i; k < features.Count - 1; k++)
            //        {
            //            Index1 row1 = new Index1(features[k + 1]);
            //            IList<double> rowRes1 = new List<double>();
            //            foreach (var item in row1.writeToCSV1(k + 1, symbol))
            //            {
            //                rowRes1.Add(item);
            //            }
            //            RDotNet.NumericVector V1 = engine.CreateNumericVector(rowRes);
            //            engine.SetSymbol("V1", V1);
            //            RDotNet.NumericVector V2 = engine.CreateNumericVector(rowRes1);
            //            engine.SetSymbol("V2", V2);
            //            GenericVector testRes = engine.Evaluate("wilcox.test(V1,V2, paired = FALSE)").AsList();
            //            double p = testRes["p.value"].AsNumeric().First();
            //            //Console.WriteLine("Group1: [{0}]", string.Join(", ", V1));
            //            //Console.WriteLine("Group2: [{0}]", string.Join(", ", V2));
            //            //engine.Evaluate("source('H:/test/结果/表格/R.r')");

            //            //Console.WriteLine("{0}与{1}的P-value = {2}", i, k + 1, p);

            //            using (StreamWriter sw = new StreamWriter(@"C:\zj\result\result3.txt", true))
            //            {
            //                sw.Write("{0}与{1}的P-Value={2}\r\n", i, k + 1, p);
            //            }
            //        }
            //    }
            //}
            //Console.ReadKey();
            ////////engine.Dispose();

            #endregion

            #region   //输出最长最短比和夹角
            //IList<ILineString> maxLines = new List<ILineString>();
            //IList<ILineString> minLines = new List<ILineString>();
            //IList<double> maxMinRatio = new List<double>();
            //IList<double> maxMinAngle = new List<double>();
            //IList<double> maxMinAngle1 = new List<double>();
            //IList<double> angle = new List<double>();
            //IList<double> angle1 = new List<double>();
            //IList<double> angleResult = new List<double>();
            //for (int i = 0; i < features.Count; i++)
            //{
            //    maxLines.Add(MaxPoints.getMaxVector(features[i]));
            //    minLines.Add(MinPoints.getMinVector(features[i]));
            //    maxMinRatio.Add(maxLines[i].Length / minLines[i].Length);
            //    double a = features[i].Geometry.Centroid.X;
            //    double b = features[i].Geometry.Centroid.Y;
            //    maxLines[i].Coordinates[1].X = maxLines[i].Coordinates[1].X - a;
            //    maxLines[i].Coordinates[1].Y = maxLines[i].Coordinates[1].Y - b;
            //    minLines[i].Coordinates[1].X = minLines[i].Coordinates[1].X - a;
            //    minLines[i].Coordinates[1].Y = minLines[i].Coordinates[1].Y - b;

            //    if (maxLines[i].Coordinates[1].X > 0 && maxLines[i].Coordinates[1].Y >= 0)
            //    {
            //        angle.Add(Math.Atan2(maxLines[i].Coordinates[1].Y, maxLines[i].Coordinates[1].X) * 180 / Math.PI);
            //    }
            //    else if (maxLines[i].Coordinates[1].X < 0 && maxLines[i].Coordinates[1].Y > 0)
            //    {
            //        angle.Add(180 - (Math.Atan2(maxLines[i].Coordinates[1].Y, -maxLines[i].Coordinates[1].X) * 180 / Math.PI));
            //    }
            //    else if (maxLines[i].Coordinates[1].X < 0 && maxLines[i].Coordinates[1].Y <= 0)
            //    {
            //        angle.Add(180 + (Math.Atan2(-maxLines[i].Coordinates[1].Y, -maxLines[i].Coordinates[1].X) * 180 / Math.PI));
            //    }

            //    else if (maxLines[i].Coordinates[1].X > 0 && maxLines[i].Coordinates[1].Y <= 0)
            //    {
            //        angle.Add(360 - (Math.Atan2(-maxLines[i].Coordinates[1].Y, maxLines[i].Coordinates[1].X) * 180 / Math.PI));

            //    }


            //    if (minLines[i].Coordinates[1].X > 0 && minLines[i].Coordinates[1].Y >= 0)
            //    {
            //        angle1.Add(Math.Atan2(minLines[i].Coordinates[1].Y, minLines[i].Coordinates[1].X) * 180 / Math.PI);
            //    }
            //    else if (minLines[i].Coordinates[1].X < 0 && minLines[i].Coordinates[1].Y > 0)
            //    {
            //        angle1.Add(180 - (Math.Atan2(minLines[i].Coordinates[1].Y, -minLines[i].Coordinates[1].X) * 180 / Math.PI));
            //    }
            //    else if (minLines[i].Coordinates[1].X < 0 && minLines[i].Coordinates[1].Y <= 0)
            //    {
            //        angle1.Add(180 + (Math.Atan2(-minLines[i].Coordinates[1].Y, -minLines[i].Coordinates[1].X) * 180 / Math.PI));
            //    }

            //    else if (minLines[i].Coordinates[1].X > 0 && minLines[i].Coordinates[1].Y <= 0)
            //    {
            //        angle1.Add(360 - (Math.Atan2(-minLines[i].Coordinates[1].Y, minLines[i].Coordinates[1].X) * 180 / Math.PI));

            //    }
            //    if (Math.Abs(angle[i] - angle1[i]) <= 180)
            //    {
            //        angleResult.Add(Math.Abs(angle[i] - angle1[i]));
            //    }
            //    else
            //    {
            //        angleResult.Add(360 - Math.Abs(angle[i] - angle1[i]));
            //    }

            //}
            //foreach (var item in maxMinRatio)
            //{
            //    Console.WriteLine(item + "最长最短比");
            //}
            //foreach (var item in angleResult)
            //{
            //    Console.WriteLine(item + "最长最短夹角");
            //}
            #endregion




            Console.WriteLine("输出成功");
            Console.Read();
        }

    }


}

