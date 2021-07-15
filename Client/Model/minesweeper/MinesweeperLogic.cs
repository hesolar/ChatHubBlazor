using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.Client.Model.minesweeper {
    public class MinesweeperLogic {
        int rows;
        List<GameBoxButton> littleBoxesOnTheHillTop;
        int mines, minesRemaining;
        int flagsRemaining;
        public bool Alive { get; set; } = true;
        const double margin = 60.0 / 100.0;

        

        public MinesweeperLogic(int rows) {
            this.littleBoxesOnTheHillTop = new List<GameBoxButton>();
            this.rows = rows;
            mines = 0;

            Random s_Generator = new Random();
            for(int row=0; row<rows;row++ ) {
                for( int col = 0; col < row; col++ ) {
                    //cambiar ancho?
                    littleBoxesOnTheHillTop.Add(new GameBoxButton(16,row,col,s_Generator.NextDouble() <= margin));
                    mines += 1;
                }
            }
            flagsRemaining = mines;
            mines = 0;


        }
        public bool Victory() {
            return this.minesRemaining == 0;
        }

        public bool MakeMove(int cx, int cy) {
           GameBoxButton gb= this.littleBoxesOnTheHillTop.Where(x => x.x.Equals(cx) && x.y.Equals(cy)).Single();
           this.Alive= gb.RevealT_ExplodedF();
          
           if( Alive ) RevealZeroSquaresNeighbors(cx,cy);
           else LostGame();

           return this.Alive;
            

        }
        private void RevealZeroSquaresNeighbors(int cx,int cy) {
            //saco adyacentes con 0 bombas y los muestro
            littleBoxesOnTheHillTop.Where(panel => panel.x == (cx - 1) && panel.x <= (cx + 1)
                                                && panel.y >= (cy - 1) && panel.y <= (cy + 1) && !panel.isRevealed()).ToList().ForEach(z=> z.RevealT_ExplodedF());

        }
        private List<GameBoxButton> LostGame() {
          return  littleBoxesOnTheHillTop.Where(x => x.isBomb).ToList();
        }

        //true if can flag a box, false no flags left
        public bool Flag( int cx,int cy ) {

            if( flagsRemaining == 0 ) return false;
            
            flagsRemaining--;
            GameBoxButton gb = this.littleBoxesOnTheHillTop.Where(x => x.x.Equals(cx) && x.y.Equals(cy)).Single();
            gb.Flag();
            if( gb.isBomb )minesRemaining--;
            
            return true;
        }

        public int BombsNeighbors(int cx,int cy ) {
            return littleBoxesOnTheHillTop.Where(panel => panel.x == (cx - 1) && panel.x <= (cx + 1)
                                                && panel.y >= (cy - 1) && panel.y <= (cy + 1)).Where(z => z.isBomb).ToList().Count;          
        }


    }
}
