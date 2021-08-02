using BlazorStrap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.Server.Data.Model.minesweeperPresentation {
    public class Mine {


        public bool isZero;
        public Boolean pulsado { get; set; }
        public String text;
        public int X { get; set; }
        public int Y { get; set; }
        public Color ColorEstado { get; set; }
        public bool bomb;
        public bool flag = false;

        //ventana deslizante
        public string seleccionadaCuadrado { set; get; }

        public Mine( int x,int y,bool bomb ) {
            this.X = x;
            this.Y = y;
            //await Task.FromResult(

            ColorEstado = Color.None;
            this.pulsado = false;
            this.bomb = bomb;
            this.text = "";
            this.isZero = false;

            this.seleccionadaCuadrado = "purple";
        }
        public void MakeMove( int bombsNeighbor ) {

            ColorEstado = Color.Success;
            pulsado = true;
            this.text = bombsNeighbor.ToString();
            if( bombsNeighbor.Equals(0) ) this.isZero = true;

        }
        public void Flag() {
            //game.Flag(X,Y);
            flag = !flag;
            ColorEstado = this.flag ? Color.Primary : Color.None;


        }
    }
}
