using Logica;

namespace Analizadores
{
    public class AnalizadorComentarioBloque
    {
        private Estado estadoActual;

        public AnalizadorComentarioBloque()
        {
            estadoActual = Estado.Q0; // Estado inicial
        }

        public Estado AnalizarCaracter(char caracter, char? siguienteCaracter)
        {
            try
            {
                switch (estadoActual)
                {
                    case Estado.Q0:
                        if (caracter == '/' && siguienteCaracter == '*')
                        {
                            estadoActual = Estado.Q1; // Inicio del comentario en bloque
                        }
                        break;

                    case Estado.Q1:
                        if (caracter == '*' && siguienteCaracter == '/')
                        {
                            estadoActual = Estado.QF; // Fin del comentario en bloque
                        }
                        break;

                    case Estado.QF:
                        // No hay más transiciones desde el estado final
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error al analizar el caracter: {ex.Message}");
            }

            return estadoActual;
        }

        public Token ProcesarComentarioBloque(string texto, int fila, int columna)
        {
            try
            {
                estadoActual = Estado.Q0; // Reiniciar al estado inicial
                int longitud = texto.Length;

                for (int i = 0; i < longitud; i++)
                {
                    char caracter = texto[i];
                    char? siguienteCaracter = i + 1 < longitud ? texto[i + 1] : null;

                    AnalizarCaracter(caracter, siguienteCaracter);

                    if (estadoActual == Estado.QF)
                    {
                        // El comentario es válido
                        return new Token("ComentarioBloque", texto.Substring(0, i + 2), fila, columna);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al procesar el comentario en bloque: {ex.Message}");
            }

            // Si no llega al estado de aceptación, no es un comentario válido
            return null!;
        }
    }
}
