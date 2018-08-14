using NetTopologySuite.Features;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapefileReaders.ShapeSimilarity.Algorithm.Utils.Public
{
    class TouchesFeatures
    {
        public static Collection<IFeature> getTouches(Feature feature, Collection<IFeature> features)
        {
            features.Remove(feature);
            Collection<IFeature> centerFeatures = new Collection<IFeature>();
            Collection<IFeature> featuresHolder = new Collection<IFeature>();
            Collection<IFeature> featureGoals = new Collection<IFeature>();


            for (int j = 0; j < centerFeatures.Count; j++)
            {
                for (int i = 0; i < features.Count; i++)
                {
                    if (features[i].Geometry.Touches(centerFeatures[j].Geometry))
                    {
                        featuresHolder.Add(features[i]);
                        featureGoals.Add(features[i]);
                        features.Remove(features[i]);
                    }                   
                }
            }

            centerFeatures = featuresHolder;
            featuresHolder.Clear();

            return null;
        }
    }
}
