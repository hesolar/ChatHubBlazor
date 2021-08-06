using Server.Data.Model.MinesweeperLogic;
using Server.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.Data.Model.MinesweeperPresentation;

namespace Server.Data.Services {
    public class Partida {
        public bool PartidaComenzada = false;
        public MinesweeperLogica logica { get; set; }
        public string id { get; set; }
        public List<String> msgs { get; set; }

        public List<Jugador> players { get; set; } = new();

        public  List<List<Casilla>> casillas{get;set;}

        public Jugador this[int x] => players[x];
        public Jugador this[String username] => this.players.Where(x=>x.username==username).First();
        public int currentPlayerTourn { get; set; } = 0;
        public int dificultad { get; set; } = 0;

        public List<List<Casilla>> createMineList(MinesweeperLogica logica) {
            //referencia
            int rows = logica.rows;
            var result = new List<List<Casilla>>();
            for( var x = 0; x < rows; x++ ) {
                var row = new List<Casilla>();
                for( var y = 0; y < rows; y++ ) {
                    row.Add(new Casilla(x,y,logica.IsBomb(x,y)));
                }
                result.Add(row);
            }
            return result;
        }

        public Partida( MinesweeperLogica logica,string id ) {
            this.logica = logica;
            this.id = id;
            this.msgs = new List<String>();
            this.casillas = createMineList(logica);

            
        }

        public bool AddPlayer(String newPLayer ) {

            if( !players.Any(x=>x.username==newPLayer)) {

                if( !this.PartidaComenzada ) {
                    this.players.Add(new Jugador(newPLayer));
                    return true;
                }
            }
            return false;
           
        }

        public void nextTourn() {
            currentPlayerTourn++;
            if( this.currentPlayerTourn >= players.Count ) currentPlayerTourn = 0;
        }
    }
}
