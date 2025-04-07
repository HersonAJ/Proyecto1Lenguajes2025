using Logica;

namespace Analizadores
{
    public class AutomataAritmetico
    {
        private Estado estadoActual;

        public AutomataAritmetico()
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
                        if (caracter == '^' || caracter == '*' || caracter == '/' || caracter == '+' || caracter == '-')
                        {
                            estadoActual = Estado.QF; // Transición al estado de aceptación
                        }
                        break;

                    case Estado.QF:
                        // No hay más transiciones desde el estado de aceptación
                        break;
                }
            }
            catch (Exception ex)

            {
                Console.WriteLine($"error al analizar el operador aritmetico: {ex.Message}");
            }

            return estadoActual;
        }

        public Token ProcesarOperadorAritmetico(string token, int fila, int columna)
        {
            try
            {
                estadoActual = Estado.Q0; // Reiniciar al estado inicial

                if (token.Length == 1) // Los operadores aritméticos son de un solo carácter
                {
                    char caracter = token[0];
                    AnalizarCaracter(caracter);

                    if (estadoActual == Estado.QF)
                    {
                        return new Token("Operador Aritmetico", token, fila, columna);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al procesar el operador aritmentico: {ex.Message}");
            }

            // Si no llega al estado de aceptación, no es un operador aritmético válido
            return null!;
        }
    }
}
