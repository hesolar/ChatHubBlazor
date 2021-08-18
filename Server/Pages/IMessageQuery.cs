using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Server.Pages {
    public class IMessageQuery {
        private StreamReader reader ;
        private StreamWriter writer;
        public String FileName { get; set; }
        public List<String> Messages { get; set; }


        public IMessageQuery(String FileName ) {
            
            this.FileName = FileName;
            Messages = RetrieveAll();
            
        }
     

        public List<String> AddMessage(String s ) {
            try { 
            writer = new(FileName,append:true);
            writer.Write(s);
            writer.Close();
            }
            catch(System.IO.IOException e ) {

            }
            return RetrieveAll();
        }

        public List<String> RetrieveAll() {
           
            reader = new(FileName);
            
            var x = reader.ReadToEnd();
            Messages = x.Split(" ").ToList();
            reader.Close();

            return this.Messages;
        }

    }
}
