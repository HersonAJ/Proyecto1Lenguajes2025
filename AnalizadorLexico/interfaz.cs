using Gtk;
using System;
using System.Text;
using Logica;
using Analizadores;

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

        menuBar.Append(fileMenu);
        menuBar.Append(editMenu);
        menuBar.Append(helpMenu);

        // Contenedor horizontal para la numeración de líneas y el editor
        Box editorContainer = new Box(Orientation.Horizontal, 0);

        // Área para los números de línea
        lineNumberArea = new TextView();
        lineNumberArea.Editable = false;
        lineNumberArea.WrapMode = WrapMode.None;
        lineNumberArea.StyleContext.AddClass("line-number-area");
        ScrolledWindow lineNumberScroll = new ScrolledWindow();
        lineNumberScroll.Add(lineNumberArea);

        // Editor de texto principal
        textEditor = new TextView();
        textEditor.StyleContext.AddClass("custom-editor");
        textEditor.Buffer.Changed += (o, e) => UpdateLineNumbers(); // Actualiza números de línea en cada cambio
        textEditor.CursorVisible = true; // Asegurar que el cursor sea visible
        textEditor.GrabFocus();
        ScrolledWindow editorScroll = new ScrolledWindow();
        editorScroll.Add(textEditor);

        // Añadir las áreas al contenedor horizontal
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
            // Obtener el texto del editor
            string texto = textEditor.Buffer.Text;

            // Llamar al método AnalizarTexto desde la clase Analizador
            string resultado = analizador!.AnalizarTexto(texto);

            // Mostrar el resultado en el área de errores
            errorArea!.Buffer.Text = resultado;
        };

        // Área de errores
        errorArea = new TextView();
        errorArea.StyleContext.AddClass("custom-error");
        errorArea.Buffer.Text = "Área de resultados...";
        errorArea.Editable = false;
        ScrolledWindow errorScroll = new ScrolledWindow();
        errorScroll.Add(errorArea);

        // Agregar componentes al contenedor principal
        mainLayout.PackStart(menuBar, false, false, 0);
        mainLayout.PackStart(editorContainer, true, true, 0);
        mainLayout.PackStart(cursorPosition, false, false, 0);
        mainLayout.PackStart(analizarButton, false, false, 0);
        mainLayout.PackStart(errorScroll, true, true, 0);

        // Configurar y mostrar ventana
        mainWindow.Add(mainLayout);
        mainWindow.ShowAll();

        // Inicializar números de línea
        UpdateLineNumbers();

        // Crear instancia de la clase Analizador
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
