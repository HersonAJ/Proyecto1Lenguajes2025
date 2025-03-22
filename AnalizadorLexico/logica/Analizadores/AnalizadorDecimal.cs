using Logica;

namespace Analizadores
{
    public class AnalizadorDecimal
    {
        private Estado estadoActual;

        public AnalizadorDecimal()
        {
            estadoActual = Estado.Q0; // Estado inicial
        }

        public Estado AnalizarCaracter(char caracter)
        {
            switch (estadoActual)
            {
                case Estado.Q0: // Estado inicial
                    if (caracter == '+' || caracter == '-') // Puede empezar con un signo
                    {
                        estadoActual = Estado.Q1;
                    }
                    else if (char.IsDigit(caracter) && caracter != '0') // Dígito distinto de '0'
                    {
                        estadoActual = Estado.Q2;
                    }
                    else if (caracter == '0') // Puede ser "0" pero debe ir seguido de un punto
                    {
                        estadoActual = Estado.Q3;
                    }
                    else
                    {
                        estadoActual = Estado.Q0; // Entrada inválida
                    }
                    break;

                case Estado.Q1: // Después del signo
                    if (char.IsDigit(caracter) && caracter != '0') // Dígito distinto de '0'
                    {
                        estadoActual = Estado.Q2;
                    }
                    else if (caracter == '0') // "0" después del signo
                    {
                        estadoActual = Estado.Q3;
                    }
                    else
                    {
                        estadoActual = Estado.Q0; // Entrada inválida
                    }
                    break;

                case Estado.Q2: // Parte entera válida (no empieza con 0)
                    if (char.IsDigit(caracter)) // Sigue siendo un número válido
                    {
                        estadoActual = Estado.Q2;
                    }
                    else if (caracter == '.') // Punto decimal
                    {
                        estadoActual = Estado.Q4;
                    }
                    else
                    {
                        estadoActual = Estado.Q0; // Entrada inválida
                    }
                    break;

                case Estado.Q3: // "0" inicial
                    if (caracter == '.') // Punto decimal obligatorio
                    {
                        estadoActual = Estado.Q4;
                    }
                    else
                    {
                        estadoActual = Estado.Q0; // Entrada inválida
                    }
                    break;

                case Estado.Q4: // Después del punto decimal
                    if (char.IsDigit(caracter)) // Al menos un dígito en la parte decimal
                    {
                        estadoActual = Estado.Q5;
                    }
                    else
                    {
                        estadoActual = Estado.Q0; // Entrada inválida
                    }
                    break;

                case Estado.Q5: // Parte decimal válida
                    if (char.IsDigit(caracter)) // Continuar en la parte decimal
                    {
                        estadoActual = Estado.Q5;
                    }
                    else
                    {
                        estadoActual = Estado.Q0; // Entrada inválida
                    }
                    break;

                default:
                    throw new Exception("Estado no definido en el autómata");
            }

            return estadoActual;
        }

        public Token ProcesarDecimal(string token, int fila, int columna)
        {
            estadoActual = Estado.Q0; // Reiniciar siempre al estado inicial

            foreach (char caracter in token)
            {
                AnalizarCaracter(caracter);
            }

            // Si termina en el estado de aceptación, es válido
            if (estadoActual == Estado.Q5)
            {
                return new Token("Decimal", token, fila, columna);
            }

            // Si no termina en el estado válido, no es un decimal
            return null!;
        }
    }
}

