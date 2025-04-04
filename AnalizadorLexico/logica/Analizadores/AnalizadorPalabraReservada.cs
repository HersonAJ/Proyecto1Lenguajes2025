/*using Logica;

namespace Analizadores
{
    public class AnalizadorPalabraReservada
    {
        private Estado estadoActual;

        public AnalizadorPalabraReservada()
        {
            estadoActual = Estado.Q0; // Estado inicial
        }

        public Estado AnalizarCaracter(string token)
        {
            estadoActual = Estado.Q0; // Reiniciar el estado inicial

            switch (token[0]) // Validar la primera letra del token
            {
                case 'i': // Palabras reservadas que comienzan con 'i': if, int, import, interface, implements
                    if ((token.EndsWith('f') && token.Length == 2) ||    // "if"
                        (token.EndsWith('t') && token.Length == 3) ||    // "int"
                        (token.EndsWith('t') && token.Length == 6) ||    // "import"
                        (token.EndsWith('e') && token.Length == 9) ||    // "interface"
                        (token.EndsWith('s') && token.Length == 10))     // "implements"
                    {
                        estadoActual = Estado.QF;
                    }
                    break;

                case 'c': // Palabras reservadas que comienzan con 'c': class, case
                    if ((token.EndsWith('s') && token.Length == 5) ||    // "class"
                        (token.EndsWith('e') && token.Length == 4))      // "case"
                    {
                        estadoActual = Estado.QF;
                    }
                    break;

                case 'f': // Palabras reservadas que comienzan con 'f': for, false, float, final
                    if ((token.EndsWith('r') && token.Length == 3) ||    // "for"
                        (token.EndsWith('e') && token.Length == 5) ||    // "false"
                        (token.EndsWith('t') && token.Length == 5) ||    // "float"
                        (token.EndsWith('l') && token.Length == 5))      // "final"
                    {
                        estadoActual = Estado.QF;
                    }
                    break;

                case 't': // Palabras reservadas que comienzan con 't': then, true
                    if ((token.EndsWith('n') && token.Length == 4) ||    // "then"
                        (token.EndsWith('e') && token.Length == 4))      // "true"
                    {
                        estadoActual = Estado.QF;
                    }
                    break;

                case 'e': // Palabras reservadas que comienzan con 'e': else, extends
                    if ((token.EndsWith('e') && token.Length == 4) ||    // "else"
                        (token.EndsWith('s') && token.Length == 7))      // "extends"
                    {
                        estadoActual = Estado.QF;
                    }
                    break;

                case 'p': // Palabras reservadas que comienzan con 'p': public, private, package, protected
                    if ((token.EndsWith('c') && token.Length == 6) ||    // "public"
                        (token.EndsWith('e') && token.Length == 7) ||    // "private"
                        (token.EndsWith('e') && token.Length == 7) ||    // "package"
                        (token.EndsWith('d') && token.Length == 9))      // "protected"
                    {
                        estadoActual = Estado.QF;
                    }
                    break;

                case 'r': // Palabra reservada que comienza con 'r': return
                    if (token.EndsWith('n') && token.Length == 6)        // "return"
                    {
                        estadoActual = Estado.QF;
                    }
                    break;

                case 'w': // Palabra reservada que comienza con 'w': while
                    if (token.EndsWith('e') && token.Length == 5)        // "while"
                    {
                        estadoActual = Estado.QF;
                    }
                    break;

                case 's': // Palabras reservadas que comienzan con 's': static, short
                    if ((token.EndsWith('c') && token.Length == 6) ||    // "static"
                        (token.EndsWith('t') && token.Length == 5))      // "short"
                    {
                        estadoActual = Estado.QF;
                    }
                    break;

                case 'v': // Palabra reservada que comienza con 'v': void
                    if (token.EndsWith('d') && token.Length == 4)        // "void"
                    {
                        estadoActual = Estado.QF;
                    }
                    break;

                case 'b': // Palabra reservada que comienza con 'b': boolean
                    if (token.EndsWith('n') && token.Length == 7)        // "boolean"
                    {
                        estadoActual = Estado.QF;
                    }
                    break;

                default:
                    // Si no coincide con ninguna condición, no se procesará como válido
                    break;
            }

            return estadoActual;
        }

        public Token ProcesarPalabraReservada(string token, int fila, int columna)
        {
            var estadoFinal = AnalizarCaracter(token);

            // Si el estado final es QF, es una palabra reservada válida
            if (estadoFinal == Estado.QF)
            {
                return new Token("PalabraReservada", token, fila, columna);
            }

            // Si no llega a QF, no es válido en este autómata
            return null!;
        }
    }
}
*/



