using BlazorApp.Server.Data.Model.MinesweeperLogic;
using Server.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Data.Services {
    public class Partida {

        public MinesweeperLogic logica { get; set; }
        public string id { get; set; }
        public List<String> msgs { get; set; }

        public List<String> players { get; set; } = new();

        public  List<List<Casilla>> casillas{get;set;}

        public string this[int x] => players[x];

        public int currentPlayerTourn { get; set; } = 0;

        public List<List<Casilla>> createMineList(MinesweeperLogic logica) {
            //referencia
            var result = new List<List<Casilla>>();
            for( var x = 0; x < logica.rows; x++ ) {
                var row = new List<Casilla>();
                for( var y = 0; y < logica.rows; y++ ) {
                    row.Add(new Casilla(x,y,logica.IsBomb(x,y)));
                }
                result.Add(row);
            }
            return result;
        }

        public Partida( MinesweeperLogic logica,string id ) {
            this.logica = logica;
            this.id = id;
            this.msgs = new List<String>();
            this.casillas = createMineList(logica);
            
        }

        public void AddPlayer(String newPLayer ) {
            this.players.Add(newPLayer);
        }

        public void nextTourn() {
            currentPlayerTourn++;
            if( this.currentPlayerTourn >= players.Count ) currentPlayerTourn = 0;
        }
    }
}
