using Gtk;
using System.IO;

namespace Funciones
{
    public class AbrirArchivo
    {
        public string? Abrir(TextView? textEditor)
        {
            try
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
            catch (IOException ioEx)
            {
                MostrarError($"Error de lectura: {ioEx.Message}");
                return null;
            }
            catch (Exception ex)
            {
                MostrarError($"Error inesperado: {ex.Message}");
                return null;
            }
        }
        private void MostrarError(string mensaje)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error al mostrar el mensaje de error: {ex.Message}");
            }
        }
    }
}
