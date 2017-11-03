using System;
using System.Collections.Generic;
using GCDConsoleLib.Internal.Operators;
using UnitsNet.Units;
using UnitsNet;

namespace GCDConsoleLib.GCD.Stats
{
    public class GCDAreaVolume
    {
        private int _count;
        private float _sum;

        private Area _cellArea;
        private LengthUnit _vUnit;

        public GCDAreaVolume(Area cellArea, LengthUnit vUnit)
        {
            _count = 0;
            _sum = 0;
            _cellArea = cellArea;
            _vUnit = vUnit;
        }
        public GCDAreaVolume(int count, float sum, Area cellArea, LengthUnit vUnit)
        {
            _count = count;
            _sum = sum;
            _cellArea = cellArea;
            _vUnit = vUnit;
        }

        public void IncrementCount(int num) { _count++; }
        public void AddToSum(float val) { _sum += val; }
        public void AddToSumAndIncrementCounter(float val) { _sum += val; _count++; }

        public Area GetArea(Area cellArea) { return _count * cellArea; }
        public Volume VolumeErosion(Area cellArea, LengthUnit vUnit) { return Volume.FromCubicMeters(Length.From(_sum, vUnit).Meters * cellArea.SquareMeters); }
    }

    //public class ChangeStats
    //{
    //    private GCDAreaVolume _eroRaw;
    //    private GCDAreaVolume _depRaw;
    //    private GCDAreaVolume _eroThr;
    //    private GCDAreaVolume _depThr;
    //    private GCDAreaVolume _eroErr;
    //    private GCDAreaVolume _depErr;

    //    private Area _cellArea;
    //    private LengthUnit _vUnit;

    //    public ChangeStats(ChangeStats incomingStats, Area cellArea, LengthUnit vUnit)
    //    {
    //        _cellArea = cellArea;
    //        _vUnit = vUnit;
    //    }

    //    public Area AreaErosionRaw { get{ return _eroRaw.GetArea(_cellArea); }}
    //    public Area AreaErosionThr { get { return _eroRaw.GetArea(_cellArea); } }
    //    public Area AreaErosionErr { get { return _eroRaw.GetArea(_cellArea); } }

    //}

}
