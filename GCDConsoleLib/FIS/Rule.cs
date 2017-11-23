using System;
using System.Collections.Generic;

namespace GCDConsoleLib.FIS
{
    public class Rule
    {
        public List<int> Inputs;
        public List<int> MFSInd;
        public List<bool> MFSNot;

        public double Weight;
        public FISOperator Operator;
        public MemberFunction Output;

        /// <summary>
        /// Constructor
        /// </summary>
        public Rule()
        {
            Weight = 1;
            Inputs = new List<int>();
            MFSInd = new List<int>();
            MFSNot = new List<bool>();
        }

        /// <summary>
        /// Add a member function to this rule
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
                MFSNot.Add(true);
            }
            else
            {
                MFSInd.Add(mfIndex - 1);
                MFSNot.Add(false);
            }
        }

    }
}
