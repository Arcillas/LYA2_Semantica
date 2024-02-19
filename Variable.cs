using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LYA2_Semantica
{
    public class Variable
    {
        public enum TipoDato
        {
            Char,Int,Float
        }
        private string nombre;
        private TipoDato  tipo;
        private float valor;
		private string String;
        public Variable(string nombre,TipoDato tipo)
        {
            this.nombre = nombre;
            this.tipo = tipo;
            this.valor = 0;
			this.String = "";
        }
        public void setValor(float valor)
        {
            this.valor = valor;
        }
        public void setString(string String)
		{
			this.String = String;
		}
        public string getNombre()
        {
            return this.nombre;
        }
        public TipoDato getTipo()
        {
            return this.tipo;
        }
        public float getValor()
        {
            return this.valor;
        }
        public string getString()
		{
			return this.String;
		}
	}
}
