using Gtk;

namespace Funciones
{
    public class BusquedaPatrones
    {
        private TextView textEditor;

        public BusquedaPatrones(TextView textEditor)
        {
            this.textEditor = textEditor;
        }

        ///<summary>
        /// Método para buscar y resaltar un patrón en el editor de texto.
        ///</summary>
        ///<param name="patron">Cadena a buscar.</param>
        ///<returns>Número de coincidencias encontradas.</returns>
        public int BuscarYResaltar(string patron)
        {
            // Limpiar resaltados previos
            LimpiarResaltados();

            TextTagTable tagTable = textEditor.Buffer.TagTable;
            if (tagTable.Lookup("highlight") == null)
            {
                TextTag highlightTag = new TextTag("highlight")
                {
                    Background = "yellow" // Color de resaltado
                };
                tagTable.Add(highlightTag);
            }

            // Buscar el patrón en el texto del editor
            string textoCompleto = textEditor.Buffer.Text;
            int coincidencias = 0;

            TextIter startIter = textEditor.Buffer.StartIter;
            TextIter matchStart, matchEnd;

            // Ajuste para el argumento 'limit' en ForwardSearch
            while (startIter.ForwardSearch(patron, TextSearchFlags.TextOnly, out matchStart, out matchEnd, textEditor.Buffer.EndIter))
            {
                textEditor.Buffer.ApplyTag("highlight", matchStart, matchEnd); // Aplicar resaltado
                startIter = matchEnd; // Continuar buscando después de la coincidencia
                coincidencias++;
            }

            return coincidencias;
        }

        ///<summary>
        /// Método para limpiar los resaltados previos en el editor de texto.
        ///</summary>
        private void LimpiarResaltados()
        {
            TextTagTable tagTable = textEditor.Buffer.TagTable;
            TextTag highlightTag = tagTable.Lookup("highlight");
            if (highlightTag != null)
            {
                textEditor.Buffer.RemoveTag(highlightTag, textEditor.Buffer.StartIter, textEditor.Buffer.EndIter);
            }
        }
    }
}
