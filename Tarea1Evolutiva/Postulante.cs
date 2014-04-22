using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tarea1Evolutiva
{
    public class Postulante
    {
        public string nombre;
        public int ingreso_familiar;
        public double promedio;
        public int valor_carrera;

        public Postulante(string nombre, int ingreso_familiar, double promedio, int valor_carrera)
        {
            this.nombre = nombre;
            this.ingreso_familiar = ingreso_familiar;
            this.promedio = promedio;
            this.valor_carrera = valor_carrera;
        }

    }
}