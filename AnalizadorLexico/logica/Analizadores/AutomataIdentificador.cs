using Logica;

namespace Analizadores
{
    public class AutomataIdentificador
    {
        private Estado estadoActual;

        public AutomataIdentificador()
        {
            estadoActual = Estado.Q0;
        }

        public Estado AnalizarCaracter(char caracter)
        {
            try
            {
                switch (estadoActual)
                {
                    case Estado.Q0:
                        if (caracter == '$')
                        {
                            estadoActual = Estado.Q1; // Transición al estado Q1 al encontrar el carácter '$'
                        }
                        else
                        {
                            estadoActual = Estado.Q0; // Estado inválido si no comienza con '$'
                        }
                        break;

                    case Estado.Q1:
                        if (char.IsLetter(caracter) || char.IsDigit(caracter) || caracter == '_' || caracter == '-')
                        {
                            estadoActual = Estado.Q2; // Transición al estado Q2 al encontrar una letra, dígito, '_' o '-'
                        }
                        else
                        {
                            estadoActual = Estado.Q0; // Estado inválido
                        }
                        break;

                    case Estado.Q2:
                        if (char.IsLetterOrDigit(caracter) || caracter == '_' || caracter == '-')
                        {
                            estadoActual = Estado.Q2; // Permanecer en Q2 si el carácter es válido
                        }
                        else
                        {
                            estadoActual = Estado.Q0; // Estado inválido si el carácter no es válido
                        }
                        break;

                    default:
                        throw new Exception("Estado no definido en el autómata");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al analizar el carácter: {ex.Message}");
            }

            return estadoActual;
        }

        public Token ProcesarIdentificador(string token, int fila, int columna)
        {
            try
            {
                estadoActual = Estado.Q0; // Reiniciar siempre al estado inicial

                foreach (char caracter in token)
                {
                    AnalizarCaracter(caracter);

                    // Si en algún momento el estado vuelve a Q0, el identificador no es válido
                    if (estadoActual == Estado.Q0)
                    {
                        return null!;
                    }
                }

                // Si el estado final es Q2, el identificador es válido
                if (estadoActual == Estado.Q2)
                {
                    estadoActual = Estado.QF; // Transición al estado final
                    return new Token("Identificador", token, fila, columna);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al procesar el identificador: {ex.Message}");
            }

            // Retornar null si no es un identificador válido
            return null!;
        }
    }
}
