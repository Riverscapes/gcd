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
            _currProgress = -1;
            _currState = States.None;
            _currMsg = msg;
        }

        public int Progress
        {
            get { return _currProgress; }
            set
            {
                _currProgress = value;
                Debug.WriteLine(string.Format("OpStatus:: Progress: {0}", _currProgress));
            }
        }

        public States State
        {
            get { return _currState; }
            set
            {
                _currState = value;
                Debug.WriteLine(string.Format("OpStatus:: State Change to: '{0}'", Enum.GetName(typeof(States), _currState)));
            }
        }

        public string Message
        {
            get { return _currMsg; }
            set
            {
                    _currMsg = value;
                    Debug.WriteLine(string.Format("OpStatus:: Msg Change: '{0}'", _currMsg));
            }
        }

    }
}
