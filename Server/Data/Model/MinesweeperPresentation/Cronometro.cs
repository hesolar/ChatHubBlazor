using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Data.Model.MinesweeperPresentation {
    public class Cronometro {
        private CancellationTokenSource tokenSource2;
        private CancellationToken ct;

        int timeToMove;
        public TimeSpan valorCronometro { get; set; }
        public bool cronometroFuncionando;
        public  bool botonStartActivo { get; set; }

        public Cronometro( int timeToMove ) {
            this.tokenSource2 = new CancellationTokenSource();
            this.ct = tokenSource2.Token;
            this.timeToMove = timeToMove;
            this.valorCronometro = new TimeSpan(0,0,timeToMove);
            this.cronometroFuncionando = false;
            this.botonStartActivo = true;
        }
        public Cronometro() {
                
        }

        public async Task StartGame(List<List<Casilla>> Casillas) {
            valorCronometro = new TimeSpan(0,0,timeToMove);
            botonStartActivo = false;


            Presentacion.UnlockBoard(Casillas);


            Presentacion.ActualizarSeleccion(Casillas);

            TimeLapse(Casillas);


        }

        public async Task NewTourn( List<List<Casilla>> Casillas ) {
            Presentacion.ActualizarSeleccion(Casillas);
            cronometroFuncionando = false;
            this.valorCronometro = new TimeSpan(0,0,timeToMove);
            botonStartActivo = true;
            this.tokenSource2.Cancel();
            this.tokenSource2 = new CancellationTokenSource();
            await TimeLapse(Casillas);
        }

        public async Task  Pause( List<List<Casilla>> Casillas ) {
            this.cronometroFuncionando = !cronometroFuncionando;
            await TimeLapse(Casillas);
        }

        public async Task TimeLapse( List<List<Casilla>> Casillas ) {

            var task = Task.Run(async () => {
                cronometroFuncionando = true;
                while( cronometroFuncionando ) {

                    if( cronometroFuncionando && valorCronometro.TotalSeconds > 0 ) {
                        await Task.Delay(1000);
                        valorCronometro = valorCronometro.Subtract(new TimeSpan(0,0,0,0,500));
                    }
                    else {
                        Presentacion.ActualizarSeleccion(Casillas);
                        valorCronometro = new TimeSpan(0,0,timeToMove);
                    }

                    //if( this.logicaBuscaminas.Victory() ) {
                    //    this.Casillas.ForEach(x => x.ForEach(y => y.pulsado = true));

                    //    Presentacion.EstadoOriginalTablero(Casillas);
                    //    break;

                    //}
                }
            },this.ct);
        }

    }
}
