using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tarea1Evolutiva;

namespace UnitTest
{
    public static class SchemaFactory
    {
        public static Schema anySchema()
        {
            List<Postulante> list = new List<Postulante>();
            list.Add(new Postulante("1", 900000, 5.0, 500));
            list.Add(new Postulante("2", 900000, 5.1, 500));
            list.Add(new Postulante("3", 900000, 5.2, 500));
            return new Schema(list);
        }

        public static Schema halfSchema()
        {
            List<Postulante> list = new List<Postulante>();
            list.Add(new Postulante("1", 1300000, 5.1, 500));
            list.Add(new Postulante("2", 1300000, 5.1, 500));
            list.Add(new Postulante("3", 1300000, 5.1, 500));
            return new Schema(list);
        }

        public static Schema nothingSchema()
        {
            List<Postulante> list = new List<Postulante>();
            list.Add(new Postulante("1", 1700000, 5.1, 500)); // por dinero
            list.Add(new Postulante("2", 50000, 3.1, 500)); // por promedio
            list.Add(new Postulante("3", 1700000, 3.1, 500)); // por ambas
            return new Schema(list);
        }

        public static Schema eachSchema()
        {
            List<Postulante> list = new List<Postulante>();
            list.Add(new Postulante("1", 900000, 5.1, 500)); // any
            list.Add(new Postulante("2", 1300000, 5.1, 500)); // half
            list.Add(new Postulante("3", 1700000, 3.1, 500)); // nothing
            return new Schema(list);
        }


    }
}
