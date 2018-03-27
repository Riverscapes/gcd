using System;
using System.Collections.Generic;

namespace GCDConsoleLib.FIS
{
    public class Rule
    {
        public List<int> InputInd;
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
            InputInd = new List<int>();
            MFSInd = new List<int>();
            MFSNot = new List<bool>();
        }

        /// <summary>
        /// Add a member function to this rule
        /// </summary>
        /// <param name="inputIndex">The input order index of this input</param>
        /// <param name="mfNum">The number (base 1) of the MF</param>
        public void addMf(int inputIndex, int mfNum)
        {
            if (mfNum == 0)
                return;

            InputInd.Add(inputIndex);

            // Here's where we parse the NOT rule when the mfIndex is negative
            // mfsNOT is a flag that means "Use the NOT operator here"
            // Also note that we're storing the ARRAY INDEX, not the rule number
            if (mfNum < 0)
            {
                MFSInd.Add(Math.Abs(mfNum) - 1);
                MFSNot.Add(true);
            }
            else
            {
                MFSInd.Add(mfNum - 1);
                MFSNot.Add(false);
            }
        }

    }
}
