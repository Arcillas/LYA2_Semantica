using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LYA2_Semantica
{
    class Program
    {
        static void Main(string[] args) 
        {
            try
            {
                using (Lenguaje L = new Lenguaje())
                {
                    L.Programa();
                    /* 
                    while (!L.FinArchivo())
                    {
                        L.nextToken();
                    }
                    */
					// Variable v = new Variable("radio", Variable.TipoDato.FLOAT);
					// v.setValor(100);
					// Console.WriteLine(v.getValor());
					
                }
            } 
            catch (Exception e)
            {
                Console.WriteLine("Error "+e.Message);
            }
        }
    }
}
