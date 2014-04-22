using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tarea1Evolutiva;


namespace UnitTest
{
    [TestClass]
    public class FitnessTest
    {
        [TestMethod]
        public void CalculoSimple()
        {
            int valorCarrera = 500000;

            List<Postulante> p = new List<Postulante>();
            for(int i = 0; i < 30; i++)
                p.Add(new Postulante("" + i, 200000, 5.5, valorCarrera));

            Chromosome c = new Chromosome(30);
            for (int i = 0; i < c.Length; i++)
                c.Set(i, Gen.FULL);
            Assert.AreEqual(30 * valorCarrera, Fitness.Evaluate(c, p), "Todos con FULL");

            for (int i = 0; i < c.Length; i++)
                c.Set(i, Gen.HALF);
            Assert.AreEqual(15 * valorCarrera, Fitness.Evaluate(c, p), "Todos con beca media");

            for (int i = 0; i < c.Length; i++)
                c.Set(i, Gen.NOTHING);
            Assert.AreEqual(0, Fitness.Evaluate(c, p), "Nadie con beca");

            for (int i = 0; i < c.Length; i++)
                if (i < 15)
                    c.Set(i, Gen.NOTHING);
                else
                    c.Set(i, Gen.FULL);
            Assert.AreEqual(15 * valorCarrera, Fitness.Evaluate(c, p), "Mitad con beca full, mitad nada");

        }

        [TestMethod]
        public void CalculoSobrePoblacion()
        {
            int valorCarrera = 500000;
            List<Postulante> data = new List<Postulante>();
            for (int i = 0; i < 3; i++)
                data.Add(new Postulante("" + i, 200000, 5.5, valorCarrera));

            Population p = new Population(data);
            Schema s = new Schema("***");
            p.Add(new Chromosome(s, "000"));
            p.Add(new Chromosome(s, "001"));
            p.Add(new Chromosome(s, "010"));
            p.Add(new Chromosome(s, "011"));
            p.Add(new Chromosome(s, "100"));
            p.Add(new Chromosome(s, "101"));
            p.Add(new Chromosome(s, "110"));
            p.Add(new Chromosome(s, "111"));

            p.Evaluate();

            Assert.AreEqual(0, p[0].fitness);
            Assert.AreEqual(valorCarrera/2, p[1].fitness);
            Assert.AreEqual(valorCarrera/2, p[2].fitness);
            Assert.AreEqual(valorCarrera, p[3].fitness);
            Assert.AreEqual(valorCarrera/2, p[4].fitness);
            Assert.AreEqual(valorCarrera, p[5].fitness);
            Assert.AreEqual(valorCarrera, p[6].fitness);
            Assert.AreEqual(3 * valorCarrera/2, p[7].fitness);

            p.SetBest();
            Assert.AreNotEqual(p.best, null);
        }

        [TestMethod]
        public void BrokenRestrictions()
        {
            List<Postulante> data = new List<Postulante>();
            data.Add(new Postulante("", 1800000, 5.0, 100000));
            data.Add(new Postulante("", 60, 50, 100000));
            data.Add(new Postulante("", 1000000, 5.0, 100000));

            Assert.AreEqual(true,  new Chromosome("200").IsBroken(0, data));
            Assert.AreEqual(false, new Chromosome("000").IsBroken(0, data));
            Assert.AreEqual(true,  new Chromosome("100").IsBroken(0, data));

            Assert.AreEqual(false, new Chromosome("000").IsBroken(1, data), "Test 2.1");
            Assert.AreEqual(false, new Chromosome("010").IsBroken(1, data), "Test 2.2");
            Assert.AreEqual(false, new Chromosome("020").IsBroken(1, data), "Test 2.3");

            Assert.AreEqual(true,  new Chromosome("002").IsBroken(2, data), "Test 3.1");
            Assert.AreEqual(false, new Chromosome("001").IsBroken(2, data), "Test 3.2");
            Assert.AreEqual(false, new Chromosome("000").IsBroken(2, data), "Test 3.3");

        }

        [TestMethod]
        public void PenalizarFitness()
        {
            List<Postulante> data = new List<Postulante>();
            data.Add(new Postulante("", 1700000, 4.8, 1000000));
            Chromosome c = new Chromosome("2");

            Fitness.Evaluate(c, data);
            Assert.AreEqual(1000000, c.fitness);

            Fitness.Penalice(c, data);
            Assert.AreEqual(0, c.fitness);

        }

        [TestMethod]
        public void Restaurar()
        {
            int oldBudget = Config.presupuesto_max;
            Config.presupuesto_max = 10;

            List<Postulante> data = new List<Postulante>();
            data.Add(new Postulante("", 0, 0, 5));
            data.Add(new Postulante("", 0, 0, 5));
            data.Add(new Postulante("", 0, 0, 4));
            data.Add(new Postulante("", 0, 0, 3));
            data.Add(new Postulante("", 0, 0, 2));
            data.Add(new Postulante("", 0, 0, 1));

            Chromosome c = new Chromosome(new Schema("******"), "222000");
            Fitness.Evaluate(c, data);

            Assert.AreEqual(true, c.ExceedsMaxBudget());
            c.Repare(data);
            Assert.AreEqual(false, c.ExceedsMaxBudget());

            Config.presupuesto_max = oldBudget;
        }

    }
}
