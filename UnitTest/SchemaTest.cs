using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tarea1Evolutiva;
using System.Collections.Generic;

namespace UnitTest
{
    [TestClass]
    public class SchemaTest
    {
        [TestMethod]
        public void Postulantes()
        {
            Schema s1 = SchemaFactory.anySchema();
            Assert.AreEqual("***", s1.ToString());

            Schema s2 = SchemaFactory.nothingSchema();
            Assert.AreEqual("000", s2.ToString());

            Schema s3 = SchemaFactory.halfSchema();
            Assert.AreEqual("111", s3.ToString());

            Schema s4 = SchemaFactory.eachSchema();
            Assert.AreEqual("*10", s4.ToString());

            Assert.AreEqual(s4.GetRestriction(0), Restriction.ANY);
            Assert.AreEqual(s4.GetRestriction(1), Restriction.HALF_OR_NOTHING);
            Assert.AreEqual(s4.GetRestriction(2), Restriction.NOTHING);
        }
    }
}
