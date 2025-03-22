using Logica;

namespace Analizadores
{
    public class AnalizadorSignosPuntuacion
    {
        private Estado estadoActual;

        public AnalizadorSignosPuntuacion()
        {
            estadoActual = Estado.Q0; // Estado inicial
        }

        public Estado AnalizarCaracter(char caracter)
        {
            switch (estadoActual)
            {
                case Estado.Q0:
                    if (caracter == '.' || caracter == ',' || caracter == ';' || caracter == ':')
                    {
                        estadoActual = Estado.QF; // Transición al estado de aceptación
                    }
                    break;

                case Estado.QF:
                    // En el estado final no hay más transiciones, se mantiene en QF
                    break;
            }

            return estadoActual;
        }

        public Token ProcesarSignoPuntuacion(string token, int fila, int columna)
        {
            estadoActual = Estado.Q0; // Reiniciar al estado inicial

            if (token.Length == 1) // Los signos de puntuación son de un solo carácter
            {
                char caracter = token[0];
                AnalizarCaracter(caracter);

                if (estadoActual == Estado.QF)
                {
                    return new Token("SignoPuntuacion", token, fila, columna);
                }
            }

            // Si no llega al estado de aceptación, no es un signo de puntuación válido
            return null!;
        }
    }
}
