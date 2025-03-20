using Logica;

namespace Analizadores
{
    public class AnalizadorLiteral
    {
        private Estado estadoActual;
        private char comillaInicio; // Almacena si inició con ' o "

        public AnalizadorLiteral()
        {
            estadoActual = Estado.Q0; // Estado inicial
            comillaInicio = '\0'; // Sin comilla definida al inicio
        }

        public Estado AnalizarCaracter(char caracter)
        {
            switch (estadoActual)
            {
                case Estado.Q0: // Estado inicial
                    if (caracter == '"' || caracter == '\'') // Inicia con " o '
                    {
                        comillaInicio = caracter; // Guardar el tipo de comilla
                        estadoActual = Estado.Q1; // Transición al estado intermedio
                    }
                    break;

                case Estado.Q1: // Dentro del literal
                    if (caracter == '\n' || caracter == '\r') // Saltos de línea no permitidos
                    {
                        estadoActual = Estado.Q0; // Literal inválido
                    }
                    else if (caracter == comillaInicio) // Comilla de cierre
                    {
                        estadoActual = Estado.QF; // Transición al estado final
                    }
                    break;

                case Estado.QF: // Estado final
                    // Ya no se procesan más caracteres
                    break;
            }

            return estadoActual;
        }

        public Token ProcesarLiteral(string token, int posicion)
        {
            estadoActual = Estado.Q0; // Reiniciar al estado inicial
            comillaInicio = '\0'; // Limpiar la comilla inicial

            foreach (char caracter in token)
            {
                AnalizarCaracter(caracter);
                if (estadoActual == Estado.Q0) // Literal inválido
                {
                    return null!;
                }
            }

            // Solo válido si termina en el estado QF
            if (estadoActual == Estado.QF)
            {
                return new Token("Literal", token, posicion);
            }

            // Si no termina en QF, no es un literal válido
            return null!;
        }
    }
}