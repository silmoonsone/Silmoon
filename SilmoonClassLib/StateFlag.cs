using System.Collections;
using System.Collections.Specialized;
using Silmoon.Reflection;
using System;

namespace Silmoon
{
    [Serializable]
    public class StateFlag : IStateFlag
    {
        int _id = -1;
        bool _booleanFlag = false;
        int _intStateFlag = -1;
        object _objectRef = null;
        string _message = string.Empty;
        string _stringFlag = string.Empty;
        bool _error = false;
        ArrayList _objectArray = new ArrayList();
        NameValueCollection _parameters = new NameValueCollection();
        NameObjectCollection _objects = new NameObjectCollection();



        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }
        public bool BooleanFlag
        {
            get { return _booleanFlag; }
            set { _booleanFlag = value; }
        }
        public string StringFlag
        {
            get { return _stringFlag; }
            set { _stringFlag = value; }
        }
        public ArrayList ObjectArray
        {
            get { return _objectArray; }
            set { _objectArray = value; }
        }
        public NameValueCollection Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }
        public NameObjectCollection Objects
        {
            get { return _objects; }
            set { _objects = value; }
        }


        #region IStateFlag 成员

        public int Code
        {
            get
            {
                return _intStateFlag;
            }
            set
            {
                _intStateFlag = value;
            }
        }

        public bool Success
        {
            get
            {
                return _error;
            }
            set
            {
                _error = value;
            }
        }

        public object UserState
        {
            get { return _objectRef; }
            set { _objectRef = value; }
        }

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        #endregion
    }
}
