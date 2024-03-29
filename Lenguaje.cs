using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/*
	[*]	Requerimento 1: Meter al stack el valor de la variable
	[*]	Requeriminto 2: Modificar el Valor de la variable, y no pasar por alto el ++ y el --
	[*]	Requeriminto 3: Printf: Implementar secuencias de escape, quitar comillas
	[*]	Requeriminto 4: Scanf: Modificar el valor de la variable, y levantar una excepcion si
						lo capturado no es un numero
	[*]	Requeriminto 5: Implementar el casteo (La neta no se como implementarlo)

	Tipo: usando un replace de '\n' a un '\\n'


	Requerimiento 1: Marcar errores sintacticos para variables no declaradas
    Requerimiento 2: Asignacion, modificar el valor de la variable, no pasar por alto el ++ y el --
    Requerimiento 3: Printf, Quitar las comillas, implementar secuencas de escape
    Requerimiento 4: Modificar el valor de la variable en el Scanf y levantar una excepcion
                     si lo capturado no es un número
    Requerimiento 5: Implementar el casteo

*/

namespace LYA2_Semantica
{
    public class Lenguaje : Sintaxis
    {
        List<Variable> variables;
        Stack<float> s;
        public Lenguaje()
        {
            variables = new List<Variable>();
            s = new Stack<float>();
        }
        public Lenguaje(string nombre) : base(nombre)
        {
            variables = new List<Variable>();
            s = new Stack<float>();
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

        private void imprimirVariables()
        {
            log.WriteLine("Variables");
            log.WriteLine("====================");
            foreach (Variable v in variables)
            {
                log.WriteLine(v.getNombre() + " = " + v.getValor());

            }
        }

        private void imprimeStack()
        {
            Console.WriteLine("\nStack:\n\t+---------------+\t");
            log.WriteLine("\nStack:\n\t+---------------+\t");
            foreach (float valor in s)
            {
                Console.WriteLine("\t|\t" + valor + "\t|\t");
                log.WriteLine("\t|\t" + valor + "\t|\t");
            }
            Console.WriteLine("\t+---------------+\t");
            log.WriteLine("\t+---------------+\t");

        }


        private float valorVariable(string nombre)
        {
            foreach (Variable v in variables)
            {
                if (v.getNombre() == nombre)
                {
                    return v.getValor();
                }
            }
            return 0;
        }
        private void modificarValor(String nombre, float NewValor)
        {
            foreach (Variable v in variables)
            {
                if (v.getNombre() == nombre)
                {
                    if (v.getTipo() == Variable.TipoDato.Char)
                    {
                        if (NewValor >= 0 && NewValor <= 255)
                        {
                            v.setValor(NewValor);
                        }
                        else
                        {
                            throw new Error("de Sintaxis: el valor de la variable [" + nombre + "] no es un char ", log);
                        }
                    }
                    else if (v.getTipo() == Variable.TipoDato.Int)
                    {
                        if (NewValor <= 65535)
                        {
                            v.setValor(NewValor);
                        }
                        else
                        {
                            throw new Error("de Sintaxis: el valor de la variable [" + nombre + "] no es un int ", log);
                        }
                    }
                    else
                    {

                        v.setValor(NewValor);
                    }

                }
            }
        }

        private bool existeVariable(string nombre)
        {
            foreach (Variable v in variables)
            {
                if (v.getNombre() == nombre)
                {
                    return true;
                }
            }
            return false;
        }

        //Variables -> tipoDato listaIdentificadores; Variables?
        private void Variables()
        {
            Variable.TipoDato tipoDato = Variable.TipoDato.Char;
            switch (getContenido())
            {
                case "int":
                    tipoDato = Variable.TipoDato.Int;
                    break;
                case "float":
                    tipoDato = Variable.TipoDato.Float;
                    break;
                default:
                    tipoDato = Variable.TipoDato.Char;
                    break;
            }
            match(Tipos.tipoDatos);
            listaIdentificadores(tipoDato);
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
            if (!existeVariable(nombre))
            {
                variables.Add(new Variable(nombre, tipo));
            }
            else
            {
                throw new Error("de Sintaxis : la variable " + nombre + " ya existe ", log);

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

            if (getClasificacion() == Tipos.tipoDatos)
                Variables();

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

            string str = getContenido();

            str = str.Replace("\"", "");
            str = str.Replace("\\n", "\n");
            str = str.Replace("\\t", "\t");

                match(Tipos.Cadena);
            if (getContenido() == ",")
            {
                match(",");

                if (str.Contains("%f"))
                {
                    str = str.Replace("%f", valorVariable(getContenido()).ToString());
                }

				string var = getContenido();

                if (!existeVariable(var))
                	throw new Error("de Sintaxis: No existe la variable "+ var, log);
				match(Tipos.Identificador);
            }
            Console.Write(str);
            match(")");
            match(";");

        }
        //    Requerimiento 2: Scanf -> scanf(cadena,&Identificador);
        private void Scanf()
        {
            string name = "";
            string? var = "deja de decir que es no nullable la concha de tu madre"; // equisde, con el ? se convierte en nullable y así no se queja el compilador 
            match("scanf");
            match("(");
            match(Tipos.Cadena);
            match(",");
            match("&");
            name = getContenido();
            match(Tipos.Identificador);
            var = Console.ReadLine();

            if (!existeVariable(name))
				throw new Error("de Sintaxis: No existe la variable " + name, log);
			if (var != null)
				modificarValor(name, float.Parse(var));
			else 
				throw new Error("de Entrada: No se capturó un numero o es \"null\"", log);

            match(")");
            match(";");
        }

        //Asignacion -> Identificador (++ | --) | (= Expresion);
        private void Asignacion()
        {
            string var_1;
            float var1_value = 0;
            float var2_value = 0;
            string var_2;
            float new_val = 0;

            var_1 = getContenido();

            // Console.WriteLine(getContenido()); 

            if (existeVariable(var_1))
            {
				match(Tipos.Identificador);
                var1_value = valorVariable(var_1);

                if (getClasificacion() == Tipos.Incremento)
                {
                    match(Tipos.Incremento);
                    new_val = var1_value + 1;
                    // modificarValor(var_1, new_val);
                }
                else if (getClasificacion() == Tipos.Decremento)
                {
                    match(Tipos.Decremento);
                    new_val = var1_value - 1;
                    // modificarValor(var_1, new_val);
                }

                else if (getClasificacion() == Tipos.IncrementoTermino)
                {

                    if (getContenido().Equals("+="))
                    {
                        match("+=");
                        if (getClasificacion() == Tipos.Identificador)
                        {
                            var_2 = getContenido();
                            match(Tipos.Identificador);
                            var2_value = valorVariable(var_2);
                            new_val = var1_value + var2_value;
                        }
                        else if (getClasificacion() == Tipos.Numero)
                        {
                            var2_value = float.Parse(getContenido());
                            match(Tipos.Numero);
                            new_val = var1_value + var2_value;
                        }
                        else
                        {
                            Expresion();
                            new_val = s.Pop();
                        }
                        // modificarValor(var_1, new_val);
                    }

                    else if (getContenido().Equals("-="))
                    {
                        match("-=");
                        if (getClasificacion() == Tipos.Identificador)
                        {
                            var_2 = getContenido();
                            match(Tipos.Identificador);
                            var2_value = valorVariable(var_2);
                            new_val = var1_value - var2_value;
                        }
                        else if (getClasificacion() == Tipos.Numero)
                        {
                            var2_value = float.Parse(getContenido());
                            match(Tipos.Numero);
                            new_val = var1_value - var2_value;
                        }
                        else
                        {
                            Expresion();
                            new_val = s.Pop();
                        }
                        // modificarValor(var_1, new_val);
                    }
                    // modificarValor(var_1, new_val);

                }
                else if (getClasificacion() == Tipos.IncrementoFactor)
                {

                    if (getContenido().Equals("*="))
                    {
                        match("*=");
                        if (getClasificacion() == Tipos.Identificador)
                        {
                            var_2 = getContenido();
                            match(Tipos.Identificador);
                            var2_value = valorVariable(var_2);
                            new_val = var1_value * var2_value;
                        }
                        else if (getClasificacion() == Tipos.Numero)
                        {
                            var2_value = float.Parse(getContenido());
                            match(Tipos.Numero);
                            new_val = var1_value * var2_value;
                        }
                        else
                        {
                            Expresion();
                            new_val = s.Pop();
                        }
                        // modificarValor(var_1, new_val);
                    }

                    else if (getContenido().Equals("/="))
                    {
                        match("/=");
                        if (getClasificacion() == Tipos.Identificador)
                        {
                            var_2 = getContenido();
                            match(Tipos.Identificador);
                            var2_value = valorVariable(var_2);
                            new_val = var1_value / var2_value;
                        }
                        else if (getClasificacion() == Tipos.Numero)
                        {
                            var2_value = float.Parse(getContenido());
                            match(Tipos.Numero);
                            new_val = var1_value / var2_value;
                        }
                        else
                        {
                            Expresion();
                            new_val = s.Pop();
                        }
                    }
                    // modificarValor(var_1, new_val);
                }

                else
                {
                    var1_value = valorVariable(var_1);
                    match("=");
                    Expresion();
                    new_val = s.Pop();
                }
                // Console.WriteLine("New val: "+ new_val);
                modificarValor(var_1, new_val);

            }
            else
            {
                throw new Error("de Sintaxis: la variable " + var_1 + " no existe ", log);
            }
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
			string var_name = getContenido();	
			if (!existeVariable(var_name))
				throw new Error("de Sintaxis: la variable " + var_name + " no existe ", log);
			
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
                switch (op)
                {
                    case "+": s.Push(N2 + N1); break;
                    case "-": s.Push(N2 - N1); break;
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

                switch (op)
                {
                    case "*": s.Push(N1 * N2); break;
                    case "/": s.Push(N1 / N2); break;
                    case "%": s.Push(N1 % N2); break;
                }
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
				string var_name = getContenido();
				if (!existeVariable(var_name))
					throw new Error("de Sintaxis: la variable " + var_name + " no existe ", log);
                s.Push(valorVariable(getContenido()));
                match(Tipos.Identificador);
            }
            else
            {
                match("(");
                if (getClasificacion() == Tipos.tipoDatos)
                {
                    float val = 0;
                    string tipo = getContenido();
                    match(Tipos.tipoDatos);
                    match(")");
                    Expresion();
                    val = s.Pop();
                    // POP 
                    // %255 O %65536
                    // PUSH
                    switch (tipo)
                    {
                        case "char":
                            // Console.WriteLine(val % 256);
                            s.Push(val % 256);
                            // Console.WriteLine(val % 256);
                            break;
                        case "int":
                            // Console.WriteLine(val % 65536);
                            s.Push(val % 65536);
                            break;
                        case "float":
                            s.Push(val);
                            break;
                    }
                }
                else
                {
                    Expresion();
                    match(")");
                }
            }
        }
    }
}

