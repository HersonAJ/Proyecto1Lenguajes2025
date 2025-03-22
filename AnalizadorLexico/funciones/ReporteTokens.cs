using Gtk;
using System;
using System.Collections.Generic;
using Logica;

public class ReporteTokens
{
    private readonly List<Token> tokens; // Lista de tokens para el reporte
    private Window ventanaReporte; // Ventana donde se mostrará el reporte
    private TreeView tablaTokens; // Tabla visual para mostrar los tokens

    // Constructor que recibe la lista de tokens
    public ReporteTokens(List<Token> tokens)
    {
        Console.WriteLine("Constructor de ReporteTokens llamado."); // Log
        this.tokens = tokens;
        ventanaReporte = new Window("Reporte de Tokens")
        {
            DefaultWidth = 600,
            DefaultHeight = 400,
            BorderWidth = 10 // Espaciado interno opcional
        };
        Console.WriteLine("Ventana del reporte creada."); // Log
        tablaTokens = new TreeView();
        CrearVentana();
    }

    // Método para crear la ventana del reporte
    private void CrearVentana()
    {
        Console.WriteLine("CrearVentana llamado."); // Log

        // Crear el contenedor principal
        Box contenedorPrincipal = new Box(Orientation.Vertical, 5);
        Console.WriteLine("Contenedor principal creado."); // Log

        // Configurar las columnas de la tabla
        AgregarColumna("Nombre del Token", 0);
        AgregarColumna("Lexema", 1);
        AgregarColumna("Posición", 2);
        Console.WriteLine("Columnas de la tabla configuradas."); // Log

        // Crear el modelo (ListStore) para la tabla
        ListStore modelo = new ListStore(typeof(string), typeof(string), typeof(string));
        Console.WriteLine("Modelo de la tabla creado."); // Log

        // Llenar el modelo con los datos de los tokens
        foreach (var token in tokens)
        {
            string posicion = $"Fila: {token.Fila}, Columna: {token.Columna}";
            modelo.AppendValues(token.Tipo, token.Valor, posicion);
        }
        Console.WriteLine("Modelo de la tabla llenado con tokens."); // Log

        // Asignar el modelo a la tabla
        tablaTokens.Model = modelo;
        Console.WriteLine("Modelo asignado a la tabla."); // Log

        // Agregar la tabla a un contenedor con scroll
        ScrolledWindow scroll = new ScrolledWindow
        {
            Child = tablaTokens
        };
        Console.WriteLine("ScrolledWindow creado y tabla agregada."); // Log

        // Agregar el scroll al contenedor principal
        contenedorPrincipal.PackStart(scroll, true, true, 0);
        Console.WriteLine("Scroll agregado al contenedor principal."); // Log

        // Agregar el contenedor principal a la ventana
        ventanaReporte.Child = contenedorPrincipal;
        Console.WriteLine("Contenedor principal agregado a la ventana."); // Log
    }

    // Método para agregar una columna a la tabla
    private void AgregarColumna(string titulo, int indice)
    {
        Console.WriteLine($"Agregando columna: {titulo}"); // Log
        CellRendererText celdaTexto = new CellRendererText();
        TreeViewColumn columna = new TreeViewColumn(titulo, celdaTexto, "text", indice);
        tablaTokens.AppendColumn(columna);
    }

    // Método para mostrar la ventana del reporte
public void MostrarReporte()
{
    Console.WriteLine("MostrarReporte llamado."); // Log

    if (ventanaReporte != null)
    {
        Console.WriteLine("Mostrando ventana del reporte..."); // Log
        ventanaReporte.Present(); // Forzar que la ventana se traiga al frente
        ventanaReporte.ShowAll();
    }
    else
    {
        Console.WriteLine("Error: La ventana del reporte no está inicializada."); // Log
    }
}
}