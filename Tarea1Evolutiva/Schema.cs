using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tarea1Evolutiva
{

    public enum Restriction
    {
        ANY = 2, // MEDIA o FULL
        NOTHING = 0, // No puede recibir
        HALF_OR_NOTHING = 1, // Solo Media
    }

    public class Schema
    {

        private Restriction[] _schema;
        private List<Postulante> _postulantes;

        public Schema(string s)
        { 
            _schema = new Restriction[s.Length];
            for (int i = 0; i < s.Length; i++)
                if (s[i] == '*')
                    _schema[i] = Restriction.ANY;
                else if (s[i] == '0')
                    _schema[i] = Restriction.NOTHING;
                else
                    _schema[i] = Restriction.HALF_OR_NOTHING;
        }

        public Schema(List<Postulante> postulantes)
        {
            _postulantes = postulantes;
            _schema = new Restriction[postulantes.Count];

            for (int i = 0; i < postulantes.Count; i++)
            {
                // 1) No puede asignarse beca a un alumno con promedio ponderado menor a 5.0
                if (_postulantes[i].promedio < 5.0)
                    _schema[i] = Restriction.NOTHING;
                // 2) ni con ingreso familiar mayor que $1.600.000,
                else if (_postulantes[i].ingreso_familiar > 1600000)
                    _schema[i] = Restriction.NOTHING;
                // 3) beca completa: sólo pueden ser aquellos con ingreso familiar menor a $1.000.000
                else if (_postulantes[i].ingreso_familiar >= 1000000)
                    _schema[i] = Restriction.HALF_OR_NOTHING;
                else
                    _schema[i] = Restriction.ANY;
            }
        }

        public void SetRestriction(int index, Restriction value)
        {
            _schema[index] = value;
        }

        public Restriction GetRestriction(int index)
        {
            return _schema[index];
        }

        public Restriction this[int index]
        {
            get
            {
                return _schema[index];
            }
            set
            {
                _schema[index] = value;
            }
        }

        public int Length { get { return _schema.Length; } }

        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < _schema.Length; i++)
                if(_schema[i] == Restriction.ANY)
                    s += "*";
                else
                    s += (int)_schema[i];
            return s;
        }

    }
}
