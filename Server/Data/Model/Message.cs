using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Data.Model {
    public class Message {

        public string Username { get; set; }
        public string Body { get; set; }
        public bool Mine { get; set; }


        public Message( string username,string body,bool mine ) {
            Username = username;
            Body = body;
            Mine = mine;
        }



        public bool IsNotice => Body.StartsWith("[Notice]");

        public string CSS => Mine ? "sent" : "received";


    }
}
