using System;


namespace Server.Data.Model.MinesweeperLogic {
    enum ButtonCurrentState {
        Initial,
        Flag,
        Open,
        Exploded
    }
    public class GameBoxButton {
        public int X { get; }
        public int Y { get; }
        private ButtonCurrentState State { get; set; }
        public bool IsBomb { get; }
        public int bombsNeighbor { get; set;}

        public GameBoxButton( int x, int y, Boolean isBomb) {
            this.X = x;
            this.Y = y;
            this.IsBomb = isBomb;
            this.State = ButtonCurrentState.Initial;
            
        }
        public bool Flag() {

            if( this.State == ButtonCurrentState.Flag ) {
                this.State = ButtonCurrentState.Initial;
                return false; 
            }
            else {
                this.State = ButtonCurrentState.Flag;
                return true;
            }
            
        }
        public bool RevealT_ExplodedF() {
            this.State = this.IsBomb ? ButtonCurrentState.Exploded : ButtonCurrentState.Open;
            return !IsBomb;
        }

       public bool IsRevealed() {
            return this.State == ButtonCurrentState.Open; 
        }
        public bool IsFlagged() {
            return this.State == ButtonCurrentState.Flag;
        }

    }
}
