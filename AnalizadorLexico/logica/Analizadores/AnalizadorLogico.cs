using Logica;

namespace Analizadores
{
    public class AnalizadorLogico
    {
        private Estado estadoActual;

        public AnalizadorLogico()
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
                        if (caracter == 'A')
                        {
                            estadoActual = Estado.Q1; // Posible inicio de AND
                        }
                        else if (caracter == 'O')
                        {
                            estadoActual = Estado.Q1; // Posible inicio de OR
                        }
                        else if (caracter == '&')
                        {
                            estadoActual = Estado.Q1; // Posible inicio de &&
                        }
                        else if (caracter == '|')
                        {
                            estadoActual = Estado.Q1; // Posible inicio de ||
                        }
                        else
                        {
                            estadoActual = Estado.Q0; // Operador inválido
                        }
                        break;

                    case Estado.Q1:
                        if (caracter == 'N') // Posible AN para AND
                        {
                            estadoActual = Estado.Q2;
                        }
                        else if (caracter == 'R') // Posible OR
                        {
                            estadoActual = Estado.QF; // Operador lógico OR completo
                        }
                        else if (caracter == '&') // Posible &&
                        {
                            estadoActual = Estado.QF; // Operador lógico && completo
                        }
                        else if (caracter == '|') // Posible ||
                        {
                            estadoActual = Estado.QF; // Operador lógico || completo
                        }
                        else
                        {
                            estadoActual = Estado.Q0; // Invalido si no coincide con nada
                        }
                        break;

                    case Estado.Q2:
                        if (caracter == 'D') // Completa AND
                        {
                            estadoActual = Estado.QF;
                        }
                        else
                        {
                            estadoActual = Estado.Q0; // Operador inválido
                        }
                        break;

                    case Estado.QF:
                        // Estado final: cualquier carácter adicional lo invalida
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

        public Token ProcesarOperadorLogico(string token, int fila, int columna)
        {
            try
            {
                estadoActual = Estado.Q0; // Reiniciar al estado inicial

                foreach (char caracter in token)
                {
                    AnalizarCaracter(caracter);

                    if (estadoActual == Estado.Q0) // Si vuelve al estado inicial, no es válido
                    {
                        return null!; // No es un operador lógico válido
                    }
                }

                // Solo es válido si termina en el estado de aceptación QF
                if (estadoActual == Estado.QF)
                {
                    return new Token("OperadorLogico", token, fila, columna);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al procesar el operador logico: {ex.Message}");
            }

            return null!; // No es un operador lógico válido
        }
    }
}
