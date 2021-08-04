using System;


namespace BlazorApp.Server.Data.Model.MinesweeperLogic {
   
    public class GameBoxButton {

        enum ButtonCurrentState {
            Initial,
            Flag,
            Open,
            Exploded
        }
        public GameBoxButton() {
            this.XId = -1;
            this.YId = -1;
            this.State = ButtonCurrentState.Initial;
            bombsNeighbor = 0;
            IsBomb = false;
        }

        public int XId { get; }
        public int YId { get; }
        private ButtonCurrentState State { get; set; }
        public bool IsBomb { get; }
        public int bombsNeighbor { get; set;
        }

        public GameBoxButton( int x, int y, Boolean isBomb) {
            this.XId = x;
            this.YId = y;
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
