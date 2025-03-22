namespace Logica
{
    public class Token
    {
        private string tipo;
        private string valor;
        private int fila;
        private int columna;

        // Constructor
        public Token(string tipo, string valor, int fila, int columna)
        {
            this.tipo = tipo;
            this.valor = valor;
            this.fila = fila;
            this.columna = columna;
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

        public int Fila
        {
            get { return fila; }
            set { fila = value; }
        }

        public int Columna
        {
            get { return columna; }
            set { columna = value; }
        }

        // Método para representar como cadena el token
        public override string ToString()
        {
            return $"Token: {tipo}, Valor: {valor}, Fila: {fila}, Columna: {columna}";
        }
    }
}
