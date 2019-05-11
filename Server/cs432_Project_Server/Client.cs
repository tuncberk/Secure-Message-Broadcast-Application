using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace cs432_Project_Server
{
    class Client
    {
        public Socket socket;
        public string username;
        public string passwordHash;
        public byte[] sessionKeyEnc;
        public byte[] sessionKeyAuth;
        public byte[] challenge;


        public Client(string username, string passwordHash)
        {
            this.username = username;
            this.passwordHash = passwordHash;
        }
    }
}
