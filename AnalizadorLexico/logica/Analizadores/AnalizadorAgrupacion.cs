using Logica;

namespace Analizadores
{
    public class AnalizadorAgrupacion
    {
        private Estado estadoActual;

        public AnalizadorAgrupacion()
        {
            estadoActual = Estado.Q0; // Estado inicial
        }

        public Estado AnalizarCaracter(char caracter)
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

            return estadoActual;
        }

        public Token ProcesarSignoAgrupacion(string token, int fila, int columna)
        {
            estadoActual = Estado.Q0; // Reiniciar al estado inicial

            if (token.Length == 1) // Los signos de agrupación son de un solo carácter
            {
                char caracter = token[0];
                AnalizarCaracter(caracter);

                if (estadoActual == Estado.QF)
                {
                    return new Token("SignoAgrupacion", token, fila, columna);
                }
            }

            // Si no llega al estado de aceptación, no es un signo de agrupación válido
            return null!;
        }
    }
}
