using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tarea1Evolutiva;
using System.Collections.Generic;

namespace UnitTest
{
    [TestClass]
    public class SelectionTest
    {
        [TestMethod]
        public void Tournament()
        {
            int valorCarrera = 250000;
            List<Postulante> data = new List<Postulante>();
            data.Add(new Postulante("", 200000, 5.0, valorCarrera));
            data.Add(new Postulante("", 200000, 5.0, valorCarrera));
            data.Add(new Postulante("", 200000, 5.0, valorCarrera));
            data.Add(new Postulante("", 200000, 5.0, valorCarrera));
            data.Add(new Postulante("", 200000, 5.0, valorCarrera));
            data.Add(new Postulante("", 200000, 5.0, valorCarrera));
            data.Add(new Postulante("", 200000, 5.0, valorCarrera));
            data.Add(new Postulante("", 200000, 5.0, valorCarrera));

            List<Chromosome> set = new List<Chromosome>();
            set.Add(new Chromosome("22222222"));
            set.Add(new Chromosome("11111111"));
            set.Add(new Chromosome("00000000"));
            set.Add(new Chromosome("10101010"));
            set.Add(new Chromosome("01010101"));

            Chromosome best = Population.TournamentSelect(set);

            Assert.AreEqual(set[0], best);

        }
    }
}
