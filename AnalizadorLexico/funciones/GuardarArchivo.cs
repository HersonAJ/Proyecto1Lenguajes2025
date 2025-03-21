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

        ///<summary>
        /// Método para guardar los cambios realizados en un archivo.
        /// Si no existe un archivo cargado, se comporta como "Guardar Como".
        ///</summary>
        ///<param name="textEditor">El TextView que contiene el texto a guardar.</param>
        public void Guardar(TextView textEditor)
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

        ///<summary>
        /// Método para guardar el texto en un archivo especificado por el usuario.
        ///</summary>
        ///<param name="textEditor">El TextView que contiene el texto a guardar.</param>
        public void GuardarComo(TextView textEditor)
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

        ///<summary>
        /// Método para establecer la ruta del archivo actual (usado al abrir un archivo).
        ///</summary>
        ///<param name="ruta">La ruta del archivo cargado.</param>
        public void EstablecerRutaArchivo(string ruta)
        {
            rutaArchivo = ruta; // Asignar la ruta del archivo abierto
        }

        ///<summary>
        /// Método para mostrar errores en un cuadro de diálogo.
        ///</summary>
        ///<param name="mensaje">El mensaje de error a mostrar.</param>
        private void MostrarError(string mensaje)
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
    }
}
