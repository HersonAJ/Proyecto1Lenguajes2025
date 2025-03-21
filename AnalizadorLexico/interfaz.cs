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
    MenuItem helpMenu = new MenuItem("Ayuda");

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
            MessageDialog errorDialog = new MessageDialog(
                null,
                DialogFlags.Modal,
                MessageType.Error,
                ButtonsType.Ok,
                $"Error al abrir el archivo: {ex.Message}"
            );
            errorDialog.Run();
            errorDialog.Destroy();
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

    menuBar.Append(fileMenu);
    menuBar.Append(editMenu);
    menuBar.Append(helpMenu);

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

    mainLayout.PackStart(menuBar, false, false, 0);
    mainLayout.PackStart(editorContainer, true, true, 0);
    mainLayout.PackStart(cursorPosition, false, false, 0);
    mainLayout.PackStart(analizarButton, false, false, 0);
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
