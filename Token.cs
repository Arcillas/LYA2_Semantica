using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LYA2_Semantica
{
    public class Token
    {
        public enum Tipos
        {
            Identificador,Numero,Caracter,Asignacion,FinSentencia,FinArchivo,
            OperadorLogico,OperadorRelacional,OperadorTermino,IncrementoTermino,
            OperadorFactor,IncrementoFactor,OperadorTernario,Cadena,Inicio,Fin,
            tipoDatos, reservada, Incremento, Decremento
        }
        private string contenido;
        private Tipos  clasificacion;
        public Token()
        {
            contenido = "";
            clasificacion = Tipos.Identificador;
        }
        public void setContenido(string contenido)
        {
            this.contenido = contenido;
        }
        public void setClasificacion(Tipos clasificacion)
        {
            this.clasificacion = clasificacion;
        }
        public string getContenido()
        {
            return this.contenido;
        }
        public Tipos getClasificacion()
        {
            return this.clasificacion;
        }
    }
}
