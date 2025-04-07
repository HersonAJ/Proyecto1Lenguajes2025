using Logica;

namespace Analizadores
{
    public class AutomataAgrupacion
    {
        private Estado estadoActual;

        public AutomataAgrupacion()
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
                        if (caracter == '(' || caracter == ')' || 
                            caracter == '[' || caracter == ']' || 
                            caracter == '{' || caracter == '}')
                        {
                            estadoActual = Estado.QF; // Transición al estado de aceptación
                        }
                        else
                        {
                            estadoActual = Estado.Q0; // Carácter no válido
                        }
                        break;

                    case Estado.QF:
                        // No hay más transiciones desde el estado final
                        break;
                }
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error al analizar el carácter: {ex.Message}");
            }

            return estadoActual;
        }

        public Token ProcesarSignoAgrupacion(string token, int fila, int columna)
        {
            try
            {
                estadoActual = Estado.Q0; // Reiniciar al estado inicial

                if (token.Length == 1) // Los signos de agrupación son de un solo carácter
                {
                    char caracter = token[0];
                    AnalizarCaracter(caracter);

                    if (estadoActual == Estado.QF)
                    {
                        return new Token("Signo Agrupacion", token, fila, columna);
                    }
                }
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error al procesar el signo de agrupación: {ex.Message}");
            }

            // Si no llega al estado de aceptación, no es un signo de agrupación válido
            return null!;
        }
    }
}
