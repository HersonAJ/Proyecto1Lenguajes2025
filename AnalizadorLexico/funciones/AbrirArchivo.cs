using Gtk;
using System.IO;

namespace Funciones
{
    public class AbrirArchivo
    {
        ///<summary>
        /// Método para abrir un archivo, validar su contenido y extraerlo al editor de texto.
        ///</summary>
        ///<param name="textEditor">TextView en donde se mostrará el contenido extraído.</param>
        public void Abrir(TextView textEditor)
        {
            // Usar un bloque 'using' para el file chooser
            using (FileChooserDialog fileChooser = new FileChooserDialog(
                "Abrir Archivo",
                null,
                FileChooserAction.Open,
                "Cancelar", ResponseType.Cancel,
                "Abrir", ResponseType.Accept))
            {
                if (fileChooser.Run() == (int)ResponseType.Accept)
                {
                    string filePath = fileChooser.Filename;

                    // Validar si el archivo existe y no está vacío
                    if (!File.Exists(filePath))
                    {
                        MostrarError("El archivo seleccionado no existe.");
                        return;
                    }

                    string fileContent = File.ReadAllText(filePath);

                    if (string.IsNullOrWhiteSpace(fileContent))
                    {
                        MostrarError("El archivo está vacío.");
                        return;
                    }

                    // Mostrar el contenido obtenido en el editor de texto
                    textEditor.Buffer.Text = fileContent;
                }
            }
        }

        ///<summary>
        /// Método para mostrar errores en un cuadro de diálogo.
        ///</summary>
        ///<param name="mensaje">El mensaje de error a mostrar.</param>
        private void MostrarError(string mensaje)
        {
            // Usar un bloque 'using' para el MessageDialog
            using (MessageDialog errorDialog = new MessageDialog(
                null,
                DialogFlags.Modal,
                MessageType.Error,
                ButtonsType.Ok,
                mensaje))
            {
                errorDialog.Run();
            }
        }
    }
}
