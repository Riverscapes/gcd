using System;
using System.Diagnostics;

namespace GCDConsoleLib
{
    public class OpStatus
    {
        public enum States { None, Initialized, Started, Complete }

        private int _currProgress;
        private States _currState;
        private string _currMsg;

        public OpStatus(string msg = "")
        {
            _currProgress = 0;
            _currState = States.None;
            _currMsg = msg;
        }

        public int Progress
        {
            get { return _currProgress; }
            set
            {
                _currProgress = value;
            }
        }

        public States State
        {
            get { return _currState; }
            set
            {
                _currState = value;
            }
        }

        public string Message
        {
            get { return _currMsg; }
            set
            {
                _currMsg = value;
            }
        }


    }
}
