namespace Analizadores
{

    public class AnalizadorIdentificador
    {
        private Estado estadoActual;

        public AnalizadorIdentificador()
        {
            estadoActual = Estado.Q0;
        }

        public Estado AnalizarCaracter(char caracter)
        {

            switch (estadoActual)
            {
                case Estado.Q0:
                    if (caracter == '$')
                    {
                        estadoActual = Estado.Q1;
                    }
                    break;
                case Estado.Q1:
                    if (char.IsLetter(caracter))
                    {
                        estadoActual = Estado.Q2;
                    }
                    break;
                case Estado.Q2:
                    if (char.IsLetterOrDigit(caracter) || caracter == '_' || caracter == '-')
                    {
                        estadoActual = Estado.Q2;
                    }
                    break;
                default:
                    throw new Exception("Estado no definido en el autómata");
            }

            return estadoActual;
        }

        public bool EsIdentificadorValido(string token)
        {
            estadoActual = Estado.Q0; // Reiniciar siempre el estado inicial antes de procesar un token.

            foreach (char caracter in token)
            {
                AnalizarCaracter(caracter);
            }

            // Imprimir estado final
            return estadoActual == Estado.Q2;
        }
    }
}