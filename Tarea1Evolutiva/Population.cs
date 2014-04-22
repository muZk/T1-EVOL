using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tarea1Evolutiva
{
    public class Population
    {

        private Schema _schema;
        private List<Chromosome> _population = new List<Chromosome>();
        private List<Postulante> _data;
        private static Random random = new Random();
        public Chromosome best = null;

        public Population(List<Postulante> data)
        {
            _data = data;
        }

        public Population(Population p, List<Postulante> postulantes)
        {
            // TODO
            _data = postulantes;
        }

        public Population(Schema schema, List<Postulante> data)
        {
            _schema = schema;
            _data = data;
            // Generate initial population
            for (int i = 0; i < Config.poblacion; i++)
                _population.Add(new Chromosome(_schema));
        }

        /// <summary>
        /// Calcula el fitness de todos los individuos de la población
        /// </summary>
        public void Evaluate()
        {
            foreach (Chromosome c in _population)
            {
                c.fitness = Fitness.Evaluate(c, _data);
            }
        }

        public void SetBest()
        {
            this.best = _population[0];
            foreach (Chromosome c in _population)
                if (c.fitness > best.fitness)
                    best = c;
        }

        /// <summary>
        /// Evalúa mutar cada individuo de la población de acuerdo
        /// a la probabilidad definido por Config.mutacion
        /// </summary>

        public void Mutate()
        {
            foreach (Chromosome c in _population)
                if(random.NextDouble() < Config.mutacion)
                    c.Mutate();
        }

        public void Penalice()
        {
            foreach (Chromosome c in _population)
                Fitness.Penalice(c, _data);
        }

        public void Print()
        {
            for (int i = 0; i < _population.Count; i++)
                Console.WriteLine(_population[i].ToString());
        }

        /// <summary>
        /// Helper para acceder a cada individuo de la población
        /// de forma más amigable
        /// </summary>
        /// <param name="key">índice del individuo</param>
        /// <returns>individuo en la posición key</returns>

        public Chromosome this[int index]
        {
            get
            {
                return _population[index];
            }
            set
            {
                _population[index] = value;
            }
        }

        /// <summary>
        /// Agrega un individuo a la población.
        /// </summary>
        /// <param name="c">Cromosoma a agregar</param>

        public void Add(Chromosome c)
        { 
            _population.Add(c);
        }

        /// <summary>
        /// Selecciona un cromosoma random de la población
        /// </summary>
        /// <returns></returns>

        public Chromosome Random()
        { 
            return _population[random.Next(0, Length)];
        }

        /// <summary>
        /// Obtiene un subconjunto de tamaño size de la población
        /// </summary>
        /// <param name="size">Tamaño del subconjunto</param>
        /// <returns>Subconjunto de tamaño size</returns>

        public List<Chromosome> Subset(int size)
        {
            List<Chromosome> set = new List<Chromosome>(size);

            while (set.Count < size)
                set.Add(Random());

            return set;
        }

        /// <summary>
        /// Realiza selección por torneo del conjunto de cromosomas en set
        /// </summary>
        /// <param name="set">Conujunto de cromosomas para realizar torneo</param>
        /// <returns>El cromosoma de mejor fitness</returns>
        public static Chromosome TournamentSelect(List<Chromosome> set)
        {

            Chromosome best = set.First();

            foreach (Chromosome c in set)
                if (c.fitness > best.fitness)
                    best = c;

            return best;
        }

        /// <summary>
        /// Esta implementación de torneo es la que se está usando en la tarea.
        /// El 10% de las veces retorna el peor de los competidores, y el 90%
        /// de las veces retorna el mejor
        /// </summary>
        /// <param name="p1">Cromosoma competidor</param>
        /// <param name="p2">Cromosoma competidor</param>
        /// <returns></returns>
        public Chromosome TournamentSelect(Chromosome p1, Chromosome p2)
        {
            if (p1.fitness == p2.fitness)
            {

                if (p1 == p2)
                    return p1;

                // Las dos tienen el mismo fitness
                // hay que elegir la que este mejor asignada
                
                // x1 y x2 son puntajes que obtiene cada uno
                // en comparación con el otro
                int x1 = 0;
                int x2 = 0;

                for (int i = 0; i < p1.Length; i++)
                {
                    if (p1[i] != Gen.NOTHING)
                    {
                        double prom1 = _data[i].promedio;
                        double ingr1 = _data[i].ingreso_familiar;

                        for (int j = 0; j < p2.Length; j++)
                        {
                            double prom2 = _data[j].promedio;
                            double ingr2 = _data[j].ingreso_familiar;

                            // La idea es que manteniendo constante
                            // la nota, se privilegie al que tiene menor ingreso
                            // Y manteniendo constante el ingreso
                            // se privilegie al que tiene mayor nota

                            double f1 = prom1 / ingr1;
                            double f2 = prom2 / ingr2;

                            if (f1 < f2)
                                x2++;
                            else if(f2 > f1)
                                x1++;
                        }

                    }
                }

                return x1 > x2 ? p1 : p2;
            }
            else
            { 
                // Hay un 10% de probabilidad de quedarse con el peor
                // esto le quita un poco de elitismo y me permite explorar más
                if (random.NextDouble() < 0.1)
                    return p1.fitness > p2.fitness ? p2 : p1;
                else
                    return p1.fitness > p2.fitness ? p1 : p2;
            }
        }


        public int Length { get { return _population.Count; } }

    }
}
