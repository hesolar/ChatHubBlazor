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
using System.Threading;

namespace Server.Data.Services {
    public class Partida :INotifyPropertyChanged {
       //implementar la interfaz
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged( [CallerMemberName] String propertyName="" ) {
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

        private List<List<Casilla>> casillas;
        public List<List<Casilla>> Casillas {
            get { return this.casillas; }
            set {
                if( value != this.casillas ) {
                    this.casillas = value;
                    NotifyPropertyChanged("Casillas");
                }
            }

        }

        private int currentPlayerTourn { get; set; } = 0;
        public Jugador CurrentPlayerTourn {
            get { return players[this.currentPlayerTourn]; }
            set {
                if( value != players[this.currentPlayerTourn] ) {
                    this.currentPlayerTourn =players.IndexOf(value);
                    NotifyPropertyChanged("CurrentPlayerTourn");
                }
            }
        }




        public Jugador this[int x] => players[x];
        public Jugador this[String username] => this.players.Where(x=>x.username==username).First();
        
        public int casillasAbiertas { get; set; }





   












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
            this.Casillas = createMineList(logica);
            this.casillasAbiertas = logica.rows * logica.rows;

            
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

        public async Task ComenzarPartida() {
            this.PartidaComenzada = true;
            Presentacion.UnlockBoard(Casillas);
            while( !logica.Victory() ) {
            
            Presentacion.ActualizarVentanaDeslizante(casillas);
                cronometroFuncionando = true;
             await TimeLapse();
            Presentacion.EstadoOriginalTablero(Casillas);
            NotifyPropertyChanged();
            nextTourn();
            }
        }



        public double timeToMove = 5;
        private TimeSpan valorCronometro = new TimeSpan(0,0,5);
        public TimeSpan ValorCronometro {
            get { return this.valorCronometro; }
            set {
                if( value != valorCronometro ) {
                    this.valorCronometro = value;
                    NotifyPropertyChanged();
                }
            }
        
        } 
        public bool cronometroFuncionando = false;




        public async Task StartTourn() {



            while( !Logica.Victory() ) {

                await TimeLapse();
                //if( TableroActual[TableroActual.CurrentPlayerTourn].username == username ) {

                    Presentacion.ActualizarVentanaDeslizante(Casillas);
                    valorCronometro = new TimeSpan(0,0,10);
                    cronometroFuncionando = true;

                    await TimeLapse();
                //}
            }

        }
        public async Task TimeLapse() {
            while( cronometroFuncionando && !Logica.Victory() && !Presentacion.NingunaSeleccionada(casillas)) {
                await Task.Delay(1);
                if( valorCronometro.TotalSeconds > 0 ) {
                    await Task.Delay(500);
                    valorCronometro = valorCronometro.Subtract(new TimeSpan(0,0,0,0,500));


                }
                else {
                    //valorCronometro = new TimeSpan(0,0,(int) (Math.Round(timeToMove)));
                    cronometroFuncionando = false;

                    CurrentPlayerTourn.puntuacion += -5;
                    

                }

            }
            Presentacion.EstadoOriginalTablero(Casillas);
            int miliseconds = Convert.ToInt32(1000 * (((double) this.casillasAbiertas / (double) (Logica.rows * Logica.rows)) * timeToMove));

            valorCronometro = new TimeSpan(0,0,0,0,miliseconds);
            nextTourn();
        }



    }
}
