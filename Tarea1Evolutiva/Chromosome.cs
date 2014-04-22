using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tarea1Evolutiva
{

    public enum Gen
    {
        NOTHING = 0,
        HALF = 1,
        FULL = 2
    }

    public class Chromosome
    {
        private static Gen[] HALF_OR_FULL = new Gen[] { Gen.HALF, Gen.FULL };
        private static Gen[] HALF_OR_NOTHING = new Gen[] { Gen.HALF, Gen.NOTHING };
        private static Gen[] FULL_OR_NOTHING = new Gen[] { Gen.FULL, Gen.NOTHING };
        private static Gen[] ANY = new Gen[] { Gen.HALF, Gen.FULL, Gen.NOTHING };

        private Gen[] _genotype;
        private Schema _schema;

        private static Random random = new Random();

        public Chromosome()
        {
        }

        public Chromosome(int length)
        {
            _genotype = new Gen[length];
        }

        public Chromosome(string g)
        {
            SetGenotype(g);
        }

        public Chromosome(Schema schema)
        {
            _genotype = new Gen[schema.Length];
            _schema = schema;

            for (int i = 0; i < schema.Length; i++)
            {
                _genotype[i] = Chromosome.RandomGen(schema.GetRestriction(i));
            }
        }

        public Chromosome(Schema schema, string g)
        {
            _schema = schema;
            SetGenotype(g);
        }

        private void SetGenotype(string g)
        {
            _genotype = new Gen[g.Length];
            for (int i = 0; i < g.Length; i++)
                if (g[i] == '0')
                    this[i] = Gen.NOTHING;
                else if (g[i] == '1')
                    this[i] = Gen.HALF;
                else
                    this[i] = Gen.FULL;
        }

        private static Gen RandomGen(Restriction restriction)
        {
            if (restriction == Restriction.ANY)
                return (Gen)random.Next(3);

            if (restriction == Restriction.HALF_OR_NOTHING && random.Next(2) == 0)
                return Gen.HALF;

            return Gen.NOTHING;
        }

        private static Gen RandomGenInArray(Gen[] genArray)
        {
            return genArray[random.Next(0, genArray.Length)];   
        }

        /// <summary>
        /// Genera muta el gen en la posición index. Se basa en el schema
        /// para realizar mutaciones válidas.
        /// </summary>
        /// <param name="index">índice del gen a mutar (0..Length)</param>
        /// <returns>true si se mutó el gen, falso si no</returns>
        public bool MutateGen(int index)
        {
            Restriction r = _schema.GetRestriction(index);
            Gen g = _genotype[index];

            if (r == Restriction.ANY)
            {
                // Quiere decir g puede ser cualquier
                // valor, por ende hay que generar uno distinto entre
                // 2 opciones
                if (g == Gen.NOTHING)
                    _genotype[index] = RandomGenInArray(HALF_OR_FULL);
                else if (g == Gen.HALF)
                    _genotype[index] = RandomGenInArray(FULL_OR_NOTHING);
                else // g == GEN.FULL
                    _genotype[index] = RandomGenInArray(HALF_OR_NOTHING);
                return true;
            }
            else if (r == Restriction.HALF_OR_NOTHING)
            {
                // Quiere decir que g es HALF o NOTHING
                // hay que hacer un "flip" entre HALF y NOTHING
                if (g == Gen.NOTHING)
                    _genotype[index] = Gen.HALF;
                else
                    _genotype[index] = Gen.NOTHING;
                return true;
            }
            else
            {
                // r == Restriction.NOTHING
                // el gen en esta posición no acepta, devolver false.
                return false;
            }

        }

        public Gen Get(int index)
        {
            return _genotype[index];
        }

        public void Set(int index, Gen g)
        {
            _genotype[index] = g;
        }

        public int Length { get { return _genotype.Length; } }

        /// <summary>
        /// Crea un clon mutado del cromosoma
        /// </summary>
        /// <param name="index">Muta el gen index</param>
        /// <returns>Clon con gen mutado en index</returns>
        public void Mutate(int index)
        {
            this.MutateGen(index);
        }

        /// <summary>
        /// Crea un clon mutado del cromosoma
        /// </summary>
        /// <returns>Clon mutado del cromosoma</returns>
        public void Mutate()
        {
            this.Mutate(random.Next(0, _genotype.Length));
        }

        /// <summary>
        /// Me dice si dos cromosomas son iguales
        /// </summary>
        /// <param name="obj">Otro cromosoma</param>
        /// <returns>true si tienen el mismo genotipo, false en otro caso.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Chromosome)
            {
                Chromosome other = (Chromosome)obj;
                if (this.Length != other.Length)
                    return false;
                for (int i = 0; i < this.Length; i++)
                    if (this.Get(i) != other.Get(i))
                        return false;
                return true;
            }
            return false;
        }

        public double GetMultiplier(int index)
        {
            if (this[index] == Gen.NOTHING)
                return 0.0;
            return this[index] == Gen.FULL ? 1 : 0.5;
        }

        public double GetContribution(int index, List<Postulante> p)
        {
            return GetMultiplier(index) * p[index].valor_carrera;
        }

        public static bool operator ==(Chromosome a, Chromosome b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            if (a.Length != b.Length)
                return false;

            for (int i = 0; i < a.Length; i++)
                if (a[i] != b[i])
                    return false;
            return true;
        }

        public static bool operator !=(Chromosome a, Chromosome b)
        {
            return !(a == b);
        }

        public override string ToString()
        {
            string cromosoma = "";
            for(int i = 0; i < this.Length; i++)
                cromosoma += (int)Get(i);
            return cromosoma;
        }

        public Gen this[int key]
        {
            get
            {
                return _genotype[key];
            }
            set
            {
                _genotype[key] = value;
            }
        }

        public double fitness { get; set; }

        /// <summary>
        /// Devuelve un arreglo de largo 2 con los hijos resultantes
        /// del crossover entre p1 y p2
        /// </summary>
        /// <param name="p1">Primer padre</param>
        /// <param name="p2">Segundo padre</param>
        /// <returns>Chromosome[2] con los hijos del crossover entre p1 y p2</returns>
        public static Chromosome[] Crossover(Chromosome p1, Chromosome p2)
        {
            // Un solo punto de corte 

            int length = p1.Length;
            int puntoDeCorte = length / 2;

            Chromosome h1 = new Chromosome(p1._schema);
            Chromosome h2 = new Chromosome(p2._schema);

            for (int i = 0; i < length; i++)
            {
                if (i < puntoDeCorte)
                {
                    // h1 tiene la primera mitad de p1, mientras
                    // que h2 la primera mitad de p2.
                    h1[i] = p1[i];
                    h2[i] = p2[i];
                }
                else
                {
                    h1[i] = p2[i];
                    h2[i] = p1[i];
                }
            }

            return new Chromosome[]{ h1, h2 };
        }

        public bool IsBroken(int index, List<Postulante> data)
        {
            if (data[index].promedio < 5.0 && this[index] != Gen.NOTHING)
                return true;
            else if (data[index].ingreso_familiar > 1600000 && this[index] != Gen.NOTHING)
                return true;
            else if (data[index].ingreso_familiar >= 1000000 && this[index] == Gen.FULL)
                return true;
            return false;
        }


        public bool ExceedsMaxBudget()
        {
            return this.fitness > Config.presupuesto_max;
        }

        /// <summary>
        /// Función que repara el cromosoma si es que este usa más 
        /// dinero que el presupuesto máximo
        /// </summary>
        /// <param name="data"></param>
        public void Repare(List<Postulante> data)
        {
            while(ExceedsMaxBudget())
            {
                // Vamos sacando los que menos aportan
                
                int index = 0;

                // Buscamos al primero que aporte
                for (int i = 0; i < Length; i++)
                    if (this[i] != Gen.NOTHING)
                        index = i;

                double contribution = GetContribution(index, data);
                int start = index + 1;

                for (int i = start; i < Length; i++)
                {
                    if (this[i] != Gen.NOTHING)
                        if (GetContribution(i, data) < contribution)
                        {
                            contribution = GetContribution(i, data);
                            index = i;
                        }
                }
                if (this[index] == Gen.FULL)
                    this[index] = Gen.HALF;
                else if (this[index] == Gen.HALF)
                    this[index] = Gen.NOTHING;
                Fitness.Evaluate(this, data);
            }
        }

        /// <summary>
        /// Cuenta el número de restricciones que no cumple el cromosoma
        /// </summary>
        /// <param name="data">Vector de datos</param>
        /// <returns>Número de restricciones que no cumple</returns>
        public int BrokenRestrictions(List<Postulante> data)
        {
            int count = 0;
            for (int i = 0; i < this.Length; i++)
            {
                if (IsBroken(i, data))
                    count++;
            }
            return count;
        }

        /// <summary>
        /// Imprime el cromosoma en la pantalla.
        /// </summary>
        /// <param name="data">Vector de datos</param>
        public void Print(List<Postulante> data)
        {
            int total = 0;
            for (int i = 0; i < Length; i++)
            {
                string msg = data[i].nombre + ", ";

                if(this[i] == Gen.FULL)
                {
                    msg += "Total";
                    total += data[i].valor_carrera;
                }
                else if(this[i] == Gen.HALF)
                {
                    msg += "Media";
                    total += data[i].valor_carrera/2;
                } 
                else
                {
                    msg += "Nada";
                }

                Console.WriteLine(msg);
            }
            Console.WriteLine("Valor total asignado: ${0:#,0}", total);
        }


    }
}