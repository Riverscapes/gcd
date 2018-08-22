using System;
using System.ComponentModel;

namespace GCDConsoleLib
{
    /// <summary>
    /// Lightweight class for tracking the status of an individual operation
    /// </summary>
    public class OpStatus
    {
        public enum States { None, Initialized, Started, Complete }

        public OpStatus(string msg = "")
        {
            Progress = 0;
            State = States.None;
            Message = msg;
        }

        public int Progress { get; set; }
        public States State { get; set; }
        public string Message { get; set; }

        public static explicit operator OpStatus(ProgressChangedEventArgs v)
        {
            throw new NotImplementedException();
        }
    }
}
