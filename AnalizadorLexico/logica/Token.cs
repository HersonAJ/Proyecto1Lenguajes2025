Token 
{
    private string tipo;
    private string valor;
    private  int posicion;

    //constructor 
    public Token (string tipo, string valor, int posicion)
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

    public string valor 
    {
        get { return valor;}
        set { valor = value; }
    }

    public int Posicion
    {
        get { return posicion;}
        set { poscion = value; }
    }

    //metodo para representar como cadena el token
    public override string ToString()
    {
        return $"Token: {tipo}, Valor: {valor}, Posicion: {posicion}";
    }

}