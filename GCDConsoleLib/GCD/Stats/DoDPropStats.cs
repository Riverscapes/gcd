using System;
using System.Collections.Generic;
using UnitsNet.Units;
using UnitsNet;
using GCDConsoleLib.Internal.Operators;

namespace GCDConsoleLib.GCD.Stats
{
    public class DoDPropStats
    {
        public GCDAreaVolume ErosionRaw, DepositionRaw, ErosionThr, 
            DepositionThr, ErosionErr, DepositionErr;

        public DoDPropStats(Area cellArea, LengthUnit vUnit)
        {
            ErosionRaw = new GCDAreaVolume(cellArea, vUnit);
            DepositionRaw = new GCDAreaVolume(cellArea, vUnit);
            ErosionThr = new GCDAreaVolume(cellArea, vUnit);
            DepositionThr = new GCDAreaVolume(cellArea, vUnit);
            ErosionErr = new GCDAreaVolume(cellArea, vUnit);
            DepositionErr = new GCDAreaVolume(cellArea, vUnit);
        }

    }
}
