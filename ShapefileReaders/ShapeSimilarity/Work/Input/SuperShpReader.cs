using GeoAPI.Geometries;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapefileReaders.ShapeSimilarity.Work.Input
{
    public  class SuperShpReader
    {
        private string shpFile;
        public SuperShpReader(string shpFile)
        {
            this.shpFile = shpFile;
        }

        public Collection<IFeature> ReadAll()
        {
            IGeometryFactory factory = GeometryFactory.Default;
            ShapefileDataReader reader = new ShapefileDataReader(shpFile, factory);
            Collection<IFeature> features = new Collection<IFeature>();
            while (reader.Read())
            {
                Feature feature = new Feature();
                feature.Geometry = reader.Geometry;
                feature.Attributes = new AttributesTable();
                for(int i = 0; i < reader.DbaseHeader.NumFields; i++)
                {
                    feature.Attributes.AddAttribute(reader.GetName(i), reader.GetValue(i));
                }
                features.Add(feature);
            }
            return features;
        }
    }
}
