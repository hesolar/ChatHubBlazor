using BlazorApp.Client.Model.minesweeper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.Client.Model.RoomRev.minesweeperLogic {
    public class MinesweeperLogic {
        
        private readonly List<GameBoxButton> LittleBoxesOnTheHilltop;
        private int bombs;
        public int BombsRemaining { get; set; }
        public int FlagsRemaining { get; set; }
        public bool PlayerAlive { get; set; } = true;



        public MinesweeperLogic( int rows ) {
            Random s_Generator = new Random();
            this.LittleBoxesOnTheHilltop = new List<GameBoxButton>();
            bombs = 0;
            
            string[] probabilityOfBomb = { "false", "false","false","false","false", "true" };

            for( int row = 0; row < rows; row++ ) {
                for( int col = 0; col < rows; col++ ) {
                    //cambiar ancho?
                    bool b = bool.Parse(probabilityOfBomb[s_Generator.Next(0,6)]);
                    if(b)  bombs += 1; 
                    LittleBoxesOnTheHilltop.Add(new GameBoxButton(row,col,b));;
                   
                }
            }
            LoadBombsNeighbors();

            BombsRemaining = bombs;
            FlagsRemaining = bombs;


        }
        public bool Victory() {
            return this.BombsRemaining.Equals(0);
        }

        public bool MakeMove( int cx,int cy ) {
            GameBoxButton gb = this.LittleBoxesOnTheHilltop.Where(x => x.X.Equals(cx) && x.Y.Equals(cy)).First();
            this.PlayerAlive = gb.RevealT_ExplodedF();
            if( PlayerAlive ) {
                if( gb.bombsNeighbor.Equals(0) ) {
                    RevealZeroSquaresNeighbors(cx,cy);
                }
                
            }
            else LostGame();


            return this.PlayerAlive;
            

        }
        private void RevealZeroSquaresNeighbors( int cx,int cy ) {
            //saco adyacentes con 0 bombas y los muestro
            var l=LittleBoxesOnTheHilltop.Where(panel => panel.X == (cx - 1) && panel.X <= (cx + 1)
                                                && panel.Y >= (cy - 1) && panel.Y <= (cy + 1)).ToList();
            l.ForEach(x => MakeMove(x.X,x.Y));

        }
        private List<GameBoxButton> LostGame() {
            return LittleBoxesOnTheHilltop.Where(x => x.IsBomb).ToList();
        }

        //true if can flag a box, false no flags left
        public bool Flag( int cx,int cy ) {
            GameBoxButton gb = this.LittleBoxesOnTheHilltop.Where(x => x.X.Equals(cx) && x.Y.Equals(cy)).Single();

            bool b = gb.Flag();

            //¿can flaged? then flag
            if( b ) {
                if( gb.IsBomb ) BombsRemaining--;
                FlagsRemaining--;
                return true;
            }
            //¿cant? then unflag
            else {
                FlagsRemaining++;
                if( gb.IsBomb ) BombsRemaining++;
                return false;

            }                    


           
        }

        private void LoadBombsNeighbors( ) {
            Func<int,int,int> neighborBombs = ( cx,cy ) => {

                var lista = LittleBoxesOnTheHilltop.Where(t => t.IsBomb && (t.X + 1 == cx || t.X - 1 == cx || t.X == cx))
                   .Intersect(LittleBoxesOnTheHilltop.Where(t => t.IsBomb && (t.Y + 1 == cy || t.Y - 1 == cy || t.Y == cy))).ToList();
                lista.Remove(LittleBoxesOnTheHilltop.Where(z => z.X == cx && z.Y == cy).First());

                return lista.Count(); ;
            };
            this.LittleBoxesOnTheHilltop.ForEach(x => { x.bombsNeighbor = neighborBombs(x.X,x.Y); });        
        }
       

        public bool IsBomb(int x,int y) {
            return this.LittleBoxesOnTheHilltop.Where(z => z.X.Equals(x) && z.Y.Equals(y)).First().IsBomb;
        }

        public bool IsButtonFlagged(int x, int y ) {
            return this.LittleBoxesOnTheHilltop.Where(z => z.X == x && z.Y == y).First().IsFlagged();
        }

        public int neighborBombs(int cx,int cy ) {
            return this.LittleBoxesOnTheHilltop.Where(x => x.X == cx && x.Y == cy).First().bombsNeighbor;
        }
    }
}
