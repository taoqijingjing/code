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
    public static class MaxPoints
    {
        public static ILineString getMaxVector(IFeature feature)
        {
            double max = 0;
            Coordinate point = new Coordinate();
            IList<Coordinate> points = new List<Coordinate>();

            IList<Coordinate> featurePoints = new List<Coordinate>();
            foreach (var dian in feature.Geometry.Coordinates)
            {
                featurePoints.Add(dian);
            }

            for (int i = 0; i < feature.Geometry.NumPoints; i++)
            {
                double dis = feature.Geometry.Centroid.Coordinate.Distance(feature.Geometry.Coordinates[i]);
                if (max < dis)
                {
                    max = dis;
                    point = feature.Geometry.Coordinates[i];
                }
            }
            points.Add(point);
            featurePoints.Remove(point);
            for (int i = 0; i < featurePoints.Count; i++)
            {
                double dis = feature.Geometry.Centroid.Coordinate.Distance(featurePoints[i]);
                if (max == dis)
                {
                    points.Add(featurePoints[i]);                   
                }
            }

            Coordinate coor = Vector.addVector(points, feature.Geometry.Centroid.Coordinate);
            Coordinate[] coors = new Coordinate[] {feature.Geometry.Centroid.Coordinate, coor };
            ILineString maxLine = new LineString(coors);
            if (maxLine.Intersects(feature.Geometry.Boundary))
            {
                IGeometry intersectPoints = maxLine.Intersection(feature.Geometry.Boundary);

                if (intersectPoints.Coordinates.Length > 1)
                {
                    if (coors[1].X > coors[0].X)
                    {
                        maxLine.Coordinates[1] = intersectPoints.Coordinates[intersectPoints.Coordinates.Length - 1];
                    }
                    else if (coors[1].X < coors[0].X)
                    {
                        maxLine.Coordinates[1] = intersectPoints.Coordinates[0];
                    }
                    else if (coors[1].X == coors[0].X)
                    {
                        if (coors[1].Y > coors[0].Y)
                        {
                            maxLine.Coordinates[1] = intersectPoints.Coordinates[intersectPoints.Coordinates.Length - 1];
                        }
                        else if (coors[1].Y < coors[0].Y)
                        {
                            maxLine.Coordinates[1] = intersectPoints.Coordinates[0];
                        }
                    }
                }
                else
                {
                    maxLine.Coordinates[1] = intersectPoints.Coordinate;
                }
            }
            return maxLine;
        }
    }
}
