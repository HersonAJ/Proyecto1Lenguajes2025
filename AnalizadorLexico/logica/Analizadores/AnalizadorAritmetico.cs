using Logica;

namespace Analizadores
{
    public class AnalizadorAritmetico
    {
        private Estado estadoActual;

        public AnalizadorAritmetico()
        {
            estadoActual = Estado.Q0; // Estado inicial
        }

        public Estado AnalizarCaracter(char caracter)
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

            return estadoActual;
        }

        public Token ProcesarOperadorAritmetico(string token, int posicion)
        {
            estadoActual = Estado.Q0; // Reiniciar al estado inicial

            if (token.Length == 1) // Los operadores aritméticos son de un solo carácter
            {
                char caracter = token[0];
                AnalizarCaracter(caracter);

                if (estadoActual == Estado.QF)
                {
                    return new Token("OperadorAritmetico", token, posicion);
                }
            }

            // Si no llega al estado de aceptación, no es un operador aritmético válido
            return null!;
        }
    }
}
