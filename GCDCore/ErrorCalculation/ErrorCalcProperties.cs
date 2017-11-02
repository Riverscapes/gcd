using System;
using System.Collections.Generic;

namespace GCDCore.ErrorCalculation
{
    public class ErrorCalcPropertiesBase
    {
        public readonly string SurveyMethod;
        public readonly string ErrorType;

        public override string ToString()
        {
            return SurveyMethod;
        }

        public ErrorCalcPropertiesBase(string sSurveyMethod, string sErrorType)
        {
            SurveyMethod = sSurveyMethod;
            ErrorType = sErrorType;
        }
    }

    public class ErrorCalcPropertiesUniform : ErrorCalcPropertiesBase
    {
        public readonly double UniformErrorValue;

        public ErrorCalcPropertiesUniform(string sSurveyMethod, double fUniformErrorValue) : base(sSurveyMethod, "Uniform Error")
        {
            if (fUniformErrorValue < 0)
            {
                throw new Exception("Invalid uniform error value");
            }

            UniformErrorValue = fUniformErrorValue;
        }
    }

    public class ErrorCalcPropertiesFIS : ErrorCalcPropertiesBase
    {
        public readonly Dictionary<string, int> FISInputs;
        public readonly System.IO.FileInfo FISRuleFilePath;
        public readonly int FISID;

        public ErrorCalcPropertiesFIS(string sSurveyMethod, System.IO.FileInfo sFISRuleFilePath, Dictionary<string, int> dFISInputs) : base(sSurveyMethod, "FIS Error")
        {
            FISRuleFilePath = sFISRuleFilePath;

            if (dFISInputs == null)
            {
                FISInputs = new Dictionary<string, int>();
            }
            else
            {
                FISInputs = dFISInputs;
            }
        }
    }

    public class ErrorCalcPropertiesAssoc : ErrorCalcPropertiesBase
    {
        public readonly int AssociatedSurfaceID;

        public ErrorCalcPropertiesAssoc(string sSurveyMethod, int nAssociatedSurfaceID)
            : base(sSurveyMethod, "Associated Surface")
        {
            AssociatedSurfaceID = nAssociatedSurfaceID;
        }
    }
}