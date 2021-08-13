using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Server.Data.Model.MinesweeperPresentation;
using Server.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Pages.BoardServiceContent {
    public class Tablerocs : ComponentBase {
        [Parameter] public String username { get; set; }
        [Parameter] public Partida TableroActual { get; set; }
        [Parameter] public String roomId { get; set; }


        public bool aciertoPuntuacion = true;
        public async Task LeftClick( MouseEventArgs e,Casilla casilla ) {
           
            
            //left click
            if( e.Detail == 1 ) {
                if( !casilla.pulsado ) {
                    if( !TableroActual.Logica.IsButtonFlagged(casilla.X,casilla.Y) ) {
                        Presentacion.EstadoOriginalTablero(TableroActual.Casillas);
                        if( !casilla.bomb ) {
                            TableroActual.Logica.MakeMove(casilla.X,casilla.Y);




                            int bombs = TableroActual.Logica.neighborBombs(casilla.X,casilla.Y);

                            casilla.MakeMove(bombs);

                            if( bombs == 0 ) {
                                Presentacion.RevealZeroSquaresRecursion(TableroActual.Casillas,casilla,TableroActual.Logica);
                                TableroActual.casillasAbiertas = Presentacion.CasillasAbiertas(this.TableroActual.Casillas,TableroActual.Casillas.Count);
                            }
                            else TableroActual.casillasAbiertas--;
                            var pt = (float) TableroActual.ValorCronometro.TotalMilliseconds/(float)1000;
                         
                             ActualizarPuntuacion(pt);

                        }
                        else ActualizarPuntuacion(-5);

                        return;
                    }


                }
                else MouseWheel(casilla);

            }



        }
        public void RightClick( Casilla casilla ) {
            //right click
            if( casilla.isSelected() && TableroActual.CurrentPlayerTourn().username == username ) {
                if( !casilla.pulsado && !casilla.isZero && !casilla.flag ) {

                    if( casilla.bomb ) {
                        TableroActual.Logica.Flag(casilla.X,casilla.Y);
                        casilla.Flag();
                        casilla.Block();
                        var pt = (float) TableroActual.ValorCronometro.TotalMilliseconds / (float) 500;

                        ActualizarPuntuacion(pt);
                        TableroActual.casillasAbiertas--;
                    }
                    else ActualizarPuntuacion(-5);
                    Presentacion.EstadoOriginalTablero(TableroActual.Casillas);
                }
            }
        }
        public async Task MouseWheel( Casilla mine ) {
            if( !mine.pulsado ) return;


            List<Casilla> flag = Presentacion.CasillasAdyacentes(TableroActual.Casillas,mine).Where(x => x.flag).ToList();
            List<Casilla> notFlagAndNotOpen = Presentacion.CasillasAdyacentes(TableroActual.Casillas,mine).Where(x => !x.pulsado && !x.flag).ToList();
            int bombsNeighbor = TableroActual.Logica.neighborBombs(mine.X,mine.Y);
            if( flag.Count.Equals(bombsNeighbor) ) {
                Presentacion.EstadoOriginalTablero(TableroActual.Casillas);
                notFlagAndNotOpen.ForEach(x => {


                    bombsNeighbor = TableroActual.Logica.neighborBombs(x.X,x.Y);
                    if( bombsNeighbor == 0 ) {
                        x.MakeMove(0);
                        TableroActual.casillasAbiertas = Presentacion.CasillasAbiertas(this.TableroActual.Casillas,TableroActual.Casillas.Count);
                    }
                    else {
                        if( TableroActual.Logica.MakeMove(x.X,x.Y) ) x.MakeMove(bombsNeighbor);
                    }
                });
                mine.Block();
                var pt = (float) TableroActual.ValorCronometro.TotalMilliseconds / (float) 1000;
                ActualizarPuntuacion(pt);
            }
        }

        public void ActualizarPuntuacion( float puntos ) {
            aciertoPuntuacion = puntos > 0;
            TableroActual.CurrentPlayerTourn().puntuacion += puntos;
        }

     

        [Parameter]
        public EventCallback<MouseEventArgs> ComenzarPartida { get; set; }

    }
}
