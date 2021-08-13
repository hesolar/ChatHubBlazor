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
            this.username = name;
            puntuacion = 0;
        }
        public string username;
        public float puntuacion { get; set; }

        public void ChangePuntuacion(long puntos)
        {
            puntuacion += puntos;
        }
        public override string ToString()
        {
            return this.username;
        }
    }
}
