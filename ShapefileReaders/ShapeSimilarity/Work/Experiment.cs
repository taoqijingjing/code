using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using ShapefileReaders.ShapeSimilarity.Algorithm;
using ShapefileReaders.ShapeSimilarity.Work.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapefileReaders.ShapeSimilarity.Work
{
    public class Experiment
    {
        private string intputShpFile;
        private string outputCSVFile;

        public Experiment(string inShp, string outCsv)
        {
            this.intputShpFile = inShp;
            this.outputCSVFile = outCsv;
        }

        public void Run(IAlogritm alog)
        {
            //SuperShpReader shp = new SuperShpReader(this.intputShpFile);
            //Collection<IFeature> features = shp.ReadAll();

            //Index inds = new IndexsOfPolygon();
            //foreach (var f in features)
            //{
            //    Index ind = alog.Caculate(f.Geometry as  Geometry);
            //    ind.Head = alog.Name;

            //    inds.Add(ind);
            //}

            //Indexs.WriteToCSV(inds, outputCSVFile);
        }

        public void Run(IList<IAlogritm> alogs)
        {
            foreach (var alog in alogs)
            {
                Run(alog);
            }
              
        }
    }
}
