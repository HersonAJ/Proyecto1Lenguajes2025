using Gtk;
using System.IO;

namespace Funciones
{
    public class GuardarArchivo
    {
        private string? rutaArchivo; // Ruta del archivo actual (si ya fue abierto previamente)

        public GuardarArchivo()
        {
            rutaArchivo = null; // Inicialmente no hay un archivo cargado
        }
        public void Guardar(TextView textEditor)
        {
            try
            {
                // Si hay un archivo cargado previamente
                if (!string.IsNullOrEmpty(rutaArchivo))
                {
                    try
                    {
                        string contenido = textEditor.Buffer.Text;
                        File.WriteAllText(rutaArchivo, contenido); // Sobrescribir el archivo existente
                    }
                    catch (Exception ex)
                    {
                        MostrarError($"No se pudo guardar el archivo: {ex.Message}");
                    }
                }
                else
                {
                    // Si no hay un archivo cargado, usar la funcionalidad de Guardar Como
                    GuardarComo(textEditor);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado en Guardar: {ex.Message}");
            }
        }
        public void GuardarComo(TextView textEditor)
        {
            try
            {
                using (FileChooserDialog guardarComoDialog = new FileChooserDialog(
                    "Guardar Archivo Como",
                    null,
                    FileChooserAction.Save,
                    "Cancelar", ResponseType.Cancel,
                    "Guardar", ResponseType.Accept))
                {
                    // Ejecutar el cuadro de diálogo
                    if (guardarComoDialog.Run() == (int)ResponseType.Accept)
                    {
                        try
                        {
                            // Obtener la ruta del archivo seleccionado
                            string filePath = guardarComoDialog.Filename;

                            // Guardar el contenido en el archivo seleccionado
                            string contenido = textEditor.Buffer.Text;
                            File.WriteAllText(filePath, contenido);

                            // Actualizar la ruta del archivo actual (porque Guardar Como asigna un nuevo archivo)
                            rutaArchivo = filePath;
                        }
                        catch (Exception ex)
                        {
                            MostrarError($"No se pudo guardar el archivo: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inseperado en GuardarComo: {ex.Message}");
            }
        }
        public void EstablecerRutaArchivo(string ruta)
        {
            try
            {
                rutaArchivo = ruta; // Asignar la ruta del archivo abierto
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado en Establecer ruta: {ex.Message}");
            }
        }
        private void MostrarError(string mensaje)
        {
            try
            {
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
                Console.WriteLine($"Error al mostrar mensaje de error: {ex.Message}");
            }
        }
    }
}
