using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server {
    public class Player {
        public String idPlayer { get; }
        public long puntuacion { get; set; }
        public Player(String idPlayer) {
            this.idPlayer = idPlayer;
            puntuacion = 0;
        }
    }
}
