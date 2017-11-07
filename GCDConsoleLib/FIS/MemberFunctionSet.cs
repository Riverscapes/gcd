using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDConsoleLib.FIS
{
    public class MemberFunctionSet
    {
        /// <summary>
        /// 
        /// </summary>
        public MemberFunctionSet()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public MemberFunctionSet(double min, double max)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sName"></param>
        /// <param name="mf"></param>
        /// <returns></returns>
        public bool addMF(string sName, MemberFunction mf)
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sName"></param>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <param name="x3"></param>
        /// <param name="yMax"></param>
        /// <returns></returns>
        public bool addTriangleMF(String sName, double x1, double x2, double x3, double yMax = 1)
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sName"></param>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <param name="x3"></param>
        /// <param name="x4"></param>
        /// <param name="yMax"></param>
        /// <returns></returns>
        public bool addTrapezoidMF(String sName, double x1, double x2, double x3, double x4, double yMax = 1)
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String getMsg()
        {
            return "";
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public bool valid()
        {
            return true;
        }


    }
}
