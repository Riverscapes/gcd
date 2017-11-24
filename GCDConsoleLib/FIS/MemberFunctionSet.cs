using System;
using System.Collections.Generic;

namespace GCDConsoleLib.FIS
{
    public class MemberFunctionSet
    {
        private double _min, _max;
        public List<MemberFunction> MFunctions;
        public Dictionary<String, int> Indices;

        /// <summary>
        /// 
        /// </summary>
        public MemberFunctionSet() : base()
        {
            Indices = new Dictionary<String, int>();
            MFunctions = new List<MemberFunction>();
            _min = 0;
            _max = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public MemberFunctionSet(double min, double max)
        {
            _min = min;
            _max = max;
            Indices = new Dictionary<String, int>();
            MFunctions = new List<MemberFunction>();
            if (min >= max)
                throw new ArgumentException("Invalid range. Max must be greater than min.");
        }

        /// <summary>
        /// Check if a membership function is valid.
        /// </summary>
        /// <returns></returns>
        public bool Valid
        {
            get
            {
                if (MFunctions.Count == 0)
                    return false;
                for (int i = 0; i < MFunctions.Count; i++)
                    if (!MFunctions[i].Valid)
                        return false;
                return true;
            }
        }

        /// <summary>
        /// Get the number of MemberFunctions in this set
        /// </summary>
        public int Count { get { return MFunctions.Count; } }

        /// <summary>
        /// Add a member function to the set.
        /// </summary>
        /// <param name="sName">The name of the member function. Cannot contain spaces</param>
        /// <param name="mf">The member function to add.</param>
        public void addMF(string sName, MemberFunction mf)
        {
            if (0 == mf.Length)
                throw new ArgumentException("The membership function cannot be added to the set because it has no vertices.");
            else if ((mf.Coords[0][0] < _min) || (mf.Coords[mf.Length - 1][0] > _max))
                throw new ArgumentException(string.Format("Membership function bounds ({0} {1}) do not fit in the set range ({2}) for this object.", mf.Coords[0][0], mf.Coords[mf.Length - 1][0], _min));
            else if (Indices.ContainsKey(sName))
                throw new ArgumentException(string.Format("The name '{0}' is already in use.", sName));
            else if (sName.Contains(" "))
                throw new ArgumentException(string.Format("Invalid name '{0}'. Spaces are not allowed.", sName));
            else
            {
                MFunctions.Add(mf);
                Indices[sName] = MFunctions.Count -1;
            }
        }

        /// <summary>
        /// Add a triangle member function to the set.
        /// The x coordinates must be in order from smallest to largest. The y values corresponding to x1 and
        /// x3 are assumed to be 0
        /// </summary>
        /// <param name="sName">The name of the member function. Cannot contain spaces</param>
        /// <param name="x1">The first x coordinate</param>
        /// <param name="x2">The second x coordinate</param>
        /// <param name="x3">The third x coordinate</param>
        /// <param name="yMax">The y value at x2. Must be in the interval (0,1]. (Optional, defaults to 1.)</param>
        public void addTriangleMF(string sName, double x1, double x2, double x3, double yMax)
        {
            if ((x1 < _min) || (x3 > _max))
                throw new ArgumentException(string.Format("Membership function bounds ({0} {1}) do not fit in the set range ({2} {3}) for this object.", x1, x3, _min, _max));
            else if (Indices.ContainsKey(sName))
                throw new ArgumentException(string.Format("The name '{0}' is already in use.", sName));
            else if (sName.Contains(" "))
                throw new ArgumentException(string.Format("Invalid name '{0}'. Spaces are not allowed.", sName));
            else
            {
                MFunctions.Add(new MemberFunction(x1, x2, x3, yMax));
                Indices[sName] = MFunctions.Count -1;
            }
        }

        /// <summary>
        /// Add a trapezoidal member function to the set.
        /// The x coordinates must be in order from smallest to largest. The y values corresponding to x1 and
        /// x4 are assumed to be 0.
        /// </summary>
        /// <param name="sName">The name of the member function. Cannot contain spaces.</param>
        /// <param name="x1">The first x coordinate</param>
        /// <param name="x2">The second x coordinate</param>
        /// <param name="x3">The third x coordinate</param>
        /// <param name="x4">The fourth x coordinate</param>
        /// <param name="yMax">The y value at x2 and x3. Must be in the interval (0,1]. (Optional, defaults to 1.)</param>
        public void addTrapezoidMF(String sName, double x1, double x2, double x3, double x4, double yMax = 1)
        {
            if ((x1 < _min) || (x4 > _max))
                throw new ArgumentException(string.Format("Membership function bounds ({0} {1}) do not fit in the set range ({2} {3}) for this object.", x1, x4, _min, _max));
            else if (Indices.ContainsKey(sName))
                throw new ArgumentException(string.Format("The name '{0}' is already in use.", sName));
            else if (sName.Contains(" "))
                throw new ArgumentException(string.Format("Invalid name '{0}'. Spaces are not allowed.", sName));
            else
            {
                MFunctions.Add(new MemberFunction(x1, x2, x3, x4, yMax));
                Indices[sName] = MFunctions.Count -1;
            }
        }

    }
}
