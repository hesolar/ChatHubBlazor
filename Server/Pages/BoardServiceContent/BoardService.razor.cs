using Microsoft.AspNetCore.Components;
using Server.Data.Model;
using Server.Data.Model.MinesweeperPresentation;
using Server.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Pages.BoardServiceContent {
    public class BoardService : ComponentBase {

        [Parameter] public Partida TableroActual { get; set; }
        [Parameter] public String roomId { set; get; }
        [Parameter] public String username { get; set; }

        static public Dictionary<string,List<Jugador>> jugadoresActulizacion;
        static public Dictionary<string,TimeSpan> Crono;

        public bool registroCorrecto = false;

        protected override void OnInitialized() {
            if( jugadoresActulizacion == null ) jugadoresActulizacion = new();
            if( Crono == null ) Crono = new();
            registroCorrecto = TableroActual.AddPlayer(username);


            var xd = TableroActual.Players.ToList();
            jugadoresActulizacion.Add(username,xd);
            var yd = TableroActual.ValorCronometro;
            Crono.Add(username,yd);

            InvokeAsync(ActualizarParametroJugadores);

        }

        //Jugadores que no son el main
        public async Task ActualizarParametroJugadores() {
            while( !TableroActual.PartidaComenzada ) {
                await Task.Delay(200);
                if( jugadoresActulizacion[username].Count != TableroActual.Players.Count ) {
                    jugadoresActulizacion[username] = TableroActual.Players;
                    await InvokeAsync(StateHasChanged);
                }
            }
            jugadoresActulizacion = new();
            while( !TableroActual.Logica.Victory() ) {
                await Task.Delay(500);
                if( Crono[username] != TableroActual.ValorCronometro ) {
                    Crono[username] = TableroActual.ValorCronometro;
                    StateHasChanged();
                    await InvokeAsync(StateHasChanged);

                }
            }

        }


        //Jugador Host
        public async Task ComenzarPartida() {
            TableroActual.PartidaComenzada = true;
            Presentacion.UnlockBoard(TableroActual.Casillas);
            InvokeAsync(Comenzar);
        }

        public async Task Comenzar() {

            while( !TableroActual.Logica.Victory() ) {

                Presentacion.ActualizarVentanaDeslizante(TableroActual.Casillas);
                StateHasChanged();
                TableroActual.cronometroFuncionando = true;
                await TimeLapse();
                Presentacion.EstadoOriginalTablero(TableroActual.Casillas);
                StateHasChanged();
                TableroActual.nextTourn();
            }

        }

        public async Task TimeLapse() {
            while( TableroActual.cronometroFuncionando && !TableroActual.Logica.Victory() && !Presentacion.NingunaSeleccionada(TableroActual.Casillas) ) {
                await Task.Delay(1);
                if( TableroActual.ValorCronometro.TotalSeconds > 0 ) {
                    await Task.Delay(100);
                    TableroActual.ValorCronometro = TableroActual.ValorCronometro.Subtract(new TimeSpan(0,0,0,0,100));
                    InvokeAsync(StateHasChanged);

                }
                else {
                    //valorCronometro = new TimeSpan(0,0,(int) (Math.Round(timeToMove)));
                    TableroActual.cronometroFuncionando = false;

                    TableroActual.CurrentPlayerTourn().puntuacion += -5;
                    InvokeAsync(StateHasChanged);

                }

            }
            TableroActual.Casillas.ForEach(x => x.ForEach(x => x.EstadoOriginal()));

            int miliseconds = Convert.ToInt32(1000 * (((double) TableroActual.casillasAbiertas / (double) (TableroActual.Logica.rows * TableroActual.Logica.rows)) * TableroActual.timeToMove));

            TableroActual.ValorCronometro = new TimeSpan(0,0,0,0,miliseconds);
            InvokeAsync(StateHasChanged);

        }

        //Refrescar Pagina
        public void Refresh() {
            InvokeAsync(StateHasChanged);
        }

    

}
}
