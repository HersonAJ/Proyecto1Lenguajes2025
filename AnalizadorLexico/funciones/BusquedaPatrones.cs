using Gtk;
using Analizadores;

namespace Funciones
{
    public class BusquedaPatrones
    {
        private TextView textEditor;

        public BusquedaPatrones(TextView textEditor)
        {
            this.textEditor = textEditor;
        }

        public int BuscarYResaltar(string patron)
        {
            try
            {
                Console.WriteLine($"[INFO] Iniciando búsqueda del patrón: {patron}");

                if (string.IsNullOrEmpty(patron))
                {
                    Console.WriteLine("[ERROR] El patrón de búsqueda está vacío.");
                    return 0;
                }

                LimpiarResaltados();

                TextTagTable tagTable = textEditor.Buffer.TagTable;
                if (tagTable.Lookup("highlight") == null)
                {
                    TextTag highlightTag = new TextTag("highlight")
                    {
                        Background = "yellow"
                    };
                    tagTable.Add(highlightTag);
                }

                string textoCompleto = textEditor.Buffer.Text;

                int coincidencias = 0;
                int longitudPatron = patron.Length;

                for (int i = 0; i <= textoCompleto.Length - longitudPatron; i++)
                {
                    bool esCoincidenciaExacta = true;

                    // Verificar si los caracteres antes y después de la coincidencia no son parte de una palabra más larga
                    if (i > 0 && char.IsLetterOrDigit(textoCompleto[i - 1]))
                    {
                        esCoincidenciaExacta = false;
                    }
                    else if (i + longitudPatron < textoCompleto.Length && char.IsLetterOrDigit(textoCompleto[i + longitudPatron]))
                    {
                        esCoincidenciaExacta = false;
                    }

                    // Verificar si el patrón coincide en la posición actual
                    if (esCoincidenciaExacta && textoCompleto.Substring(i, longitudPatron) == patron)
                    {
                        coincidencias++;
                        TextIter matchStart = textEditor.Buffer.GetIterAtOffset(i);
                        TextIter matchEnd = textEditor.Buffer.GetIterAtOffset(i + longitudPatron);
                        textEditor.Buffer.ApplyTag("highlight", matchStart, matchEnd);
                    }
                }


                Console.WriteLine($"[INFO] Búsqueda completada. Total de coincidencias: {coincidencias}");
                return coincidencias;
            }

            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Ocurrio un error durante la busqueda: {ex.Message}");
                return 0;
            }
        }

        private void LimpiarResaltados()
        {
            try
            {
                TextTagTable tagTable = textEditor.Buffer.TagTable;
                TextTag highlightTag = tagTable.Lookup("highlight");
                if (highlightTag != null)
                {
                    textEditor.Buffer.RemoveTag(highlightTag, textEditor.Buffer.StartIter, textEditor.Buffer.EndIter);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Ocurrio un error al limpiar los resultados: {ex.Message}");
            }
        }
    }
}