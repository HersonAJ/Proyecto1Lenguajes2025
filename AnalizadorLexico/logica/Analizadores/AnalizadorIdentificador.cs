using Logica;

namespace Analizadores
{
    public class AnalizadorIdentificador
    {
        private Estado estadoActual;

        public AnalizadorIdentificador()
        {
            estadoActual = Estado.Q0;
        }

        public Estado AnalizarCaracter(char caracter)
        {
            switch (estadoActual)
            {
                case Estado.Q0:
                    if (caracter == '$')
                    {
                        estadoActual = Estado.Q1; // Transición al estado Q1 al encontrar el carácter '$'
                    }
                    break;

                case Estado.Q1:
                    if (char.IsLetter(caracter))
                    {
                        estadoActual = Estado.Q2; // Transición al estado Q2 al encontrar una letra
                    }
                    else
                    {
                        estadoActual = Estado.Q0; // Volver al estado inicial si el siguiente carácter no es válido
                    }
                    break;

                case Estado.Q2:
                    if (char.IsLetterOrDigit(caracter) || caracter == '_' || caracter == '-')
                    {
                        estadoActual = Estado.Q2; // Permanecer en Q2 si el carácter es válido
                    }
                    else
                    {
                        estadoActual = Estado.Q0; // Volver al estado inicial si el carácter no es válido
                    }
                    break;

                default:
                    throw new Exception("Estado no definido en el autómata");
            }

            return estadoActual;
        }

        public Token ProcesarIdentificador(string token, int posicion)
        {
            estadoActual = Estado.Q0; // Reiniciar siempre al estado inicial

            foreach (char caracter in token)
            {
                AnalizarCaracter(caracter);
            }

            // Si el token es válido, retornar un objeto Token
            if (estadoActual == Estado.Q2 || estadoActual == Estado.QF)
            {
                estadoActual = Estado.QF; // Asegurar que se alcance QF
                return new Token("Identificador", token, posicion);
            }

            // Retornar null si no es un identificador válido
            return null!;
        }
    }
}
