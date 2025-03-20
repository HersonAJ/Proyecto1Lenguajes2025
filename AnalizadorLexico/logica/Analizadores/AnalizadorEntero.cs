using Logica;

namespace Analizadores
{
    public class AnalizadorEntero
    {
        private Estado estadoActual;

        public AnalizadorEntero()
        {
            estadoActual = Estado.Q0; // Estado inicial
        }

        public Estado AnalizarCaracter(char caracter)
        {
            switch (estadoActual)
            {
                case Estado.Q0: // Estado inicial
                    if (caracter == '+' || caracter == '-') // Puede comenzar con un signo
                    {
                        estadoActual = Estado.Q1;
                    }
                    else if (char.IsDigit(caracter) && caracter != '0') // Primer dígito válido (no '0')
                    {
                        estadoActual = Estado.Q2;
                    }
                    else if (caracter == '0') // Exactamente "0" es válido
                    {
                        estadoActual = Estado.QF; // Estado de aceptación
                    }
                    else
                    {
                        estadoActual = Estado.Q0; // Permanecer en Q0 para entradas no válidas
                    }
                    break;

                case Estado.Q1: // Después de un signo
                    if (char.IsDigit(caracter) && caracter != '0') // Primer dígito válido
                    {
                        estadoActual = Estado.Q2;
                    }
                    else if (caracter == '0') // "0" inmediatamente después de un signo no es válido
                    {
                        estadoActual = Estado.Q0;
                    }
                    else
                    {
                        estadoActual = Estado.Q0; // Permanecer en estado inválido
                    }
                    break;

                case Estado.Q2: // Procesando dígitos
                    if (char.IsDigit(caracter)) // Cualquier dígito es válido
                    {
                        estadoActual = Estado.Q2; // Mantenerse en Q2
                    }
                    else
                    {
                        estadoActual = Estado.Q0; // Caracteres inválidos
                    }
                    break;

                default:
                    throw new Exception("Estado no definido en el autómata");
            }

            return estadoActual;
        }

        public Token ProcesarEntero(string token, int posicion)
        {
            estadoActual = Estado.Q0; // Reiniciar siempre al estado inicial

            foreach (char caracter in token)
            {
                AnalizarCaracter(caracter);
            }

            // Si termina en el estado de aceptación (QF o Q2), el número es válido
            if (estadoActual == Estado.QF || estadoActual == Estado.Q2)
            {
                return new Token("Entero", token, posicion);
            }

            // Si no termina en estado de aceptación, no es válido
            return null!;
        }
    }
}
