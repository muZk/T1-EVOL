using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tarea1Evolutiva
{
    public static class Fitness
    {
        /// <summary>
        /// Función de evaluación para un individuo de la población
        /// </summary>
        /// <param name="chromosome">Cromosoma a evluar</param>
        /// <param name="postulantes">Datos de los postulantes</param>
        /// <returns>Valor de fitness para este cromosoma</returns>
        public static double Evaluate(Chromosome chromosome, List<Postulante> postulantes)
        {
            // Enunciado: "Buscando el mayor beneficio al máximo de alumnos"
            double benefit = 0;
            for (int i = 0; i < chromosome.Length; i++)
                benefit += postulantes[i].valor_carrera * chromosome.GetMultiplier(i);
            chromosome.fitness = benefit;
            return benefit;
        }

        /// <summary>
        /// Pondera el fitness de un cromosoma dependiende del número de restriccíones
        /// que no cumpla
        /// </summary>
        /// <param name="chromosome">Cromosoma a evaluar</param>
        /// <param name="data">Datos de los postulantes</param>
        public static void Penalice(Chromosome chromosome, List<Postulante> data)
        {
            if (chromosome.ExceedsMaxBudget())
            {
                // Reparar
                chromosome.Repare(data);
            }
            else
            {
                int notBroken = chromosome.Length - chromosome.BrokenRestrictions(data);
                chromosome.fitness *= notBroken / chromosome.Length;
            }
        }


    }
}
