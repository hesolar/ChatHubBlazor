using Server.Data.Model.MinesweeperLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Data.Model.MinesweeperPresentation {
    //Ventana deslizante

    



    public class Presentacion {



       public static int rows { get; set; }
        public const int sizeWindow= 4;

        static public VentanaDeslizante ventanaDeslizante= new VentanaDeslizante(5000,sizeWindow,rows,10);

        static public List<Casilla> CasillasAdyacentes( List<List<Casilla>> Casillas,Casilla casilla ) {
            List<Casilla> vecinos = Casillas.SelectMany(z => z.Where(t => t.X + 1 == casilla.X || t.X - 1 == casilla.X || t.X == casilla.X)).Intersect(
            Casillas.SelectMany(z => z.Where(t => t.Y + 1 == casilla.Y || t.Y - 1 == casilla.Y || t.Y == casilla.Y))).ToList();
            vecinos.Remove(Casillas.SelectMany(z => z.Where(t => t.X == casilla.X && t.Y == casilla.Y)).First());
            return vecinos;
        }

        static public List<List<Casilla>> EstadoOriginalTablero( List<List<Casilla>> Casillas ) {
            Casillas.ForEach(x => x.ForEach(y => y.EstadoOriginal()));
            return Casillas;
        }
        public async static  Task EstadoOriginalTableroAsync( List<List<Casilla>> Casillas ) {
           await Task.Run(()=> Casillas.AsParallel().ToList().ForEach(x => x.ForEach(y => y.EstadoOriginal())));
        }


        static public void RevealZeroSquaresRecursion( List<List<Casilla>> Casillas,Casilla casilla,MinesweeperLogica logicaBuscaminas,int casillasAbiertas=0 ) {
            
            List<Casilla> minesNeighbors = Presentacion.CasillasAdyacentes(Casillas,casilla);

            //las no zero las despliego
            var notZeroMines = minesNeighbors.Where(x => !x.flag && logicaBuscaminas.neighborBombs(x.X,x.Y) != 0).ToList();
            notZeroMines.ForEach(z => z.MakeMove(logicaBuscaminas.neighborBombs(z.X,z.Y)));
            //las casillas con 0
            List<Casilla> ZeroMines = minesNeighbors.Where(z => !z.flag && logicaBuscaminas.neighborBombs(z.X,z.Y)==0 && !z.isZero).ToList();


            foreach( Casilla m in ZeroMines ) {
                m.Block();
                m.MakeMove(0);
                //List<Casilla> vecinos = Presentacion.CasillasAdyacentes(Casillas,casilla);
                
                //vecinos.AsParallel().ToList().Where(x => !x.flag && logicaBuscaminas.neighborBombs(x.X,x.Y) != 0).ToList().ForEach(x => x.MakeMove(logicaBuscaminas.neighborBombs(x.X,x.Y)));

                //List<Casilla> b = vecinos.Where(x => !x.flag && logicaBuscaminas.neighborBombs(x.X,x.Y) == 0 && !x.isZero).ToList();
                //b.AsParallel().ToList().ForEach(x => x.MakeMove(0));
                RevealZeroSquaresRecursion(Casillas,m,logicaBuscaminas);

               

            }

        }

        static public List<List<Casilla>> crearTablero( int rows,MinesweeperLogica logicaBuscaminas ) {

            Presentacion.rows = rows;
            ventanaDeslizante = new VentanaDeslizante(5000,sizeWindow,rows,10);
            //referencia
            var result = new List<List<Casilla>>();
            for( var x = 0; x < rows; x++ ) {
                var row = new List<Casilla>();
                for( var y = 0; y < rows; y++ ) {
                    row.Add(new Casilla(x,y,logicaBuscaminas.IsBomb(x,y)));
                }
                result.Add(row);
            }
            return result;

        }

      

        static public List<List<Casilla>> ActualizarVentanaDeslizante( List<List<Casilla>> Casillas ) {
            
            
            
            EstadoOriginalTablero(Casillas);
            List<Casilla> l = Casillas.SelectMany(x => x.Where(x => !x.isZero && !x.pulsado && !x.flag)).ToList();
            ventanaDeslizante.GenerateNewSquare(l);
         
            int xa, xb, ya, yb;
            List<int> listadoBordes = ventanaDeslizante.getBorders();

            xa = listadoBordes[0];
            xb = listadoBordes[1];
            ya = listadoBordes[2];
            yb = listadoBordes[3];


           


            List<Casilla> x = Casillas.SelectMany(x => x.Where(z => z.X >= xa && z.X <= xb)).ToList();
            List<Casilla> y = Casillas.SelectMany(x => x.Where(z => z.Y >= ya && z.Y <= yb)).ToList();
            List<Casilla> xy = x.Intersect(y).ToList();
            if(!ComprobarMovimientosEnCuadrado(Casillas,xy) ) {
                ActualizarVentanaDeslizante(Casillas);
                
            }

            xy.ForEach(x => x.Seleccionar());


            return Casillas;
        }

        private static bool ComprobarMovimientosEnCuadrado(List<List<Casilla>> Casillas, List<Casilla> ventana ) {
            if( ventana.All(x => x.pulsado || x.flag) ) return false;
            
            return true;
        }


        public static bool MovesLeftOnSelectedArea( List<List<Casilla>> casillas) {
            List<Casilla> currentWindow = casillas.SelectMany(x => x.Where(y => y.isSelected())).ToList();
            List<Casilla> casillasSinMovimiento = currentWindow.Where(x => x.pulsado || x.flag || x.isZero).ToList();

            return currentWindow.Count != casillasSinMovimiento.Count;
        }

        public static void UnlockBoard(List<List<Casilla>> casillas ) {
            casillas.AsParallel().ToList().ForEach(x => x.ForEach(x => x.isZero = false));
        }

        public static void LockBoard( List<List<Casilla>> casillas ) {
            casillas.AsParallel().ToList().ForEach(x => x.ForEach(x => x.seleccionadaCuadrado = Casilla.original));
        }

        public static int CasillasAbiertas(List<List<Casilla>> casillas ,int filas) {
            //double difficulty = (double) (rows * rows - dificultad) / (double) (rows * rows);
          
            
            return casillas.SelectMany(x => x.Where(x => !x.pulsado && !x.flag && !x.isZero)).ToList().Count();
            
        }

        public static bool NingunaSeleccionada( List<List<Casilla>> casillas ) {
            foreach(var rows in casillas){
                foreach(var cas in rows ) {
                    if( cas.isSelected() ) return false;
                }
            }
            return true;
        }

  
    }
}
