
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorApp.Client.Model.RoomRev.minesweeperLogic {

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




    public VentanaDeslizante( int MilisecondstimeAvaliable,int rows ,int sizeRectangle,int decrementTime) {
        
        this.MilisecondstimeAvaliable = MilisecondstimeAvaliable;
        this.decrementTime = decrementTime;
        this.rows = rows;
        this.sizeRectangle = 2;
    }



    public async Task<bool> StartTimeLapse(  ) {
        WasSuccesfull = true;
        Canceller = new CancellationTokenSource();

        Worker = Task<bool>.Factory.StartNew(() =>{
            try {
                // specify this thread's Abort() as the cancel delegate
                Task.Delay(this.MilisecondstimeAvaliable);
                return true;
            }
            catch( ThreadAbortException ) {
                WasSuccesfull = false;
                return false;
            }
        },Canceller.Token);
        return Worker.Result;
    }

    public void AbortMovement() {
        Canceller.Cancel();
    }


    private void GenerateNewSquare() {
        int xposible = this.rows - this.sizeRectangle;
        Random r = new Random();
        int x= r.Next(0,xposible);
        int y = r.Next(0,xposible);

        this.xa = x;
        this.ya = y;
        this.xb = xa+ this.sizeRectangle;
        this.yb = ya + this.sizeRectangle;

     }

     public void DoMove() {
        GenerateNewSquare();
        //MilisecondstimeAvaliable=this.MilisecondstimeAvaliable - this.decrementTime;
        //bool PlayerCanContinueMoving =await StartTimeLapse();
        //return PlayerCanContinueMoving;
    }

    public void getBorders(out int xa,out int xb,out int ya , out int yb ) {
        xa = this.xa;
        ya = this.ya;
       xb  = this.xb;
        yb = this.yb;
    }

   
}
}