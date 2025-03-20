using Logica;

namespace Analizadores
{
    public class AnalizadorComentarioSimple
    {
        private Estado estadoActual;

        public AnalizadorComentarioSimple()
        {
            estadoActual = Estado.Q0; // Estado inicial
        }

        public Estado AnalizarCaracter(char caracter)
        {
            switch (estadoActual)
            {
                case Estado.Q0:
                    if (caracter == '#')
                    {
                        estadoActual = Estado.Q1; // Inicio del comentario
                    }
                    else
                    {
                        estadoActual = Estado.Q0; // No válido
                    }
                    break;

                case Estado.Q1:
                    if (caracter == '\n') // Fin de línea
                    {
                        estadoActual = Estado.QF; // Comentario válido completo
                    }
                    else
                    {
                        estadoActual = Estado.Q1; // Se mantiene en el comentario
                    }
                    break;

                case Estado.QF:
                    // Ya no hay transiciones desde el estado final
                    break;
            }

            return estadoActual;
        }

        public Token ProcesarComentarioSimple(string token, int posicion)
        {
            estadoActual = Estado.Q0; // Reiniciar al estado inicial

            foreach (char caracter in token)
            {
                AnalizarCaracter(caracter);

                if (estadoActual == Estado.Q0 && caracter != '#') // Si regresa a Q0, no es válido
                {
                    return null!; // No es un comentario válido
                }
            }

            // Solo válido si termina en el estado final QF
            if (estadoActual == Estado.QF || estadoActual == Estado.Q1)
            {
                return new Token("ComentarioSimple", token, posicion);
            }

            return null!; // No es un comentario válido
        }
    }
}
