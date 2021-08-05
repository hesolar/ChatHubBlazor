using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Data.Model
{
    public class Jugador
    {
        public Jugador(String name)
        {
            this.name = name;
            puntuacion = 0;
        }
        public string name;
        public long puntuacion { get; set; }

        public void ChangePuntuacion(long puntos)
        {
            puntuacion += puntos;
        }
        public override string ToString()
        {
            return this.name;
        }
    }
}
