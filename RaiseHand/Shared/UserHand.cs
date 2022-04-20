using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaiseHand.Shared
{
    public struct UserHand
    {
        public UserHand() : this(string.Empty, false) { }
        public UserHand(string username) : this(username, false) { }
        public UserHand(string username, bool handRaised)
        {
            Username = username;
            HandRaised = handRaised;
        }

        public string Username;
        public bool HandRaised;
    }
}
