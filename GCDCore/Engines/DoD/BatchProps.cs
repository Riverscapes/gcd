using System;
using GCDCore.Project;

namespace GCDCore.Engines.DoD
{
    /// <summary>
    /// Represents a single DoD batch item
    /// </summary>
    public class BatchProps
    {
        public Surface NewSurface { get; private set; }
        public ErrorSurface NewError { get; private set; }
        public Surface OldSurface { get; private set; }
        public ErrorSurface OldError { get; private set; }
        public ThresholdProps ThresholdProps { get; private set; }
        public Project.Masks.AOIMask AOIMask { get; private set; }

        public string NewSurfaceName { get { return NewSurface.Name; } }

        /// <summary>
        /// Default constructor needed for binding;
        /// </summary>
        public BatchProps()
        {

        }

        public BatchProps(Surface newSurface, ErrorSurface newError, Surface oldSurface, ErrorSurface oldError, GCDCore.Project.Masks.AOIMask aoiMask, ThresholdProps tProps)
        {
            NewSurface = newSurface;
            NewError = newError;
            OldSurface = oldSurface;
            OldError = oldError;
            AOIMask = aoiMask;
            ThresholdProps = tProps;
        }
    }
}
