using System;
using System.Collections.Generic;

namespace GCDConsoleLib.FIS
{
    class Rule
    {
        public List<int> Inputs;
        public List<int> MFSInd;
        public List<int> MFSNot;
        private int _weight;
        public MemberFunction Output;

        public Rule()
        {
            _weight = 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputIndex"></param>
        /// <param name="mfIndex"></param>
        public void addMf(int inputIndex, int mfIndex)
        {
            if (mfIndex == 0)
                return;

            Inputs.Add(inputIndex);

            // Here's where we parse the NOT rule when the mfIndex is negative
            // mfsNOT is a flag that means "Use the NOT operator here"
            // Also note that we're storing the ARRAY INDEX, not the rule number
            if (mfIndex < 0)
            {
                MFSInd.Add(Math.Abs(mfIndex) - 1);
                MFSNot.Add(1);
            }
            else
            {
                MFSInd.Add(mfIndex - 1);
                MFSNot.Add(0);
            }
        }
    }
}
