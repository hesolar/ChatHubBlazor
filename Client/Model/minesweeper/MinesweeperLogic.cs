using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.Client.Model.minesweeper {
    public class MinesweeperLogic {
        int rows;
        List<GameBoxButton> LittleBoxesOnTheHilltop;
        public int bombs;
        public int bombsRemaining { get; set; }
        public int flagsRemaining { get; set; }
        public bool Alive { get; set; } = true;
        const double margin = 60.0 / 100.0;



        public MinesweeperLogic( int rows ) {
            Random s_Generator = new Random();
            this.LittleBoxesOnTheHilltop = new List<GameBoxButton>();
            this.rows = rows;
            bombs = 0;
            
                                string[] trueOrFalse = { "false","false","false", "true" };

            for( int row = 0; row < rows; row++ ) {
                for( int col = 0; col < rows; col++ ) {
                    //cambiar ancho?
                   int i= s_Generator.Next(0,1);
                    bool b = bool.Parse(trueOrFalse[s_Generator.Next(0,4)]);
                    if( b == true )  bombs += 1; 
                    LittleBoxesOnTheHilltop.Add(new GameBoxButton(16,row,col,b));;
                   
                }
            }
            bombsRemaining = bombs;
            flagsRemaining = bombs;


        }
        public bool Victory() {
            return this.bombsRemaining == 0;
        }

        public bool MakeMove( int cx,int cy ) {
            GameBoxButton gb = this.LittleBoxesOnTheHilltop.Where(x => x.x.Equals(cx) && x.y.Equals(cy)).First();
            this.Alive = gb.RevealT_ExplodedF();
            if( Alive ) RevealZeroSquaresNeighbors(cx,cy);
            else LostGame();
            return this.Alive;
            

        }
        private void RevealZeroSquaresNeighbors( int cx,int cy ) {
            //saco adyacentes con 0 bombas y los muestro
            LittleBoxesOnTheHilltop.Where(panel => panel.x == (cx - 1) && panel.x <= (cx + 1)
                                                && panel.y >= (cy - 1) && panel.y <= (cy + 1) && !panel.isRevealed()).ToList().ForEach(z => z.RevealT_ExplodedF());

        }
        private List<GameBoxButton> LostGame() {
            return LittleBoxesOnTheHilltop.Where(x => x.isBomb).ToList();
        }

        //true if can flag a box, false no flags left
        public bool Flag( int cx,int cy ) {

            

            
           
            GameBoxButton gb = this.LittleBoxesOnTheHilltop.Where(x => x.x.Equals(cx) && x.y.Equals(cy)).Single();

            bool b = gb.Flag();

            //¿not flaged?
            if( b ) {
                if( gb.isBomb ) bombsRemaining--;
                flagsRemaining--;
                return true;
            }
            //¿flaged? then unflag
            else {
                flagsRemaining++;
                if( gb.isBomb ) bombsRemaining++;
                return false;

            }                    


           
        }

        public int BombsNeighbors( int cx,int cy ) {
            var x = LittleBoxesOnTheHilltop.Where(t => t.isBomb && (t.x + 1 == cx || t.x - 1 == cx || t.x == cx)).Intersect(LittleBoxesOnTheHilltop.Where(t => t.isBomb && (t.y + 1 == cy || t.y - 1 == cy || t.y == cy))).ToList();
            x.Remove(LittleBoxesOnTheHilltop.Where(z => z.x == cx && z.y == cy).First());
            
            return x.Count();
        }
       

        public bool IsBomb(int x,int y) {
            return this.LittleBoxesOnTheHilltop.Where(z => z.x.Equals(x) && z.y.Equals(y)).First().isBomb;
        }

        public bool IsButtonFlagged(int x, int y ) {
            return this.LittleBoxesOnTheHilltop.Where(z => z.x == x && z.y == y).First().isFlagged();
        }
    }
}
