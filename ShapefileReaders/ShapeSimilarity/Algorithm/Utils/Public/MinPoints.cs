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
    class MinPoints
    {
        public static ILineString getMinVector(IFeature feature)
        {
            double min = 100000;
            Coordinate point = new Coordinate();
            IList<Coordinate> points = new List<Coordinate>();
            IList<Coordinate> featurePoints = new List<Coordinate>();
            foreach (var dian in feature.Geometry.Coordinates)
            {
                featurePoints.Add(dian);
            }

            for (int i = 1; i < feature.Geometry.NumPoints; i++)
            {
                double dis = feature.Geometry.Centroid.Coordinate.Distance(feature.Geometry.Coordinates[i]);
                if (min > dis)
                {
                    min = dis;
                    point = feature.Geometry.Coordinates[i];
                }
            }
            points.Add(point);
            featurePoints.Remove(point);
            for (int i = 0; i < featurePoints.Count; i++)
            {
                double dis = feature.Geometry.Centroid.Coordinate.Distance(featurePoints[i]);
                if (min == dis)
                {
                    points.Add(feature.Geometry.Coordinates[i]);
                }
            }

            Coordinate coor = Vector.addVector(points, feature.Geometry.Centroid.Coordinate);
            Coordinate[] coors = new Coordinate[] { feature.Geometry.Centroid.Coordinate, coor };
            ILineString minLine = new LineString(coors);
            if (minLine.Intersects(feature.Geometry.Boundary))
            {
                IGeometry intersectPoints = minLine.Intersection(feature.Geometry.Boundary);

                if (intersectPoints.Coordinates.Length > 1)
                {
                    if (coors[1].X > coors[0].X)
                    {
                        minLine.Coordinates[1] = intersectPoints.Coordinates[0];
                    }
                    else if (coors[1].X < coors[0].X)
                    {
                        minLine.Coordinates[1] = intersectPoints.Coordinates[intersectPoints.Coordinates.Length - 1];
                    }
                    else if (coors[1].X == coors[0].X)
                    {
                        if (coors[1].Y > coors[0].Y)
                        {
                            minLine.Coordinates[1] = intersectPoints.Coordinates[0];
                        }
                        else if (coors[1].Y < coors[0].Y)
                        {
                            minLine.Coordinates[1] = intersectPoints.Coordinates[intersectPoints.Coordinates.Length - 1];
                        }
                    }
                }
                else
                {
                    minLine.Coordinates[1] = intersectPoints.Coordinate;
                }
            }
            return minLine;
        }
    }
}
