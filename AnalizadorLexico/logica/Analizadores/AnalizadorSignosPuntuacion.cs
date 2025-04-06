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
            try
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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al analizar el caracter: {ex.Message}");
            }

            return estadoActual;
        }

        public Token ProcesarSignoPuntuacion(string token, int fila, int columna)
        {
            try
            {
                estadoActual = Estado.Q0; // Reiniciar al estado inicial

                if (token.Length == 1) // Los signos de puntuación son de un solo carácter
                {
                    char caracter = token[0];
                    AnalizarCaracter(caracter);

                    if (estadoActual == Estado.QF)
                    {
                        return new Token("Signo Puntuacion", token, fila, columna);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al procesar el signo de puntuacion: {ex.Message}");
            }

            // Si no llega al estado de aceptación, no es un signo de puntuación válido
            return null!;
        }
    }
}
