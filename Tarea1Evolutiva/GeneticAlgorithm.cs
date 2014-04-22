using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tarea1Evolutiva
{
    public static class GeneticAlgorithm
    {
        /// <summary>
        /// Esta clase se encarga de correr el algoritmo genérico.
        /// </summary>

        private const double PROMEDIO_MINIMO = 5;
        private const int INGRESO_MAXIMO = 1600000;
        private const int INGRESO_MEDIO = 1000000;
        private static Random random = new Random();
        
        private static List<Postulante> _postulantes = new List<Postulante>();
        private static List<Population> _generaciones = new List<Population>();
        private static Population _currentGeneration;

        private static Schema _schema;


        /// <summary>
        /// Lee el archivo de postulantes y los guarda en la lista postulantes
        /// </summary>
        private static void ReadFile()
        {
            System.IO.StreamReader file = new System.IO.StreamReader(@"postulantes.txt");
            string line;
            int count = 0;
            while((line = file.ReadLine()) != null)
            {
                string[] parameters = line.Split(';');
                string nombre = parameters[0];
                int ingreso_familiar = int.Parse(parameters[1]);
                double promedio = double.Parse(parameters[2]);
                int valor_carrera = int.Parse(parameters[3]);
                _postulantes.Add(new Postulante(nombre, ingreso_familiar, promedio, valor_carrera));
                count++;
            }
        }

        /// <summary>
        /// Inicializa el Schema que van a tener las soluciones, en base a las
        /// restricciones de Enunciado.
        /// </summary>
        private static void CreateInitialPopulation()
        {

            Console.WriteLine("Creando schema inicial");
            _schema = new Schema(_postulantes);

            Console.WriteLine("Creando primera población");
            _currentGeneration = new Population(_schema, _postulantes);
            _generaciones.Add(_currentGeneration);

        }

        /// <summary>
        /// Evalúa a la población
        /// </summary>
        private static void Evaluate()
        {
            Console.WriteLine("Evaluando individuos...");
            Population g = _currentGeneration;
            _currentGeneration.Evaluate();
            _currentGeneration.Penalice(); // recién aquí sabemos si había una solución que usa más recursos de los que debe
            _currentGeneration.SetBest();
            Console.WriteLine("Asignación: ${0:#,0}", _currentGeneration.best.fitness);
        }

        /// <summary>
        /// Genera la próxima generación y actualiza _currentGeneration
        /// </summary>
        private static void CreateNextGeneration()
        {

            Console.WriteLine("Creando siguiente generación...");

            // Selección por torneo para obtener a los padres

            Population nextGeneration = new Population(_postulantes);

            while (nextGeneration.Length < _currentGeneration.Length)
            {

                Console.WriteLine("  Realizando torneo...");

                // Para elegir padres, elegimos dos pares de cromosomas
                // y mediante torneo, elegimos al "mejor" de cada pareja
                // Nota: el 10% de las veces torneo me dará el peor, lo cual
                // busca incrementar la variabilidad

                int size = 2;
                List<Chromosome> subset1 = _currentGeneration.Subset(size); 
                List<Chromosome> subset2 = _currentGeneration.Subset(size);

                Chromosome parent1 = _currentGeneration.TournamentSelect(subset1[0], subset1[1]);
                Chromosome parent2 = _currentGeneration.TournamentSelect(subset2[0], subset2[1]);

                if (random.NextDouble() <= Config.crossover)
                {
                    Console.WriteLine("  Realizando crossover...");
                    Chromosome[] childs = Chromosome.Crossover(parent1, parent2);
                    nextGeneration.Add(childs[0]);
                    nextGeneration.Add(childs[1]);
                }
                else
                {
                    nextGeneration.Add(parent1);
                    nextGeneration.Add(parent2);
                }

            }

            // Penalizar, ya que crossover pudo haber dejado soluciones inválidas
            Console.WriteLine("Penalizando...");

            nextGeneration.Penalice();

            _generaciones.Add(nextGeneration);
            _currentGeneration = nextGeneration;
        }

        /// <summary>
        /// Ejecuta la mutación en la población actual
        /// </summary>
        private static void Mutation()
        {
            Console.WriteLine("Mutando...");
            _currentGeneration.Mutate();
        }

        /// <summary>
        /// Bucle principal del algoritmo genético
        /// </summary>
        private static void Repeat()
        {
            do
            {
                Console.WriteLine("\n");
                Console.WriteLine("### Generación {0}", _generaciones.Count);
                GeneticAlgorithm.Evaluate();
                GeneticAlgorithm.CreateNextGeneration();
                GeneticAlgorithm.Mutation();
            } while (_generaciones.Count <= Config.generaciones);
        }

        /// <summary>
        /// Inicia el algoritmo genérico
        /// </summary>
        public static void Run()
        {
            ReadFile();
            CreateInitialPopulation();
            Repeat();
            PrintSolution();
        }

        private static void PrintSolution()
        {

            Console.WriteLine("\nASIGNACÍON OPTIMA");

            Chromosome best = _generaciones[0].best;
            foreach (Population g in _generaciones)
                if(g != _generaciones.Last())
                    if (g.best.fitness > best.fitness)
                        best = g.best;

            best.Print(_postulantes);
        }
    }
}
