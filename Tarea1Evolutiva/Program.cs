using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tarea1Evolutiva
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Ingrese archivo de configuración (string vacio para opciones por defecto):");
            Config.readConfig(Console.ReadLine());

            Config.poblacion = 10;
            Console.WriteLine("Configuración:");
            Config.printConfig();

            GeneticAlgorithm.Run();

            Console.WriteLine();
            Console.WriteLine("[ENTER] para finalizar");
            Console.ReadKey();
        }
    }
}