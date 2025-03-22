using Logica;

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
