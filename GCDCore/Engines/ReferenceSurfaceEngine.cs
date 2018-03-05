using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCDCore.Project;

namespace GCDCore.Engines
{
    public class ReferenceSurfaceEngine
    {
        public readonly string Name;
        public readonly List<Tuple<Project.DEMSurvey, Project.ErrorSurface>> DEMSurveys;
        public readonly GCDConsoleLib.RasterOperators.MultiMathOpType Method;

        public string ErrorSurfaceName
        {
            get
            {
                switch (Method)
                {
                    case GCDConsoleLib.RasterOperators.MultiMathOpType.Maximum:
                        return "Max Error Surface";

                    case GCDConsoleLib.RasterOperators.MultiMathOpType.Minimum:
                        return "Min Error Surface";

                    // Both the mean and standard deviation use the mean
                    case GCDConsoleLib.RasterOperators.MultiMathOpType.Mean:
                    case GCDConsoleLib.RasterOperators.MultiMathOpType.StandardDeviation:
                        return "Mean Error Surface";

                    default:
                        throw new Exception("Unhandled math operation type " + Method.ToString());
                }
            }
        }

        public ReferenceSurfaceEngine(string name, List<Tuple<Project.DEMSurvey, Project.ErrorSurface>> demSurveys,
             GCDConsoleLib.RasterOperators.MultiMathOpType eMethod)
        {
            Name = name;
            DEMSurveys = demSurveys;
            Method = eMethod;
        }

        public Surface Run(FileInfo rsPath, FileInfo errPath)
        {
            GCDConsoleLib.Raster rSurface = BuildReferenceSurface(rsPath);
            GCDConsoleLib.Raster rErrorSf = BuildErrorSurface(errPath);
            GCDConsoleLib.Raster rHillshd = GCDConsoleLib.RasterOperators.Hillshade(rSurface, Surface.HillShadeRasterPath(rsPath));

            Surface refSurf = new GCDCore.Project.Surface(Name, rSurface, rHillshd);
            refSurf.ErrorSurfaces.Add(new ErrorSurface(ErrorSurfaceName, errPath, refSurf));

            ProjectManager.Project.ReferenceSurfaces[refSurf.Name] = refSurf;
            ProjectManager.Project.Save();

            return refSurf;
        }

        private GCDConsoleLib.Raster BuildReferenceSurface(FileInfo outputPath)
        {
            // Ensure the output path exists
            outputPath.Directory.Create();

            List<GCDConsoleLib.Raster> demRasters = new List<GCDConsoleLib.Raster>(DEMSurveys.Select(x => x.Item1.Raster));

            switch (Method)
            {
                case GCDConsoleLib.RasterOperators.MultiMathOpType.Maximum:
                    return GCDConsoleLib.RasterOperators.Maximum(demRasters, outputPath);

                case GCDConsoleLib.RasterOperators.MultiMathOpType.Mean:
                    return GCDConsoleLib.RasterOperators.Mean(demRasters, outputPath);

                case GCDConsoleLib.RasterOperators.MultiMathOpType.Minimum:
                    return GCDConsoleLib.RasterOperators.Minimum(demRasters, outputPath);

                case GCDConsoleLib.RasterOperators.MultiMathOpType.StandardDeviation:
                    return GCDConsoleLib.RasterOperators.StandardDeviation(demRasters, outputPath);

                default:
                    throw new Exception("Unhandled math operation type " + Method.ToString());
            }
        }

        private GCDConsoleLib.Raster BuildErrorSurface(FileInfo outputPath)
        {
            // Ensure the output path exists
            outputPath.Directory.Create();

            List<GCDConsoleLib.Raster> demRasters = new List<GCDConsoleLib.Raster>(DEMSurveys.Select(x => x.Item1.Raster));
            List<GCDConsoleLib.Raster> errRasters = new List<GCDConsoleLib.Raster>(DEMSurveys.Select(x => x.Item2.Raster));

            switch (Method)
            {
                case GCDConsoleLib.RasterOperators.MultiMathOpType.Maximum:
                    return GCDConsoleLib.RasterOperators.MaximumErr(demRasters, errRasters, outputPath);

                case GCDConsoleLib.RasterOperators.MultiMathOpType.Minimum:
                    return GCDConsoleLib.RasterOperators.MinimumErr(demRasters, errRasters, outputPath);

                // Both the mean and standard deviation use the mean
                case GCDConsoleLib.RasterOperators.MultiMathOpType.Mean:
                case GCDConsoleLib.RasterOperators.MultiMathOpType.StandardDeviation:
                    return GCDConsoleLib.RasterOperators.MultiRootSumSquares(errRasters, outputPath);

                default:
                    throw new Exception("Unhandled math operation type " + Method.ToString());
            }
        }
    }
}
