using System;


namespace BlazorApp.Client.Model.minesweeper {
    enum ButtonCurrentState {
        Initial,
        Flag,
        Open,
        Exploded
    }
    public class GameBoxButton {
        private int width;
        public int x { get; }
        public int y { get; }
        private ButtonCurrentState state { get; set; }
        public bool isBomb { get; }
       

        public GameBoxButton( int? width,int x, int y, Boolean isBomb) {
            this.x = x;
            this.y = y;
            this.isBomb = isBomb;
            this.state = ButtonCurrentState.Initial;
            if(width == null) this.width=16; //parece q mas o menos esto estara bn d ancho
        }
        public bool Flag() {

            if( this.state == ButtonCurrentState.Flag ) {
                this.state = ButtonCurrentState.Initial;
                return false;
            }
            else {
                this.state = ButtonCurrentState.Flag;
                return true;
            }
            
        }
        public bool RevealT_ExplodedF() {
            this.state = this.isBomb ? ButtonCurrentState.Exploded : ButtonCurrentState.Open;
            return !isBomb;
        }

       public bool isRevealed() {
            return this.state == ButtonCurrentState.Open; 
        }
        public bool isFlagged() {
            return this.state == ButtonCurrentState.Flag;
        }

    }
}
