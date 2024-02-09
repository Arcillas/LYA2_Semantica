using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LYA2_Semantica
{
    public class Error : Exception
    {
        public Error(string mensaje, StreamWriter log) : base(mensaje)
        {
            log.WriteLine("Error "+mensaje);
        }
        public Error(string mensaje, int line) : base(mensaje)
        {
        }


    }
}
