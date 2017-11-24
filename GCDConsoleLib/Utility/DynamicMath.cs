using System;


namespace GCDConsoleLib.Utility
{
    /// <summary>
    /// You can't just do (T * T) so we wrote a few helper methods to do this for us.
    /// </summary>
    public static class DynamicMath
    {
        public static dynamic Add(dynamic a, dynamic b) { return a + b; }
        public static dynamic Subtract(dynamic a, dynamic b) { return a - b; }
        public static dynamic Multiply(dynamic a, dynamic b) { return a * b; }
        public static dynamic Divide(dynamic a, dynamic b) { return a / b; }

    }
}
