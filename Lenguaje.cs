using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/*
	[ ]	Requerimento 1: Meter al stack el valor de la variable
	[ ]	Requeriminto 2: Modificar el Valor de la variable, y no pasar por alto el ++ y el --
	[ ]	Requeriminto 3: Printf: Implementar secuencias de escape, quitar comillas
	[ ]	Requeriminto 4: Scanf: Modificar el valor de la variable, y levantar una excepcion si
						lo capturado no es un numero
	[ ]	Requeriminto 5: Implementar el casteo

	Tipo: usando un replace de '\n' a un '\\n'

*/

namespace LYA2_Semantica
{
    public class Lenguaje : Sintaxis
    {
        List<Variable>	variables;
		Stack<float> s;
        public Lenguaje()
        {
            variables = new List<Variable>();
			s = new Stack<float>();
        }
        public Lenguaje(string nombre) : base(nombre)
        {
			s = new Stack<float>();
            variables = new List<Variable>();
        }
        //Programa  -> Librerias? Variables? Main
        public void Programa()
        {
            if (getContenido() == "#")
            {
                Librerias();
            }
            if (getClasificacion() == Tipos.tipoDatos)
            {
                Variables();
            }
            Main();
            imprimirVariables();
			// imprimeStack();
        }
		private void imprimirVariables(){
			log.WriteLine("Variables");
			log.WriteLine("====================");
			foreach (Variable v in variables)
			{
			    	log.WriteLine(v.getNombre() + " = "+ v.getValor());
			}
		}

		private void imprimeStack(){
			Console.WriteLine("\nStack:\n\t+---------------+\t");
			foreach (float valor in s)
			{
			    	Console.WriteLine("\t|\t" + valor + "\t|\t");
			}
			Console.WriteLine("\t+---------------+\t");

		}

		private float valorVariable(string nombre){
            foreach (Variable v in variables)
            {
                if (v.getNombre() == nombre)
                {
                    return v.getValor();
                }
            }
            return 0;
        }
		
		private float modificarValor(String nombre, float NewValor){
			foreach (Variable v in variables)
			{
				if (v.getNombre() == nombre)
				{
					v.setValor(NewValor);
					return NewValor;
				}
			}
			return 0;
		}

		private bool existeVariable(string nombre){
            foreach (Variable v in variables)
            {
                if (v.getNombre() == nombre)
                {
                    return true;
                }
            }
            return false;
        }
        //Librerias -> #include<identificador(.h)?> Librerias?
        private void Librerias()
        {
            match("#");
            match("include");
            match("<");
            match(Tipos.Identificador);
            if (getContenido() == ".")
            {
                match(".");
                match("h");
            }
            match(">");
            if (getContenido() == "#")
            {
                Librerias();
            }
        }
        //Variables -> tipoDato listaIdentificadores; Variables?
        private void Variables()
        {
            Variable.TipoDato tipo = Variable.TipoDato.Char;
            switch(getContenido())
            {
                case "int":
                    tipo = Variable.TipoDato.Int;
                    break;
                case "float":
                    tipo = Variable.TipoDato.Float;
                    break;
            }
            match(Tipos.tipoDatos);
            listaIdentificadores(tipo);

            match(";");
            if (getClasificacion() == Tipos.tipoDatos)
            {
                Variables();
            }
        }
        //listaIdentificadores -> Identificador (,listaIdentificadores)?
        private void listaIdentificadores(Variable.TipoDato tipo)
        {
            string nombre = getContenido();
            match(Tipos.Identificador);
            if(!existeVariable(nombre))
            {
                variables.Add(new Variable(nombre,tipo));
            }
            else
            {
                throw new Error("de Sintaxis : la variable " + nombre + " ya existe",log);
            }
            if (getContenido() == ",")
            {
                match(",");
                listaIdentificadores(tipo);
            }
        }
        //bloqueInstrucciones -> { listaIntrucciones? }
        private void bloqueInstrucciones()
        {
            match("{");
            if (getContenido() != "}")
            {
                ListaInstrucciones();
            }
            match("}");
        }
        //ListaInstrucciones -> Instruccion ListaInstrucciones?
        private void ListaInstrucciones()
        {
            Instruccion();
            if (getContenido() != "}")
            {
                ListaInstrucciones();
            }
        }
        //Instruccion -> Printf | Scanf | If | While | do while | For | Asignacion
        private void Instruccion()
        {
            if (getContenido() == "printf")
            {
                Printf();
            }
            else if (getContenido() == "scanf")
            {
                Scanf();
            }
            else if (getContenido() == "if")
            {
                If();
            }
            else if (getContenido() == "while")
            {
                While();
            }
            else if (getContenido() == "do")
            {
                Do();
            }
            else if (getContenido() == "for")
            {
                For();
            }
            else
            {
                Asignacion();
            }
        }
        //    Requerimiento 1: Printf -> printf(cadena(, Identificador)?);
        private void Printf()
        {
            match("printf");
            match("(");
			Console.Write(getContenido());
            match(Tipos.Cadena);
            if (getContenido() == ",")
            {
                match(",");
                Console.Write(valorVariable(getContenido()));
				match(Tipos.Identificador);
            }
            match(")");
            match(";");

        }
        //    Requerimiento 2: Scanf -> scanf(cadena,&Identificador);
        private void Scanf()
        {
            match("scanf");
            match("(");
            match(Tipos.Cadena);
            match(",");
            match("&");
			string name = getContenido();
            match(Tipos.Identificador);
			string var = Console.ReadLine();
			modificarValor(name, float.Parse(var));
            match(")");
            match(";");
        }

