using Server.Data.Model.MinesweeperLogic;
using Server.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Server.Data.Services {
    public class PartidaService {
        public Partida[] partidas;
        
    


        public PartidaService(string test = "Hola") {
            partidas= GetPartidaAsync().Result;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Task<Partida[]> GetPartidaAsync() {
            return Task.FromResult(Enumerable.Range(1,5).Select(index => new Partida(new MinesweeperLogica(20),index.ToString())
            
           ).ToArray()); 
        }

      


    }
}
