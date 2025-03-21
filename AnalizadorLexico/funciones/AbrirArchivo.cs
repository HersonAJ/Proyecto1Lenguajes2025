using Gtk;
using System.IO;

namespace Funciones
{
    public class AbrirArchivo
    {
        ///<summary>
        /// Método para abrir un archivo, validar su contenido y extraerlo al editor de texto.
        /// Retorna la ruta del archivo abierto, o null si no se seleccionó ningún archivo.
        ///</summary>
        ///<param name="textEditor">TextView en donde se mostrará el contenido extraído.</param>
        ///<returns>La ruta del archivo abierto, o null si no se seleccionó un archivo.</returns>
        public string? Abrir(TextView? textEditor)
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
                        return null;
                    }

                    string fileContent = File.ReadAllText(filePath);

                    if (string.IsNullOrWhiteSpace(fileContent))
                    {
                        MostrarError("El archivo está vacío.");
                        return null;
                    }

                    // Validar que textEditor no sea null antes de asignar el contenido
                    if (textEditor != null)
                    {
                        textEditor.Buffer.Text = fileContent;
                    }

                    // Retornar la ruta del archivo cargado
                    return filePath;
                }
            }

            // Si se cancela el diálogo, retornar null
            return null;
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
