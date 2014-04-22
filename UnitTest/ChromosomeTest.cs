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
    public class ChromosomeTest
    {
        /// <summary>
        /// Verifica si las mutaciones se realizaron
        /// </summary>
        [TestMethod]
        public void FacibilidadDeMutar()
        {
            // Generar schema valido
            Schema s = SchemaFactory.anySchema();

            // Generar cromosoma
            Chromosome c = new Chromosome(s);

            Assert.AreEqual(c.MutateGen(0), true);
            Assert.AreEqual(c.MutateGen(1), true);
            Assert.AreEqual(c.MutateGen(2), true);

            // Generar schema que no acepta
            s = SchemaFactory.nothingSchema();
            c = new Chromosome(s);
            Assert.AreEqual(c.MutateGen(0), false);
            Assert.AreEqual(c.MutateGen(1), false);
            Assert.AreEqual(c.MutateGen(2), false);

            // Generar schema que acepta solo NOTHING o HALF
            s = SchemaFactory.halfSchema();
            c = new Chromosome(s);
            Assert.AreEqual(c.MutateGen(0), true);
            Assert.AreEqual(c.MutateGen(1), true);
            Assert.AreEqual(c.MutateGen(2), true);

        }

        /// <summary>
        /// Verifica si para ciertos valores de gen
        /// se muta correctamente el gen
        /// </summary>
        [TestMethod]
        public void MutacionFlip()
        {
            Schema s = new Schema("01*");

            Chromosome c = new Chromosome(s);
            c[0] = Gen.NOTHING;
            c[1] = Gen.HALF;
            c[2] = Gen.FULL;

            // Generar cromosoma
            Gen normal;
            Gen mutado;

            // Mutar un NOTHING CROMOSOMA
            normal = c[0];
            c.MutateGen(0);
            mutado = c[0];
            Assert.AreEqual(mutado, normal, "Mutar NOTHING");

            // Mutar un ANY
            normal = c[2];
            c.MutateGen(2);
            mutado = c[2];

            // Deben ser distintos SI O SI
            Assert.AreNotEqual(normal, mutado, "Mutar ANY");

            // Dependiendo de que era m, n debe ser
            if (normal == Gen.FULL)
                Assert.AreEqual(true, mutado == Gen.HALF || mutado == Gen.NOTHING, "Mutar ANY - Gen.FULL");
            else if (normal == Gen.HALF)
                Assert.AreEqual(true, mutado == Gen.FULL || mutado == Gen.NOTHING, "Mutar ANY - Gen.HALF");
            else // NOTHING
                Assert.AreEqual(true, mutado == Gen.FULL || mutado == Gen.HALF, "Mutar ANY - Gen.NOTHING");
        
            // MUTAR UN HALF
            normal = c.Get(1);
            c.MutateGen(1);
            mutado = c.Get(1);

            if (normal == Gen.HALF)
                Assert.AreEqual(mutado, Gen.NOTHING, "Mutar HALF - Gen.HALF");
            else
                Assert.AreEqual(mutado, Gen.HALF, "Mutar HALF - Gen.NOTHING");

        }

        [TestMethod]
        public void ChromosomeToString()
        {
            // Generar schema que no acepta
            Schema s = SchemaFactory.nothingSchema();
            Chromosome c = new Chromosome(s);

            Assert.AreEqual("000", c.ToString());
        }

        [TestMethod]
        public void Crossover()
        {
            Schema s = new Schema("******");

            Chromosome p1 = new Chromosome(s, "111000");
            Chromosome p2 = new Chromosome(s, "000111");

            Chromosome h1 = new Chromosome(s, "111111");
            Chromosome h2 = new Chromosome(s, "000000");

            Chromosome[] childs = Chromosome.Crossover(p1, p2);
            Chromosome h1Real = childs[0];
            Chromosome h2Real = childs[1];

            Assert.AreEqual(h1, h1Real);
            Assert.AreEqual(h2, h2Real);
        }

        [TestMethod]
        public void CrossoverImpar()
        {
            Schema s = new Schema("*******");

            Chromosome p1 = new Chromosome(s, "1110000");
            Chromosome p2 = new Chromosome(s, "0001111");

            Chromosome h1 = new Chromosome(s, "1111111");
            Chromosome h2 = new Chromosome(s, "0000000");

            Chromosome[] childs = Chromosome.Crossover(p1, p2);
            Chromosome h1Real = childs[0];
            Chromosome h2Real = childs[1];

            Assert.AreEqual(h1, h1Real);
            Assert.AreEqual(h2, h2Real);
        }

    }
}
