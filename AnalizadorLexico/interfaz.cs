

using Gtk;
using System;
using System.Collections.Generic;
using Analizadores; // namespace asignado 

public class Interfaz
{
    private Window mainWindow;
    private Box mainLayout;
    private MenuBar menuBar;
    private TextView textEditor;
    private Label cursorPosition;
    private TextView errorArea;
    private Button analizarButton;

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

        // Editor de texto
        textEditor = new TextView();
        textEditor.StyleContext.AddClass("custom-editor");
        ScrolledWindow editorScroll = new ScrolledWindow();
        editorScroll.Add(textEditor);

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
        analizarButton.Clicked += (o, e) => AnalizarTexto();

        // Área de errores
        errorArea = new TextView();
        errorArea.StyleContext.AddClass("custom-error");
        errorArea.Buffer.Text = "Área de resultados...";
        errorArea.Editable = false;
        ScrolledWindow errorScroll = new ScrolledWindow();
        errorScroll.Add(errorArea);

        // Agregar componentes al contenedor principal
        mainLayout.PackStart(menuBar, false, false, 0);
        mainLayout.PackStart(editorScroll, true, true, 0);
        mainLayout.PackStart(cursorPosition, false, false, 0);
        mainLayout.PackStart(analizarButton, false, false, 0);
        mainLayout.PackStart(errorScroll, true, true, 0);

        // Configurar y mostrar ventana
        mainWindow.Add(mainLayout);
        mainWindow.ShowAll();
    }

    public void Run()
    {
        Application.Run();
    }

    private void AnalizarTexto()
    {
        // Obtiene el texto del editor
        string text = textEditor.Buffer.Text;

        // Divide el texto en tokens utilizando espacios y saltos de línea como separadores
        List<string> tokens = new List<string>(text.Split(new[] { ' ', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries));

        // Instancia del analizador léxico
        AnalizadorLexico analizador = new AnalizadorLexico();

        // Limpia el área de errores
        errorArea.Buffer.Text = "";

        // Analiza cada token y muestra los resultados en el área de errores
        foreach (string token in tokens)
        {
            string resultado = analizador.AnalizarToken(token);
            errorArea.Buffer.Text += $"Token: {token} -> Resultado: {resultado}\n";
        }
    }
}
