using System;

namespace GCDCore.Project
{
    public class SurveyDateTime : IComparable<SurveyDateTime>
    {
        public const string NotSetString = "Not Set";

        public ushort Year { get; set; }
        private byte m_nMonth;

        private byte m_nDay;
        private short m_nHour;
        private short m_nMin;

        public byte Month
        {
            get { return m_nMonth; }
            set
            {
                if (value < 0 || value > 12)
                {
                    throw new ArgumentOutOfRangeException("value", value, "Invalid month value.");
                }
                else
                {
                    m_nMonth = value;
                }
            }
        }

        public byte Day
        {
            get { return m_nDay; }
            set
            {
                if (value > 31)
                {
                    throw new ArgumentOutOfRangeException("value", value, "Invalid day value.");
                }
                else
                {
                    m_nDay = value;
                }
            }
        }

        public short Hour
        {
            get { return m_nHour; }
            set
            {
                if (value < -1 || value > 23)
                {
                    throw new ArgumentOutOfRangeException("value", value, "Invalid hour value.");
                }
                else
                {
                    m_nHour = value;
                }
            }
        }

        /// <summary>
        /// Minutes of the hour
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>Note that zero minutes is valid. Therefore a null minute value is -1</remarks>
        public short Minute
        {
            get { return m_nMin; }
            set
            {
                if (value < -1 || value > 59)
                {
                    throw new ArgumentOutOfRangeException("value", value, "Invalid minute value.");
                }
                else
                {
                    m_nMin = value;
                }
            }
        }

        public SurveyDateTime()
        {
            // The default for all values is Null, stored as zero
            Year = 0;
            m_nMonth = 0;
            m_nDay = 0;

            m_nHour = -1;
            m_nMin = -1;
        }
     
        public override string ToString()
        {

            string sDate = string.Empty;
            if (Year > 0)
            {
                if (m_nMonth > 0)
                {
                    if (m_nDay > 0)
                    {
                        DateTime dt = new DateTime(Year, m_nMonth, m_nDay);
                        sDate = dt.ToString("yyyy MMM dd");
                    }
                    else
                    {
                        DateTime dt = new DateTime(Year, m_nMonth, 1);
                        sDate = dt.ToString("yyyy MMM");
                    }
                }
                else
                {
                    sDate = Year.ToString();
                }
            }

            string sTime = string.Empty;
            if (m_nHour >= 0)
            {
                // Note that minutes can be zero
                if (m_nMin >= 0)
                {
                    sTime = string.Format("{0:00}:{1:00}", m_nHour, m_nMin);
                }
                else
                {
                    sTime = m_nHour.ToString("00");
                }
            }

            string sResult = string.Empty;
            if (string.IsNullOrEmpty(sDate))
            {
                if (string.IsNullOrEmpty(sTime))
                {
                    sResult = NotSetString;
                }
                else
                {
                    sResult = sTime;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(sTime))
                {
                    sResult = sDate;
                }
                else
                {
                    sResult = string.Format("{0} {1}", sDate, sTime);
                }
            }

            return sResult;

        }

        public int CompareTo(SurveyDateTime other)
        {

            int nResult = Year - other.Year;
            if (nResult == 0)
            {
                nResult = m_nMonth - other.Month;
                if (nResult == 0)
                {
                    nResult = m_nDay - other.Day;
                    if (nResult == 0)
                    {
                        nResult = m_nHour - other.Hour;
                        if (nResult == 0)
                        {
                            nResult = m_nMin - other.Minute;
                        }
                    }
                }
            }

            return nResult;
        }
    }
}