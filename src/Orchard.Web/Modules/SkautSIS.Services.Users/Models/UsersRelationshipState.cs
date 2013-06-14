using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkautSIS.Services.Users.Models
{
    public enum State
    {
        Approved,
        Disapproved,
        Pending
    }

    public class StateWrapper 
    {
        private State _t;

        public int Value
        {
            get { return (int) _t; }
            set { _t = (State) value; }
        }

        public State EnumValue
        {
            get { return _t; }
            set { _t = value; }
        }

        public static implicit operator StateWrapper(State s)
        {
            return new StateWrapper { EnumValue = s };
        }

        public static implicit operator State(StateWrapper sw)
        {
            if (sw == null) return State.Pending;
            else return sw.EnumValue;
        }
    }
}