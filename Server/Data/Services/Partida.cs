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
using PropertyChanged;

namespace Server.Data.Services {
    public class Partida :INotifyPropertyChanged {
       //implementar la interfaz
       
       

        public Partida( MinesweeperLogica logica,string id ) {
            this.Logica = logica;
            this.id = id;
            this.msgs = new List<String>();
            this.Casillas = createMineList(logica);
            this.casillasAbiertas = logica.rows * logica.rows;

        }

        public bool PartidaComenzada = false;

        public MinesweeperLogica Logica { get; set; }
  


        public List<Jugador> Players{get; set; }=new();
    
        
        public string id { get; set; }
        public List<String> msgs { get; set; }

        public List<List<Casilla>> Casillas { get; set; }
      

        private int currentPlayerTourn { get; set; } = 0;
        public Jugador CurrentPlayerTourn() {
           return this.Players[this.currentPlayerTourn];   
        }




        public Jugador this[int x] => Players[x];
        public Jugador this[String username] => this.Players.Where(x=>x.username==username).First();
        
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

        

        public bool AddPlayer(String newPLayer ) {

            if( !Players.Any(x=>x.username==newPLayer)) {

                if( !this.PartidaComenzada ) {
                    this.Players.Add(new Jugador(newPLayer));
                    return true;
                }
            }
            return false;
           
        }

        public void nextTourn() {
            currentPlayerTourn++;
            if( this.currentPlayerTourn >= Players.Count ) currentPlayerTourn = 0;
        }


        public bool playerInRoom( String id ) {
            return this.Players.Where(x => x.username == id).Count() >0;
        }

        public async Task ComenzarPartida() {
            this.PartidaComenzada = true;
            Presentacion.UnlockBoard(Casillas);
            while( !Logica.Victory() ) {
            
            Presentacion.ActualizarVentanaDeslizante(Casillas);
            cronometroFuncionando = true;
            await TimeLapse();
            Presentacion.EstadoOriginalTablero(Casillas);
            nextTourn();
            }
        }



        public double timeToMove = 5;
        public TimeSpan ValorCronometro { get; set; } = new TimeSpan(0,0,5);
        
        public bool cronometroFuncionando = false;

        public event PropertyChangedEventHandler PropertyChanged;

        public async Task StartTourn() {



            while( !Logica.Victory() ) {

                await TimeLapse();
                //if( TableroActual[TableroActual.CurrentPlayerTourn].username == username ) {

                    Presentacion.ActualizarVentanaDeslizante(Casillas);
                    ValorCronometro = new TimeSpan(0,0,10);
                    cronometroFuncionando = true;

                    await TimeLapse();
                //}
            }

        }
        public async Task TimeLapse() {
            while( cronometroFuncionando && !Logica.Victory() && !Presentacion.NingunaSeleccionada(Casillas)) {
                await Task.Delay(1);
                if( ValorCronometro.TotalSeconds > 0 ) {
                    await Task.Delay(500);
                    ValorCronometro = ValorCronometro.Subtract(new TimeSpan(0,0,0,0,500));


                }
                else {
                    //valorCronometro = new TimeSpan(0,0,(int) (Math.Round(timeToMove)));
                    cronometroFuncionando = false;

                    CurrentPlayerTourn().puntuacion += -5;
                    

                }

            }
            Casillas.ForEach(x => x.ForEach(x => x.EstadoOriginal()));

            int miliseconds = Convert.ToInt32(1000 * (((double) this.casillasAbiertas / (double) (Logica.rows * Logica.rows)) * timeToMove));

            ValorCronometro = new TimeSpan(0,0,0,0,miliseconds);

        }



    }
}