        //Asignacion -> Identificador (++ | --) | (= Expresion);
        private void Asignacion()
        {
            match(Tipos.Identificador);
            if (getClasificacion() == Tipos.OperadorTermino)
            {
                match(Tipos.OperadorTermino);
            }
            else if (getClasificacion() == Tipos.OperadorFactor)
            {
                match(Tipos.OperadorFactor);
            }
            else if (getClasificacion() == Tipos.Incremento)
            {
                match(Tipos.Incremento);
            }
            else if (getClasificacion() == Tipos.Decremento)
            {
                match(Tipos.Decremento);
            }

            else if (getClasificacion() == Tipos.IncrementoTermino)
            {
                match(Tipos.IncrementoTermino);
                Expresion();
            }
            else if (getClasificacion() == Tipos.IncrementoFactor)
            {
                match(Tipos.IncrementoFactor);
                Expresion();
            }

            else
            {
                match("=");
                Expresion();
            }
			Console.WriteLine(s.Pop());
            match(";");
        }
        //If -> if (Condicion) instruccion | bloqueInstrucciones 
        //      (else instruccion | bloqueInstrucciones)?
        private void If()
        {
            match("if");
            match("(");
            Condicion();
            match(")");
            if (getContenido() == "{")
            {
                bloqueInstrucciones();
            }
            else
            {
                Instruccion();
            }
            if (getContenido() == "else")
            {
                match("else");
                if (getContenido() == "{")
                {
                    bloqueInstrucciones();
                }
                else
                {
                    Instruccion();
                }
            }
        }
        //Condicion -> Expresion operadoRelacional Expresion
        private void Condicion()
        {
            Expresion();
            match(Tipos.OperadorRelacional);
            Expresion();
        }
        //While -> while(Condicion) bloqueInstrucciones | Instruccion
        private void While()
        {
            match("while");
            match("(");
            Condicion();
            match(")");
            if (getContenido() == "{")
            {
                bloqueInstrucciones();
            }
            else
            {
                Instruccion();
            }
        }
        //Do -> do bloqueInstrucciones | Intruccion while(Condicion);
        private void Do()
        {
            match("do");
            if (getContenido() == "{")
            {
                bloqueInstrucciones();
            }
            else
            {
                Instruccion();
            }
            match("while");
            match("(");
            Condicion();
            match(")");
            match(";");

        }
        //For -> for(Asignacion Condicion; Incremento) BloqueInstruccones | Instruccion 
        private void For()
        {
            match("for");
            match("(");
            Asignacion();
            Condicion();
            match(";");
            Incremento();
            match(")");
            if (getContenido() == "{")
            {
                bloqueInstrucciones();
            }
            else
            {
                Instruccion();
            }
        }
        //Incremento -> Identificador ++ | --
        private void Incremento()
        {
            match(Tipos.Identificador);
            if (getClasificacion() == Tipos.Incremento)
            {
                match(Tipos.Incremento);
            }
            else
            {
                match(Tipos.Decremento);
            }

        }
        //Main      -> void main() bloqueInstrucciones
        private void Main()
        {
            match("void");
            match("main");
            match("(");
            match(")");
            bloqueInstrucciones();
        }
        //Expresion -> Termino MasTermino
        private void Expresion()
        {
            Termino();
            MasTermino();
        }
        //MasTermino -> (OperadorTermino Termino)?
        private void MasTermino()
        {
            if (getClasificacion() == Tipos.OperadorTermino)
            {
				string op = getContenido();
                match(Tipos.OperadorTermino);
                Termino();
				float N1 = s.Pop();
				float N2 = s.Pop();
				switch (op){
					case "+": s.Push(N2+N1); break;
					case "-": s.Push(N2-N1); break;
				}

				// Console.Write(" " + op);
            }
        }
        //Termino -> Factor PorFactor
        private void Termino()
        {
            Factor();
            PorFactor();

        }
        //PorFactor -> (OperadorFactor Factor)?
        private void PorFactor()
        {
            if (getClasificacion() == Tipos.OperadorFactor)
            {
				string op = getContenido();
                match(Tipos.OperadorFactor);
                Factor();
				
				float N1 = s.Pop();
				float N2 = s.Pop();
				switch (op){
					case "*": s.Push(N2*N1); break;
					case "/": s.Push(N2/N1); break;
					case "%": s.Push(N2%N1); break;
				}
				
				// Console.Write(" " + op);
            }
        }
        //Factor -> numero | identificador | (Expresion)
        private void Factor()
        {
            if (getClasificacion() == Tipos.Numero)
            {
				// Console.Write(" " + getContenido());
				s.Push(float.Parse(getContenido()));
                match(Tipos.Numero);
            }
            else if (getClasificacion() == Tipos.Identificador)
            {
				// Valor de la variable s.Push(); // 
				s.Push(valorVariable(getContenido()));
				// Console.Write(" " + getContenido());
                match(Tipos.Identificador);
            }
            else
            {
                match("(");
                Expresion();
                match(")");
            }
        }
    }
}