using System.Runtime.CompilerServices;
using Logica;

namespace Analizadores
{
    public class AnalizadorPalabraReservada
    {
        private Estado estadoActual;

        public AnalizadorPalabraReservada()
        {
            estadoActual = Estado.Q0;//estado inicial
        }

        public Estado AnalizarCaracter(string token)
        {
            try
            {
                estadoActual = Estado.Q0;

                switch (token)
                {
                    case "for":
                        foreach (char caracter in token)
                        {
                            switch (estadoActual)
                            {
                                case Estado.Q0:
                                    if (caracter == 'f')
                                    {
                                        estadoActual = Estado.Q1;
                                    }
                                    break;

                                case Estado.Q1:
                                    if (caracter == 'o')
                                    {
                                        estadoActual = Estado.Q2;
                                    }
                                    break;
                                case Estado.Q2:
                                    if (caracter == 'r')
                                    {
                                        estadoActual = Estado.QF;
                                    }
                                    break;
                            }
                        }
                        break;
                    case "if":
                        foreach (char caracter in token)
                        {
                            switch (estadoActual)
                            {
                                case Estado.Q0:
                                    if (caracter == 'i')
                                    {
                                        estadoActual = Estado.Q1;
                                    }
                                    break;
                                case Estado.Q1:
                                    if (caracter == 'f')
                                    {
                                        estadoActual = Estado.QF;
                                    }
                                    break;
                            }
                        }
                        break;
                    case "int":
                        foreach (char caracter in token)
                        {
                            switch (estadoActual)
                            {
                                case Estado.Q0:
                                    if (caracter == 'i')
                                    {
                                        estadoActual = Estado.Q1;
                                    }
                                    break;
                                case Estado.Q1:
                                    if (caracter == 'n')
                                    {
                                        estadoActual = Estado.Q2;
                                    }
                                    break;
                                case Estado.Q2:
                                    if (caracter == 't')
                                    {
                                        estadoActual = Estado.QF;
                                    }
                                    break;
                            }
                        }
                        break;
                    case "import":
                        foreach (char caracter in token)
                        {
                            switch (estadoActual)
                            {
                                case Estado.Q0:
                                    if (caracter == 'i')
                                    {
                                        estadoActual = Estado.Q1;
                                    }
                                    break;
                                case Estado.Q1:
                                    if (caracter == 'm')
                                    {
                                        estadoActual = Estado.Q2;
                                    }
                                    break;
                                case Estado.Q2:
                                    if (caracter == 'p')
                                    {
                                        estadoActual = Estado.Q3;
                                    }
                                    break;
                                case Estado.Q3:
                                    if (caracter == 'o')
                                    {
                                        estadoActual = Estado.Q4;
                                    }
                                    break;
                                case Estado.Q4:
                                    if (caracter == 'r')
                                    {
                                        estadoActual = Estado.Q5;
                                    }
                                    break;
                                case Estado.Q5:
                                    if (caracter == 't')
                                    {
                                        estadoActual = Estado.QF;
                                    }
                                    break;
                            }
                        }
                        break;
                    case "interface":
                        foreach (char caracter in token)
                        {
                            switch (estadoActual)
                            {
                                case Estado.Q0:
                                    if (caracter == 'i')
                                    {
                                        estadoActual = Estado.Q1;
                                    }
                                    break;
                                case Estado.Q1:
                                    if (caracter == 'n')
                                    {
                                        estadoActual = Estado.Q2;
                                    }
                                    break;
                                case Estado.Q2:
                                    if (caracter == 't')
                                    {
                                        estadoActual = Estado.Q3;
                                    }
                                    break;
                                case Estado.Q3:
                                    if (caracter == 'e')
                                    {
                                        estadoActual = Estado.Q4;
                                    }
                                    break;
                                case Estado.Q4:
                                    if (caracter == 'r')
                                    {
                                        estadoActual = Estado.Q5;
                                    }
                                    break;
                                case Estado.Q5:
                                    if (caracter == 'f')
                                    {
                                        estadoActual = Estado.Q6;
                                    }
                                    break;
                                case Estado.Q6:
                                    if (caracter == 'a')
                                    {
                                        estadoActual = Estado.Q7;
                                    }
                                    break;
                                case Estado.Q7:
                                    if (caracter == 'c')
                                    {
                                        estadoActual = Estado.Q8;
                                    }
                                    break;
                                case Estado.Q8:
                                    if (caracter == 'e')
                                    {
                                        estadoActual = Estado.QF;
                                    }
                                    break;
                            }
                        }
                        break;
                    case "implements":
                        foreach (char caracter in token)
                        {
                            switch (estadoActual)
                            {
                                case Estado.Q0:
                                    if (caracter == 'i')
                                    {
                                        estadoActual = Estado.Q1;
                                    }
                                    break;
                                case Estado.Q1:
                                    if (caracter == 'm')
                                    {
                                        estadoActual = Estado.Q2;
                                    }
                                    break;
                                case Estado.Q2:
                                    if (caracter == 'p')
                                    {
                                        estadoActual = Estado.Q3;
                                    }
                                    break;
                                case Estado.Q3:
                                    if (caracter == 'l')
                                    {
                                        estadoActual = Estado.Q4;
                                    }
                                    break;
                                case Estado.Q4:
                                    if (caracter == 'e')
                                    {
                                        estadoActual = Estado.Q5;
                                    }
                                    break;
                                case Estado.Q5:
                                    if (caracter == 'm')
                                    {
                                        estadoActual = Estado.Q6;
                                    }
                                    break;
                                case Estado.Q6:
                                    if (caracter == 'e')
                                    {
                                        estadoActual = Estado.Q7;
                                    }
                                    break;
                                case Estado.Q7:
                                    if (caracter == 'n')
                                    {
                                        estadoActual = Estado.Q8;
                                    }
                                    break;
                                case Estado.Q8:
                                    if (caracter == 't')
                                    {
                                        estadoActual = Estado.Q9;
                                    }
                                    break;
                                case Estado.Q9:
                                    if (caracter == 's')
                                    {
                                        estadoActual = Estado.QF;
                                    }
                                    break;
                            }
                        }
                        break;
                    case "class":
                        foreach (char caracter in token)
                        {
                            switch (estadoActual)
                            {
                                case Estado.Q0:
                                    if (caracter == 'c')
                                    {
                                        estadoActual = Estado.Q1;
                                    }
                                    break;
                                case Estado.Q1:
                                    if (caracter == 'l')
                                    {
                                        estadoActual = Estado.Q2;
                                    }
                                    break;
                                case Estado.Q2:
                                    if (caracter == 'a')
                                    {
                                        estadoActual = Estado.Q3;
                                    }
                                    break;
                                case Estado.Q3:
                                    if (caracter == 's')
                                    {
                                        estadoActual = Estado.Q4;
                                    }
                                    break;
                                case Estado.Q4:
                                    if (caracter == 's')
                                    {
                                        estadoActual = Estado.QF;
                                    }
                                    break;
                            }
                        }
                        break;
                    case "case":
                        foreach (char caracter in token)
                        {
                            switch (estadoActual)
                            {
                                case Estado.Q0:
                                    if (caracter == 'c')
                                    {
                                        estadoActual = Estado.Q1;
                                    }
                                    break;
                                case Estado.Q1:
                                    if (caracter == 'a')
                                    {
                                        estadoActual = Estado.Q2;
                                    }
                                    break;
                                case Estado.Q2:
                                    if (caracter == 's')
                                    {
                                        estadoActual = Estado.Q3;
                                    }
                                    break;
                                case Estado.Q3:
                                    if (caracter == 'e')
                                    {
                                        estadoActual = Estado.QF;
                                    }
                                    break;
                            }
                        }
                        break;
                    case "false":
                        foreach (char caracter in token)
                        {
                            switch (estadoActual)
                            {
                                case Estado.Q0:
                                    if (caracter == 'f')
                                    {
                                        estadoActual = Estado.Q1;
                                    }
                                    break;
                                case Estado.Q1:
                                    if (caracter == 'a')
                                    {
                                        estadoActual = Estado.Q2;
                                    }
                                    break;
                                case Estado.Q2:
                                    if (caracter == 'l')
                                    {
                                        estadoActual = Estado.Q3;
                                    }
                                    break;
                                case Estado.Q3:
                                    if (caracter == 's')
                                    {
                                        estadoActual = Estado.Q4;
                                    }
                                    break;
                                case Estado.Q4:
                                    if (caracter == 'e')
                                    {
                                        estadoActual = Estado.QF;
                                    }
                                    break;
                            }
                        }
                        break;
                    case "float":
                        foreach (char caracter in token)
                        {
                            switch (estadoActual)
                            {
                                case Estado.Q0:
                                    if (caracter == 'f')
                                    {
                                        estadoActual = Estado.Q1;
                                    }
                                    break;
                                case Estado.Q1:
                                    if (caracter == 'l')
                                    {
                                        estadoActual = Estado.Q2;
                                    }
                                    break;
                                case Estado.Q2:
                                    if (caracter == 'o')
                                    {
                                        estadoActual = Estado.Q3;
                                    }
                                    break;
                                case Estado.Q3:
                                    if (caracter == 'a')
                                    {
                                        estadoActual = Estado.Q4;
                                    }
                                    break;
                                case Estado.Q4:
                                    if (caracter == 't')
                                    {
                                        estadoActual = Estado.QF;
                                    }
                                    break;
                            }
                        }
                        break;
                    case "final":
                        foreach (char caracter in token)
                        {
                            switch (estadoActual)
                            {
                                case Estado.Q0:
                                    if (caracter == 'f')
                                    {
                                        estadoActual = Estado.Q1;
                                    }
                                    break;
                                case Estado.Q1:
                                    if (caracter == 'i')
                                    {
                                        estadoActual = Estado.Q2;
                                    }
                                    break;
                                case Estado.Q2:
                                    if (caracter == 'n')
                                    {
                                        estadoActual = Estado.Q3;
                                    }
                                    break;
                                case Estado.Q3:
                                    if (caracter == 'a')
                                    {
                                        estadoActual = Estado.Q4;
                                    }
                                    break;
                                case Estado.Q4:
                                    if (caracter == 'l')
                                    {
                                        estadoActual = Estado.QF;
                                    }
                                    break;
                            }
                        }
                        break;
                    case "then":
                        foreach (char caracter in token)
                        {
                            switch (estadoActual)
                            {
                                case Estado.Q0:
                                    if (caracter == 't')
                                    {
                                        estadoActual = Estado.Q1;
                                    }
                                    break;
                                case Estado.Q1:
                                    if (caracter == 'h')
                                    {
                                        estadoActual = Estado.Q2;
                                    }
                                    break;
                                case Estado.Q2:
                                    if (caracter == 'e')
                                    {
                                        estadoActual = Estado.Q3;
                                    }
                                    break;
                                case Estado.Q3:
                                    if (caracter == 'n')
                                    {
                                        estadoActual = Estado.QF;
                                    }
                                    break;
                            }
                        }
                        break;
                    case "true":
                        foreach (char caracter in token)
                        {
                            switch (estadoActual)
                            {
                                case Estado.Q0:
                                    if (caracter == 't')
                                    {
                                        estadoActual = Estado.Q1;
                                    }
                                    break;
                                case Estado.Q1:
                                    if (caracter == 'r')
                                    {
                                        estadoActual = Estado.Q2;
                                    }
                                    break;
                                case Estado.Q2:
                                    if (caracter == 'u')
                                    {
                                        estadoActual = Estado.Q3;
                                    }
                                    break;
                                case Estado.Q3:
                                    if (caracter == 'e')
                                    {
                                        estadoActual = Estado.QF;
                                    }
                                    break;
                            }
                        }
                        break;
                    case "else":
                        foreach (char caracter in token)
                        {
                            switch (estadoActual)
                            {
                                case Estado.Q0:
                                    if (caracter == 'e')
                                    {
                                        estadoActual = Estado.Q1;
                                    }
                                    break;
                                case Estado.Q1:
                                    if (caracter == 'l')
                                    {
                                        estadoActual = Estado.Q2;
                                    }
                                    break;
                                case Estado.Q2:
                                    if (caracter == 's')
                                    {
                                        estadoActual = Estado.Q3;
                                    }
                                    break;
                                case Estado.Q3:
                                    if (caracter == 'e')
                                    {
                                        estadoActual = Estado.QF;
                                    }
                                    break;
                            }
                        }
                        break;
                    case "extends":
                        foreach (char caracter in token)
                        {
                            switch (estadoActual)
                            {
                                case Estado.Q0:
                                    if (caracter == 'e')
                                    {
                                        estadoActual = Estado.Q1;
                                    }
                                    break;
                                case Estado.Q1:
                                    if (caracter == 'x')
                                    {
                                        estadoActual = Estado.Q2;
                                    }
                                    break;
                                case Estado.Q2:
                                    if (caracter == 't')
                                    {
                                        estadoActual = Estado.Q3;
                                    }
                                    break;
                                case Estado.Q3:
                                    if (caracter == 'e')
                                    {
                                        estadoActual = Estado.Q4;
                                    }
                                    break;
                                case Estado.Q4:
                                    if (caracter == 'n')
                                    {
                                        estadoActual = Estado.Q5;
                                    }
                                    break;
                                case Estado.Q5:
                                    if (caracter == 'd')
                                    {
                                        estadoActual = Estado.Q6;
                                    }
                                    break;
                                case Estado.Q6:
                                    if (caracter == 's')
                                    {
                                        estadoActual = Estado.QF;
                                    }
                                    break;
                            }
                        }
                        break;
                    case "public":
                        foreach (char caracter in token)
                        {
                            switch (estadoActual)
                            {
                                case Estado.Q0:
                                    if (caracter == 'p')
                                    {
                                        estadoActual = Estado.Q1;
                                    }
                                    break;
                                case Estado.Q1:
                                    if (caracter == 'u')
                                    {
                                        estadoActual = Estado.Q2;
                                    }
                                    break;
                                case Estado.Q2:
                                    if (caracter == 'b')
                                    {
                                        estadoActual = Estado.Q3;
                                    }
                                    break;
                                case Estado.Q3:
                                    if (caracter == 'l')
                                    {
                                        estadoActual = Estado.Q4;
                                    }
                                    break;
                                case Estado.Q4:
                                    if (caracter == 'i')
                                    {
                                        estadoActual = Estado.Q5;
                                    }
                                    break;
                                case Estado.Q5:
                                    if (caracter == 'c')
                                    {
                                        estadoActual = Estado.QF;
                                    }
                                    break;
                            }
                        }
                        break;
                    case "private":
                        foreach (char caracter in token)
                        {
                            switch (estadoActual)
                            {
                                case Estado.Q0:
                                    if (caracter == 'p')
                                    {
                                        estadoActual = Estado.Q1;
                                    }
                                    break;
                                case Estado.Q1:
                                    if (caracter == 'r')
                                    {
                                        estadoActual = Estado.Q2;
                                    }
                                    break;
                                case Estado.Q2:
                                    if (caracter == 'i')
                                    {
                                        estadoActual = Estado.Q3;
                                    }
                                    break;
                                case Estado.Q3:
                                    if (caracter == 'v')
                                    {
                                        estadoActual = Estado.Q4;
                                    }
                                    break;
                                case Estado.Q4:
                                    if (caracter == 'a')
                                    {
                                        estadoActual = Estado.Q5;
                                    }
                                    break;
                                case Estado.Q5:
                                    if (caracter == 't')
                                    {
                                        estadoActual = Estado.Q6;
                                    }
                                    break;
                                case Estado.Q6:
                                    if (caracter == 'e')
                                    {
                                        estadoActual = Estado.QF;
                                    }
                                    break;
                            }
                        }
                        break;
                    case "package":
                        foreach (char caracter in token)
                        {
                            switch (estadoActual)
                            {
                                case Estado.Q0:
                                    if (caracter == 'p')
                                    {
                                        estadoActual = Estado.Q1;
                                    }
                                    break;
                                case Estado.Q1:
                                    if (caracter == 'a')
                                    {
                                        estadoActual = Estado.Q2;
                                    }
                                    break;
                                case Estado.Q2:
                                    if (caracter == 'c')
                                    {
                                        estadoActual = Estado.Q3;
                                    }
                                    break;
                                case Estado.Q3:
                                    if (caracter == 'k')
                                    {
                                        estadoActual = Estado.Q4;
                                    }
                                    break;
                                case Estado.Q4:
                                    if (caracter == 'a')
                                    {
                                        estadoActual = Estado.Q5;
                                    }
                                    break;
                                case Estado.Q5:
                                    if (caracter == 'g')
                                    {
                                        estadoActual = Estado.Q6;
                                    }
                                    break;
                                case Estado.Q6:
                                    if (caracter == 'e')
                                    {
                                        estadoActual = Estado.QF;
                                    }
                                    break;
                            }
                        }
                        break;
                    case "protected":
                        foreach (char caracter in token)
                        {
                            switch (estadoActual)
                            {
                                case Estado.Q0:
                                    if (caracter == 'p')
                                    {
                                        estadoActual = Estado.Q1;
                                    }
                                    break;
                                case Estado.Q1:
                                    if (caracter == 'r')
                                    {
                                        estadoActual = Estado.Q2;
                                    }
                                    break;
                                case Estado.Q2:
                                    if (caracter == 'o')
                                    {
                                        estadoActual = Estado.Q3;
                                    }
                                    break;
                                case Estado.Q3:
                                    if (caracter == 't')
                                    {
                                        estadoActual = Estado.Q4;
                                    }
                                    break;
                                case Estado.Q4:
                                    if (caracter == 'e')
                                    {
                                        estadoActual = Estado.Q5;
                                    }
                                    break;
                                case Estado.Q5:
                                    if (caracter == 'c')
                                    {
                                        estadoActual = Estado.Q6;
                                    }
                                    break;
                                case Estado.Q6:
                                    if (caracter == 't')
                                    {
                                        estadoActual = Estado.Q7;
                                    }
                                    break;
                                case Estado.Q7:
                                    if (caracter == 'e')
                                    {
                                        estadoActual = Estado.Q8;
                                    }
                                    break;
                                case Estado.Q8:
                                    if (caracter == 'd')
                                    {
                                        estadoActual = Estado.QF;
                                    }
                                    break;
                            }
                        }
                        break;
                    case "return":
                        foreach (char caracter in token)
                        {
                            switch (estadoActual)
                            {
                                case Estado.Q0:
                                    if (caracter == 'r')
                                    {
                                        estadoActual = Estado.Q1;
                                    }
                                    break;
                                case Estado.Q1:
                                    if (caracter == 'e')
                                    {
                                        estadoActual = Estado.Q2;
                                    }
                                    break;
                                case Estado.Q2:
                                    if (caracter == 't')
                                    {
                                        estadoActual = Estado.Q3;
                                    }
                                    break;
                                case Estado.Q3:
                                    if (caracter == 'u')
                                    {
                                        estadoActual = Estado.Q4;
                                    }
                                    break;
                                case Estado.Q4:
                                    if (caracter == 'r')
                                    {
                                        estadoActual = Estado.Q5;
                                    }
                                    break;
                                case Estado.Q5:
                                    if (caracter == 'n')
                                    {
                                        estadoActual = Estado.QF;
                                    }
                                    break;
                            }
                        }
                        break;
                    case "while":
                        foreach (char caracter in token)
                        {
                            switch (estadoActual)
                            {
                                case Estado.Q0:
                                    if (caracter == 'w')
                                    {
                                        estadoActual = Estado.Q1;
                                    }
                                    break;
                                case Estado.Q1:
                                    if (caracter == 'h')
                                    {
                                        estadoActual = Estado.Q2;
                                    }
                                    break;
                                case Estado.Q2:
                                    if (caracter == 'i')
                                    {
                                        estadoActual = Estado.Q3;
                                    }
                                    break;
                                case Estado.Q3:
                                    if (caracter == 'l')
                                    {
                                        estadoActual = Estado.Q4;
                                    }
                                    break;
                                case Estado.Q4:
                                    if (caracter == 'e')
                                    {
                                        estadoActual = Estado.QF;
                                    }
                                    break;
                            }
                        }
                        break;
                    case "static":
                        foreach (char caracter in token)
                        {
                            switch (estadoActual)
                            {
                                case Estado.Q0:
                                    if (caracter == 's')
                                    {
                                        estadoActual = Estado.Q1;
                                    }
                                    break;
                                case Estado.Q1:
                                    if (caracter == 't')
                                    {
                                        estadoActual = Estado.Q2;
                                    }
                                    break;
                                case Estado.Q2:
                                    if (caracter == 'a')
                                    {
                                        estadoActual = Estado.Q3;
                                    }
                                    break;
                                case Estado.Q3:
                                    if (caracter == 't')
                                    {
                                        estadoActual = Estado.Q4;
                                    }
                                    break;
                                case Estado.Q4:
                                    if (caracter == 'i')
                                    {
                                        estadoActual = Estado.Q5;
                                    }
                                    break;
                                case Estado.Q5:
                                    if (caracter == 'c')
                                    {
                                        estadoActual = Estado.QF;
                                    }
                                    break;
                            }
                        }
                        break;
                    case "short":
                        foreach (char caracter in token)
                        {
                            switch (estadoActual)
                            {
                                case Estado.Q0:
                                    if (caracter == 's')
                                    {
                                        estadoActual = Estado.Q1;
                                    }
                                    break;
                                case Estado.Q1:
                                    if (caracter == 'h')
                                    {
                                        estadoActual = Estado.Q2;
                                    }
                                    break;
                                case Estado.Q2:
                                    if (caracter == 'o')
                                    {
                                        estadoActual = Estado.Q3;
                                    }
                                    break;
                                case Estado.Q3:
                                    if (caracter == 'r')
                                    {
                                        estadoActual = Estado.Q4;
                                    }
                                    break;
                                case Estado.Q4:
                                    if (caracter == 't')
                                    {
                                        estadoActual = Estado.QF;
                                    }
                                    break;
                            }
                        }
                        break;
                    case "void":
                        foreach (char caracter in token)
                        {
                            switch (estadoActual)
                            {
                                case Estado.Q0:
                                    if (caracter == 'v')
                                    {
                                        estadoActual = Estado.Q1;
                                    }
                                    break;
                                case Estado.Q1:
                                    if (caracter == 'o')
                                    {
                                        estadoActual = Estado.Q2;
                                    }
                                    break;
                                case Estado.Q2:
                                    if (caracter == 'i')
                                    {
                                        estadoActual = Estado.Q3;
                                    }
                                    break;
                                case Estado.Q3:
                                    if (caracter == 'd')
                                    {
                                        estadoActual = Estado.QF;
                                    }
                                    break;
                            }
                        }
                        break;
                    case "boolean":
                        foreach (char caracter in token)
                        {
                            switch (estadoActual)
                            {
                                case Estado.Q0:
                                    if (caracter == 'b')
                                    {
                                        estadoActual = Estado.Q1;
                                    }
                                    break;
                                case Estado.Q1:
                                    if (caracter == 'o')
                                    {
                                        estadoActual = Estado.Q2;
                                    }
                                    break;
                                case Estado.Q2:
                                    if (caracter == 'o')
                                    {
                                        estadoActual = Estado.Q3;
                                    }
                                    break;
                                case Estado.Q3:
                                    if (caracter == 'l')
                                    {
                                        estadoActual = Estado.Q4;
                                    }
                                    break;
                                case Estado.Q4:
                                    if (caracter == 'e')
                                    {
                                        estadoActual = Estado.Q5;
                                    }
                                    break;
                                case Estado.Q5:
                                    if (caracter == 'a')
                                    {
                                        estadoActual = Estado.Q6;
                                    }
                                    break;
                                case Estado.Q6:
                                    if (caracter == 'n')
                                    {
                                        estadoActual = Estado.QF;
                                    }
                                    break;
                            }
                        }
                        break;
                    default:
                        estadoActual = Estado.Q0;
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al analizar el caracter: {ex.Message}");
            }
            return estadoActual;
        }
        public Token ProcesarPalabraReservada(string token, int fila, int columna)
        {
            try
            {
                var estadoFinal = AnalizarCaracter(token);

                if (estadoFinal == Estado.QF)
                {
                    return new Token("Palabra Reservada", token, fila, columna);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al procesar la palabra reservada: {ex.Message}");
            }
            return null!;
        }
    }
}
