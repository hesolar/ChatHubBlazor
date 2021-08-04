using BlazorApp.Server.Data.Model.MinesweeperLogic;
using Server.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Data.Services {
    public class PartidaService {
        public Partida[] partidas { get; set; }
        public PartidaService(string test = "Hola") {
            partidas= GetPartidaAsync().Result;
        }


        public Task<Partida[]> GetPartidaAsync() {
            return Task.FromResult(Enumerable.Range(1,5).Select(index => new Partida(new MinesweeperLogic(10),index.ToString())
            
           ).ToArray()); 
        }

      


    }
}
