using Logica;

namespace Analizadores
{
    public class AutomataRelacionalAsignacion
    {
        private Estado estadoActual;

        public AutomataRelacionalAsignacion()
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
                        if (caracter == '<' || caracter == '>')
                        {
                            estadoActual = Estado.Q1; // Puede ser un operador relacional simple o compuesto
                        }
                        else if (caracter == '=')
                        {
                            estadoActual = Estado.QF_A; // Operador de asignación
                        }
                        else
                        {
                            estadoActual = Estado.Q0; // Operador inválido
                        }
                        break;

                    case Estado.Q1:
                        if (caracter == '=') // Combinaciones válidas: <= o >=
                        {
                            estadoActual = Estado.QF_R;
                        }
                        else if (caracter == '<' || caracter == '>') // Detecta operadores inválidos << o >>
                        {
                            estadoActual = Estado.Q0; // Operador inválido
                        }
                        else
                        {
                            estadoActual = Estado.QF_R; // < o > son operadores relacionales válidos
                        }
                        break;

                    case Estado.QF_R:
                    case Estado.QF_A:
                        // Si se intenta procesar más caracteres después del estado final, es inválido
                        estadoActual = Estado.Q0;
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al analizar el caracter: {ex.Message}");
            }

            return estadoActual;
        }

        public Token ProcesarOperador(string token, int fila, int columna)
        {
            try
            {
                estadoActual = Estado.Q0; // Reiniciar al estado inicial
                int longitud = token.Length;

                for (int i = 0; i < longitud; i++)
                {
                    AnalizarCaracter(token[i]);

                    if (estadoActual == Estado.Q0 && i < longitud - 1) // Vuelve al estado inicial antes de terminar
                    {
                        return null!; // No es válido
                    }
                }

                // Clasificar el token según el estado final
                if (estadoActual == Estado.QF_R && longitud <= 2)
                {
                    return new Token("Operador Relacional", token, fila, columna);
                }
                else if (estadoActual == Estado.QF_A && longitud == 1)
                {
                    return new Token("Operador Asignacion", token, fila, columna);
                }
                else if (estadoActual == Estado.Q1) // Asegurar que < y > sean aceptados
                {
                    return new Token("Operador Relacional", token, fila, columna);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al procesar el operador racional: {ex.Message}");
            }

            return null!; // No es un operador válido
        }
    }
}
