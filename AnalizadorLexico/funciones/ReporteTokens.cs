using Gtk;
using System;
using System.Collections.Generic;
using Logica;

public class ReporteTokens
{
    private readonly List<Token> tokens; // Lista de tokens para el reporte  
    private Window ventanaReporte;         // Ventana donde se mostrará el reporte  
    private TreeView tablaTokens;          // Tabla visual para mostrar los tokens

    // Constructor que recibe la lista de tokens
    public ReporteTokens(List<Token> tokens)
    {
        // Asignar campos no anulables para satisfacer las restricciones del compilador
        this.tokens = tokens ?? throw new ArgumentNullException(nameof(tokens));
        ventanaReporte = new Window("Reporte de Tokens")
        {
            DefaultWidth = 600,
            DefaultHeight = 400,
            BorderWidth = 10 // Espaciado interno opcional
        };
        tablaTokens = new TreeView();

        try
        {
            Console.WriteLine("Constructor de ReporteTokens llamado."); // Log
            Console.WriteLine("Ventana del reporte creada."); // Log
            CrearVentana();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Ocurrió un error en el constructor de ReporteTokens: {ex.Message}");
        }
    }

    // Método para crear la ventana del reporte
    private void CrearVentana()
    {
        try
        {
            Console.WriteLine("CrearVentana llamado."); // Log

            // Crear el contenedor principal
            Box contenedorPrincipal = new Box(Orientation.Vertical, 5);
            Console.WriteLine("Contenedor principal creado."); // Log

            // Configurar las columnas de la tabla:
            // 0. Nombre del Token
            // 1. Lexema
            // 2. Fila
            // 3. Columna
            AgregarColumna("Nombre del Token", 0);
            AgregarColumna("Lexema", 1);
            AgregarColumna("Fila", 2);
            AgregarColumna("Columna", 3);
            Console.WriteLine("Columnas de la tabla configuradas."); // Log

            // Crear el modelo (ListStore) para la tabla con 4 columnas de tipo string
            ListStore modelo = new ListStore(typeof(string), typeof(string), typeof(string), typeof(string));
            Console.WriteLine("Modelo de la tabla creado."); // Log

            // Llenar el modelo con los datos de los tokens
            foreach (var token in tokens)
            {
                modelo.AppendValues(token.Tipo, token.Valor, token.Fila.ToString(), token.Columna.ToString());
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

            // Agregar el scroll al contenedor principal
            contenedorPrincipal.PackStart(scroll, true, true, 0);

            // Agregar el contenedor principal a la ventana
            ventanaReporte.Child = contenedorPrincipal;
            Console.WriteLine("Contenedor principal agregado a la ventana."); // Log
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Ocurrió un error al crear la ventana del reporte: {ex.Message}");
        }
    }

    // Método para agregar una columna a la tabla
    private void AgregarColumna(string titulo, int indice)
    {
        try
        {
            Console.WriteLine($"Agregando columna: {titulo}"); // Log
            CellRendererText celdaTexto = new CellRendererText();
            TreeViewColumn columna = new TreeViewColumn(titulo, celdaTexto, "text", indice);
            tablaTokens.AppendColumn(columna);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Ocurrió un error al agregar la columna '{titulo}': {ex.Message}");
        }
    }

    // Método para mostrar la ventana del reporte
    public void MostrarReporte()
    {
        try
        {
            Console.WriteLine("MostrarReporte llamado."); // Log

            if (ventanaReporte != null)
            {
                Console.WriteLine("Mostrando ventana del reporte..."); // Log
                ventanaReporte.Present(); // Llevar la ventana al frente
                ventanaReporte.ShowAll();
            }
            else
            {
                Console.WriteLine("Error: La ventana del reporte no está inicializada."); // Log
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Ocurrió un error al mostrar el reporte: {ex.Message}");
        }
    }
}
