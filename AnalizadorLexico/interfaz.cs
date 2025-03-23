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
    private Entry patronBusqueda;
    private Button botonBuscar;
    private Label labelCoincidencias;
    private BusquedaPatrones busquedaPatrones;

    public Interfaz()
    {
        // Inicializa GTK
        Application.Init();

        // Inicializa el analizador
        analizador = new Analizador();

        // Inicializa el botón de análisis
        analizarButton = new Button("Analizar");

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
            if (analizador != null && analizador.Tokens.Count > 0 && analizador.Errores.Count == 0)
            {
                // Crear y mostrar la ventana del reporte
                ReporteTokens reporte = new ReporteTokens(analizador.Tokens);
                reporte.MostrarReporte();
            }
            else if (analizador!.Errores.Count > 0)
            {
                // Mostrar mensaje si hay errores
                using (MessageDialog errorDialog = new MessageDialog(
                    mainWindow,
                    DialogFlags.Modal,
                    MessageType.Error,
                    ButtonsType.Ok,
                    "El reporte no se puede generar porque existen errores en el análisis. Por favor, corrige los errores e intenta de nuevo."
                ))
                {
                    errorDialog.Run();
                }
            }
            else
            {
                // Mostrar mensaje si no hay tokens reconocidos
                using (MessageDialog noTokensDialog = new MessageDialog(
                    mainWindow,
                    DialogFlags.Modal,
                    MessageType.Warning,
                    ButtonsType.Ok,
                    "No hay tokens reconocidos para generar el reporte."
                ))
                {
                    noTokensDialog.Run();
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

        // Contenedor vertical para envolver el área de números de línea
        Box lineNumberContainer = new Box(Orientation.Vertical, 0);
        lineNumberContainer.StyleContext.AddClass("line-number-container");

        // Área para los números de línea
        lineNumberArea = new TextView();
        lineNumberArea.Editable = false;
        lineNumberArea.WrapMode = WrapMode.None;
        lineNumberArea.StyleContext.AddClass("line-number-area");
        lineNumberContainer.PackStart(lineNumberArea, true, true, 0); // Agregar al contenedor

        // ScrolledWindow para el contenedor de números de línea
        ScrolledWindow lineNumberScroll = new ScrolledWindow();
        lineNumberScroll.Add(lineNumberContainer);

        // Sincronizar el desplazamiento del área de numeración de líneas con el editor de texto
        Adjustment textScrollAdjustment = editorScroll.Vadjustment;
        Adjustment lineNumberScrollAdjustment = lineNumberScroll.Vadjustment;

        textScrollAdjustment.ValueChanged += (o, e) =>
        {
            // Actualiza la posición de scroll de los números de línea
            lineNumberScrollAdjustment.Value = textScrollAdjustment.Value;
        };

        // Agregar lineNumberScroll y editorScroll al editorContainer
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
        analizarButton.Clicked += (o, e) =>
        {
            if (analizador == null)
            {
                Console.WriteLine("Error: El analizador no está inicializado.");
                return;
            }

            string texto = textEditor.Buffer.Text;
            string resultado = analizador.AnalizarTexto(texto);

            // Limpiar el área de errores antes de mostrar nuevos resultados
            errorArea!.Buffer.Text = "";

            // Mostrar solo errores
            if (analizador.Errores.Count > 0)
            {
                errorArea.Buffer.Text = string.Join("\n", analizador.Errores); // Mostrar errores
                errorArea.Visible = true; // Hacer visible el área de errores
                mainWindow.Resize(800, 600); // Expandir la ventana
            }
            else
            {
                errorArea.Visible = false; // Ocultar si no hay errores
                mainWindow.Resize(800, 400); // Contraer la ventana
            }
        };

        // Área de errores
        errorArea = new TextView();
        errorArea.StyleContext.AddClass("custom-error");
        errorArea.Buffer.Text = ""; // Inicializar sin texto
        errorArea.Editable = false;
        errorArea.Visible = false; // Ocultar por defecto
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

        // Contenedor horizontal para la búsqueda, botón analizar y posición
        Box botonBusquedaPosicionContainer = new Box(Orientation.Horizontal, 5);
        botonBusquedaPosicionContainer.PackStart(analizarButton, false, false, 0);
        botonBusquedaPosicionContainer.PackStart(patronBusqueda, true, true, 0);
        botonBusquedaPosicionContainer.PackStart(botonBuscar, false, false, 0);
        botonBusquedaPosicionContainer.PackStart(labelCoincidencias, false, false, 0);
        botonBusquedaPosicionContainer.PackStart(cursorPosition, false, false, 0);

        // Inicializar la instancia de BusquedaPatrones
        busquedaPatrones = new BusquedaPatrones(textEditor);

        // Distribuir el espacio entre textEditor y errorArea (3/4 para textEditor, 1/4 para errorArea)
        Box editorErrorContainer = new Box(Orientation.Vertical, 0);
        editorErrorContainer.PackStart(editorContainer, true, true, 0); // textEditor ocupa 3/4
        editorErrorContainer.PackStart(errorScroll, false, false, 0); // errorArea ocupa 1/4

        mainLayout.PackStart(editorErrorContainer, true, true, 0);
        mainLayout.PackStart(botonBusquedaPosicionContainer, false, false, 0); // Añadir el contenedor de búsqueda, botón analizar y posición

        mainWindow.Add(mainLayout);
        mainWindow.ShowAll();

        UpdateLineNumbers();
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