using BlazorApp.Client.Model.RoomRev.minesweeperLogic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorApp.Client.Model.RoomRev.minesweeperPresentation {
    //Ventana deslizante




    public static class ActionsOnBoard {

    static public int rows { get; set; }
    static public VentanaDeslizante ventanaDeslizante = new VentanaDeslizante(5000,5,rows,10);

    static public List<Casilla> CasillasAdyacentes( List<List<Casilla>> Casillas,Casilla casilla ) {
            List<Casilla> vecinos = Casillas.SelectMany(z => z.Where(t => t.X + 1 == casilla.X || t.X - 1 == casilla.X || t.X == casilla.X)).Intersect(
            Casillas.SelectMany(z => z.Where(t => t.Y + 1 == casilla.Y || t.Y - 1 == casilla.Y || t.Y == casilla.Y))).ToList();
            vecinos.Remove(Casillas.SelectMany(z => z.Where(t => t.X == casilla.X && t.Y == casilla.Y)).First());
            return vecinos;
    }

    static public List<List<Casilla>>  EstadoOriginalTablero( List<List<Casilla>> Casillas ) {
            Casillas.ForEach(x => x.ForEach(y => y.EstadoOriginal()));
            return Casillas;
    }

    static public List<List<Casilla>> RevealZeroSquaresRecursion( List<List<Casilla>> Casillas,Casilla casilla ,MinesweeperLogic logicaBuscaminas) {

            List<Casilla> minesNeighbors = ActionsOnBoard.CasillasAdyacentes(Casillas,casilla);

            //las no zero las despliego
            var notZeroMines = minesNeighbors.Where(x => !x.flag && logicaBuscaminas.neighborBombs(x.X,x.Y) != 0).ToList();
            notZeroMines.ForEach(z => z.MakeMove(logicaBuscaminas.neighborBombs(z.X,z.Y)));

            //las casillas con 0
            var ZeroMines = minesNeighbors.Where(z => !z.flag && logicaBuscaminas.neighborBombs(z.X,z.Y).Equals(0)).ToList();

            foreach( Casilla m in ZeroMines ) {

                m.MakeMove(0);
                List<Casilla> a = ActionsOnBoard.CasillasAdyacentes(Casillas,casilla);
                a.AsParallel().ToList().Where(x => !x.flag && logicaBuscaminas.neighborBombs(x.X,x.Y) != 0).ToList().ForEach(x => x.MakeMove(logicaBuscaminas.neighborBombs(x.X,x.Y)));

                List<Casilla> b = a.Where(x => !x.flag && logicaBuscaminas.neighborBombs(x.X,x.Y) == 0 && !x.pulsado).ToList();
                b.AsParallel().ToList().ForEach(x => x.MakeMove(0));
                b.AsParallel().ToList().ForEach(x => RevealZeroSquaresRecursion(Casillas,x,logicaBuscaminas));



            }
            return Casillas;
        }

    static  public List<List<Casilla>> crearTablero(int rows,MinesweeperLogic logicaBuscaminas) {


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

    static public void ActualizarSeleccion(List<List<Casilla>> Casillas) {

            ventanaDeslizante.DoMove();
            int xa, xb, ya, yb;
            ventanaDeslizante.getBorders(out xa,out xb,out ya,out yb);


            List<Casilla> x = Casillas.SelectMany(x => x.Where(z => z.X > xa && z.X <= xb)).ToList();
            List<Casilla> y = Casillas.SelectMany(x => x.Where(z => z.Y > ya && z.Y <= yb)).ToList();
            List<Casilla> xy = x.Intersect(y).ToList();
            xy.ForEach(x => x.Seleccionar());



        }
    }
}
