using Server.Data.Model.MinesweeperLogic;
using Server.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;


using Server.Data.Model.MinesweeperPresentation;

namespace Server.Data.Services {
    public class Partida :INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged( [CallerMemberName] String propertyName = "" ) {
            if( PropertyChanged != null ) {
                PropertyChanged(this,new PropertyChangedEventArgs(propertyName));
            }
        }
        


        public bool PartidaComenzada = false;

        private MinesweeperLogica logica;
        public MinesweeperLogica Logica {
            get { return this.logica; }
            set {
                if( value != this.logica ) {
                    this.logica = value;
                    NotifyPropertyChanged("Logica");
                }
            }
            
        }
        private List<Jugador> players = new();
        public List<Jugador> Players {
            get { return this.players; }
            set { 
                if( value != this.players ) {
                    this.players = value;
                    NotifyPropertyChanged("Players");
                }
            }

        }
        public string id { get; set; }
        public List<String> msgs { get; set; }

        

        private  List<List<Casilla>> casillas{get;set;}
        public List<List<Casilla>> Casillas {
            get { return this.casillas; }
            set {
                if( value != this.casillas ) {
                    this.casillas = value;
                    NotifyPropertyChanged("Casillas");
                }
            }

        }



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
            this.Logica = logica;
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


        public bool playerInRoom( String id ) {
            return this.players.Where(x => x.username == id).Count() >0;
        }

    }
}
