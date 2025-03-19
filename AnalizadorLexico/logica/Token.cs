namespace Logica
{
    public class Token
    {
        private string tipo;
        private string valor;
        private int posicion;

        // Constructor
        public Token(string tipo, string valor, int posicion)
        {
            this.tipo = tipo;
            this.valor = valor;
            this.posicion = posicion;
        }

        public string Tipo
        {
            get { return tipo; }
            set { tipo = value; }
        }

        public string Valor
        {
            get { return valor; }
            set { valor = value; }
        }

        public int Posicion
        {
            get { return posicion; }
            set { posicion = value; }
        }

        // Método para representar como cadena el token
        public override string ToString()
        {
            return $"Token: {tipo}, Valor: {valor}, Posición: {posicion}";
        }
    }
}