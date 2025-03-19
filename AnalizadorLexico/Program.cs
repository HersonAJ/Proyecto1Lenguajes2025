using Gtk;
using System;

class Program
{
    static void Main(string[] args)
    {
        // Inicializa GTK
        Application.Init();

        // Cargar archivo CSS
        Gtk.CssProvider cssProvider = new Gtk.CssProvider();
        cssProvider.LoadFromPath("style.css");
        Gtk.StyleContext.AddProviderForScreen(
            Gdk.Screen.Default,
            cssProvider,
            Gtk.StyleProviderPriority.Application
        );

        // Crea la ventana principal
        Window mainWindow = new Window("Analizador Léxico - IDE Básico");
        mainWindow.SetDefaultSize(800, 600);
        mainWindow.DeleteEvent += (o, e) => Application.Quit();

        // Contenedor principal vertical
        Box mainLayout = new Box(Orientation.Vertical, 5);

        // Menú superior
        MenuBar menuBar = new MenuBar();
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

        Menu editSubMenu = new Menu();
        editMenu.Submenu = editSubMenu;
        MenuItem undo = new MenuItem("Deshacer");
        MenuItem redo = new MenuItem("Rehacer");
        MenuItem copy = new MenuItem("Copiar");
        MenuItem paste = new MenuItem("Pegar");
        editSubMenu.Append(undo);
        editSubMenu.Append(redo);
        editSubMenu.Append(copy);
        editSubMenu.Append(paste);

        Menu helpSubMenu = new Menu();
        helpMenu.Submenu = helpSubMenu;
        MenuItem about = new MenuItem("Acerca de");
        helpSubMenu.Append(about);

        menuBar.Append(fileMenu);
        menuBar.Append(editMenu);
        menuBar.Append(helpMenu);

        // Editor de texto
        TextView textEditor = new TextView();
        textEditor.StyleContext.AddClass("custom-editor");

        ScrolledWindow editorScroll = new ScrolledWindow();
        editorScroll.Add(textEditor);

        // Indicador de línea y columna
        Label cursorPosition = new Label("Línea: 1, Columna: 1");

        textEditor.Buffer.Changed += (o, e) =>
        {
            TextIter cursorIter = textEditor.Buffer.GetIterAtMark(textEditor.Buffer.InsertMark);
            int line = cursorIter.Line + 1;
            int column = cursorIter.LineOffset + 1;
            cursorPosition.Text = $"Línea: {line}, Columna: {column}";
        };

        // Área de errores
        TextView errorArea = new TextView();
        errorArea.StyleContext.AddClass("custom-error");
          errorArea.Buffer.Text = "Área de errores...";
        errorArea.Editable = false;

        ScrolledWindow errorScroll = new ScrolledWindow();
        errorScroll.Add(errorArea);

        // Agregar componentes al contenedor principal
        mainLayout.PackStart(menuBar, false, false, 0);
        mainLayout.PackStart(editorScroll, true, true, 0);
        mainLayout.PackStart(cursorPosition, false, false, 0);
        mainLayout.PackStart(errorScroll, true, true, 0);

        // Configurar y mostrar ventana
        mainWindow.Add(mainLayout);
        mainWindow.ShowAll();
        Application.Run();
    }
}