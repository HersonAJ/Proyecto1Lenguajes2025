using Gtk;
using System;
using System.Text;
using Logica;
using Analizadores;
using Funciones;

public class Interfaz
{
    private Window mainWindow;
    private Box mainLayout;
    private MenuBar menuBar;
    private TextView textEditor;
    private TextView lineNumberArea; // Área para los números de línea
    private Label cursorPosition;
    private TextView errorArea;
    private Button analizarButton;
    private Analizador analizador;
    private AbrirArchivo abrirArchivo;
    private GuardarArchivo guardarArchivo;
    //Nuevos elementos para busqueda de patrones
    private Entry patronBusqueda;
    private Button botonBuscar;
    private Label labelCoincidencias;
    private BusquedaPatrones busquedaPatrones;

    public Interfaz()
    {
        // Inicializa GTK
        Application.Init();

        // Cargar archivo CSS
        CssProvider cssProvider = new CssProvider();
        cssProvider.LoadFromPath("style.css");
        StyleContext.AddProviderForScreen(
            Gdk.Screen.Default,
            cssProvider,
            StyleProviderPriority.Application
        );

        // Crea la ventana principal
        mainWindow = new Window("Analizador Léxico - IDE Básico");
        mainWindow.SetDefaultSize(800, 600);
        mainWindow.DeleteEvent += (o, e) => Application.Quit();

        // Contenedor principal vertical
        mainLayout = new Box(Orientation.Vertical, 5);

        // Menú superior
        menuBar = new MenuBar();
        MenuItem fileMenu = new MenuItem("Archivo");
        MenuItem editMenu = new MenuItem("Editar");
        MenuItem aboutMenuItem = new MenuItem("Acerca de");

        MenuItem reportMenu = new MenuItem("Reportes");
        Menu reportSubMenu = new Menu();
        reportMenu.Submenu = reportSubMenu;
        MenuItem generateReport = new MenuItem("Generar Reporte");
        reportSubMenu.Append(generateReport);

        // Submenús
        Menu fileSubMenu = new Menu();
        fileMenu.Submenu = fileSubMenu;
        MenuItem newFile = new MenuItem("Nuevo");
        MenuItem openFile = new MenuItem("Abrir");
        MenuItem saveFile = new MenuItem("Guardar");
        MenuItem saveAsFile = new MenuItem("Guardar como...");
        fileSubMenu.Append(newFile);
        fileSubMenu.Append(openFile);
        fileSubMenu.Append(saveFile);
        fileSubMenu.Append(saveAsFile);

        abrirArchivo = new AbrirArchivo();
        guardarArchivo = new GuardarArchivo();

        // Funcionalidad para Abrir
        openFile.Activated += (o, e) =>
        {
            try
            {
                string? rutaCargada = abrirArchivo.Abrir(textEditor); // Cargar el archivo y obtener su ruta
                if (!string.IsNullOrEmpty(rutaCargada))
                {
                    guardarArchivo.EstablecerRutaArchivo(rutaCargada); // Actualizar la ruta del archivo actual
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier error al abrir un archivo
                using (MessageDialog errorDialog = new MessageDialog(
                    mainWindow, // Establecer como padre la ventana principal
                    DialogFlags.Modal,
                    MessageType.Error,
                    ButtonsType.Ok,
                    $"Error al abrir el archivo: {ex.Message}"
                ))
                {
                    errorDialog.Run();
                }
            }
        };

        // Funcionalidad para Guardar
        saveFile.Activated += (o, e) =>
        {
            guardarArchivo.Guardar(textEditor!);
        };

        // Funcionalidad para Guardar Como
        saveAsFile.Activated += (o, e) =>
        {
            guardarArchivo.GuardarComo(textEditor!);
        };

        // Funcionalidad de "Acerca de"
        aboutMenuItem.Activated += (o, e) =>
        {
            using (MessageDialog acercaDeDialogo = new MessageDialog(
                mainWindow, // Establecer como padre la ventana principal
                DialogFlags.Modal,
                MessageType.Info,
                ButtonsType.Ok,
                "Desarrollado por:\n\nHerson Isaias Aguilar Juarez\n\nCentro Universitario de Occidente\nUniversidad de San Carlos de Guatemala\n\nProyecto: Analizador Léxico - IDE Básico"
            ))
            {
                acercaDeDialogo.Run(); // Mostrar el cuadro de diálogo
            }
        };

        // Lógica para el reporte

generateReport.Activated += (o, e) =>
{
    Console.WriteLine("Evento generateReport.Activated disparado."); // Log
    if (analizador != null && analizador.Tokens.Count > 0)
    {
        // Crear y mostrar la ventana del reporte
        ReporteTokens reporte = new ReporteTokens(analizador.Tokens);
        reporte.MostrarReporte();
    }
    else
    {
        // Mostrar mensaje si no hay tokens o si hay errores
        using (MessageDialog reportDialog = new MessageDialog(
            mainWindow,
            DialogFlags.Modal,
            MessageType.Warning,
            ButtonsType.Ok,
            "No hay tokens reconocidos o existen errores en el análisis."
        ))
        {
            reportDialog.Run();
        }
    }
};
      

        // Organizar los menús principales en el menuBar
        menuBar.Append(fileMenu); // Añadir "Archivo"
        menuBar.Append(editMenu); // Añadir "Editar"
        menuBar.Append(aboutMenuItem); // Añadir "Acerca de"
        menuBar.Append(reportMenu);

        // Añadir el menuBar al layout principal
        mainLayout.PackStart(menuBar, false, false, 0);

        // Editor de texto principal
        textEditor = new TextView();
        textEditor.StyleContext.AddClass("custom-editor");
        textEditor.Buffer.Changed += (o, e) => UpdateLineNumbers(); // Actualiza números de línea en cada cambio
        textEditor.CursorVisible = true; // Asegurar que el cursor sea visible
        ScrolledWindow editorScroll = new ScrolledWindow();
        editorScroll.Add(textEditor);

        // Crear instancia de HistorialCambios después de inicializar textEditor
        HistorialCambios historialCambios = new HistorialCambios(textEditor);

        // Submenú para "Editar"
        Menu editSubMenu = new Menu();
        editMenu.Submenu = editSubMenu;

        MenuItem undoItem = new MenuItem("Deshacer");
        undoItem.Activated += (o, e) =>
        {
            historialCambios.Deshacer();
        };

        MenuItem redoItem = new MenuItem("Rehacer");
        redoItem.Activated += (o, e) =>
        {
            historialCambios.Rehacer();
        };

        // Agregar las opciones al submenú de edición
        editSubMenu.Append(undoItem);
        editSubMenu.Append(redoItem);

        // Contenedor horizontal para la numeración de líneas y el editor
        Box editorContainer = new Box(Orientation.Horizontal, 0);

        // Área para los números de línea
        lineNumberArea = new TextView();
        lineNumberArea.Editable = false;
        lineNumberArea.WrapMode = WrapMode.None;
        lineNumberArea.StyleContext.AddClass("line-number-area");
        ScrolledWindow lineNumberScroll = new ScrolledWindow();
        lineNumberScroll.Add(lineNumberArea);

        editorContainer.PackStart(lineNumberScroll, false, false, 0);
        editorContainer.PackStart(editorScroll, true, true, 0);

        // Indicador de línea y columna
        cursorPosition = new Label("Línea: 1, Columna: 1");
        textEditor.Buffer.Changed += (o, e) =>
        {
            TextIter cursorIter = textEditor.Buffer.GetIterAtMark(textEditor.Buffer.InsertMark);
            int line = cursorIter.Line + 1;
            int column = cursorIter.LineOffset + 1;
            cursorPosition.Text = $"Línea: {line}, Columna: {column}";
        };

        // Botón para iniciar el análisis
        analizarButton = new Button("Analizar");
        analizarButton.Clicked += (o, e) =>
        {
            string texto = textEditor.Buffer.Text;
            string resultado = analizador!.AnalizarTexto(texto);
            errorArea!.Buffer.Text = resultado;
        };

        // Área de errores
        errorArea = new TextView();
        errorArea.StyleContext.AddClass("custom-error");
        errorArea.Buffer.Text = "Área de resultados...";
        errorArea.Editable = false;
        ScrolledWindow errorScroll = new ScrolledWindow();
        errorScroll.Add(errorArea);

        // Elementos para la búsqueda de patrones
        patronBusqueda = new Entry();
        patronBusqueda.PlaceholderText = "Buscar...";
        botonBuscar = new Button("Buscar");
        labelCoincidencias = new Label("Coincidencias: 0");

        // Funcionalidad para el botón Buscar
        botonBuscar.Clicked += (o, e) =>
        {
            string patron = patronBusqueda.Text;
            int coincidencias = busquedaPatrones!.BuscarYResaltar(patron);
            labelCoincidencias.Text = $"Coincidencias: {coincidencias}";
        };

        // Contenedor horizontal para la búsqueda, botón analizar y posicion
        Box botonBusquedaPosicionContainer = new Box(Orientation.Horizontal, 5);
        botonBusquedaPosicionContainer.PackStart(analizarButton, false, false, 0);
        botonBusquedaPosicionContainer.PackStart(patronBusqueda, true, true, 0);
        botonBusquedaPosicionContainer.PackStart(botonBuscar, false, false, 0);
        botonBusquedaPosicionContainer.PackStart(labelCoincidencias, false, false, 0);
        botonBusquedaPosicionContainer.PackStart(cursorPosition, false, false, 0);

        // Inicializar la instancia de BusquedaPatrones
        busquedaPatrones = new BusquedaPatrones(textEditor);

        mainLayout.PackStart(editorContainer, true, true, 0);
        mainLayout.PackStart(botonBusquedaPosicionContainer, false, false, 0); // Añadir el contenedor de búsqueda, botón analizar y posicion
        mainLayout.PackStart(errorScroll, true, true, 0);

        mainWindow.Add(mainLayout);
        mainWindow.ShowAll();

        UpdateLineNumbers();

        analizador = new Analizador();
    }

    private void UpdateLineNumbers()
    {
        // Obtener la cantidad de líneas en el editor de texto
        int totalLines = textEditor.Buffer.LineCount;

        // Crear los números de línea
        StringBuilder lineNumbers = new StringBuilder();
        for (int i = 1; i <= totalLines; i++)
        {
            lineNumbers.AppendLine(i.ToString());
        }

        // Actualizar el área de números de línea
        lineNumberArea.Buffer.Text = lineNumbers.ToString();
    }

    public void Run()
    {
        Application.Run();
    }
}