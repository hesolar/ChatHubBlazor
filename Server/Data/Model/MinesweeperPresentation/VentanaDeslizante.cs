
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Data.Model.MinesweeperPresentation {

    public class VentanaDeslizante {
    public int xa { get; private set; }
    public int ya { get; private set; }
    public int yb { get; private set; }
    public int xb { get; private set; }


    int sizeRectangle;
    int MilisecondstimeAvaliable;
    int decrementTime;
    int rows;


    public bool WasSuccesfull { get; private set; }
    private CancellationTokenSource Canceller { get; set; }
    private Task<bool> Worker { get; set; }




    public VentanaDeslizante( int MilisecondstimeAvaliable,int sizeRectangle ,int rows,int decrementTime) {
        
        this.MilisecondstimeAvaliable = MilisecondstimeAvaliable;
        this.decrementTime = decrementTime;
        this.rows = rows;
        this.sizeRectangle = sizeRectangle;
    }



    

    public void AbortMovement() {
        Canceller.Cancel();
    }


    


        public void GenerateNewSquare(List<Casilla> l) {
            Random r = new Random();
            Casilla c =  l[r.Next(0,l.Count-1)];
            
            
            int maximo = this.rows - this.sizeRectangle + 1;
            this.xa =c.X;
            this.ya = c.Y;
            this.xb = xa + this.sizeRectangle >= maximo ? xa - this.sizeRectangle : xa + this.sizeRectangle;
            this.yb = ya + this.sizeRectangle >= maximo ? ya - this.sizeRectangle : ya + this.sizeRectangle;



        }

        public List<int> getBorders() {
            int aux;
            if( xa > xb ) {
                aux = xa;
                xa = xb;
                xb = aux;
            }
            if( ya > yb ) {
                aux = ya;
                ya = yb;
                yb = aux;
            }



            return new List<int> { this.xa,this.xb,this.ya,this.yb };
        }
        
    }

   
}
