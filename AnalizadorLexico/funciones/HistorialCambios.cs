using System;
using System.Collections.Generic;
using Gtk;

namespace Funciones
{
    public class HistorialCambios
    {
        private Stack<string> pilaDeshacer; // Pila para almacenar los estados previos del texto
        private Stack<string> pilaRehacer;  // Pila para almacenar los estados deshechos
        private TextView textEditor;        // Referencia al editor de texto
        private string ultimoEstado;        // Último estado conocido para evitar duplicados innecesarios
        private bool esCambioProgramatico;  // Bandera para diferenciar cambios programáticos y manuales

        public HistorialCambios(TextView textEditor)
        {
            this.textEditor = textEditor;

            // Inicializar las pilas y el último estado
            pilaDeshacer = new Stack<string>();
            pilaRehacer = new Stack<string>();
            ultimoEstado = textEditor.Buffer.Text; // Estado inicial
            esCambioProgramatico = false;

            // Agregar el estado inicial del texto
            pilaDeshacer.Push(ultimoEstado);

            // Escuchar cambios en el texto para actualizar el historial
            textEditor.Buffer.Changed += (sender, args) =>
            {
                if (esCambioProgramatico)
                {
                
                    return;
                }

                string estadoActual = textEditor.Buffer.Text;

                // Registrar cambios solo si el texto es diferente
                if (estadoActual != ultimoEstado)
                {
                    pilaDeshacer.Push(estadoActual);
                    ultimoEstado = estadoActual;

                    // Limpiar la pila de rehacer porque se pierde el contexto
                    pilaRehacer.Clear();
                }
            };
        }

        ///<summary>
        /// Método para deshacer el último cambio en el editor de texto.
        ///</summary>
        public void Deshacer()
        {
            if (pilaDeshacer.Count > 1)
            {
                // Guardar el estado actual en la pila de rehacer
                string estadoActual = pilaDeshacer.Pop();
                pilaRehacer.Push(estadoActual);

                // Restaurar el estado anterior
                string estadoAnterior = pilaDeshacer.Peek();
                Console.WriteLine($"[HistorialCambios] Deshacer. Restaurando estado: {estadoAnterior}");
                esCambioProgramatico = true; // Marcar como cambio programático
                textEditor.Buffer.Text = estadoAnterior;
                esCambioProgramatico = false; // Restablecer bandera
                ultimoEstado = estadoAnterior; // Actualizar último estado
            }
            else
            {
                Console.WriteLine("[HistorialCambios] No hay más cambios para deshacer.");
            }
        }

        ///<summary>
        /// Método para rehacer el último cambio deshecho en el editor de texto.
        ///</summary>
        public void Rehacer()
        {
            if (pilaRehacer.Count > 0)
            {
                // Recuperar el estado deshecho más reciente
                string estadoRehecho = pilaRehacer.Pop();

                // Agregar el estado rehecho a la pila de deshacer
                pilaDeshacer.Push(estadoRehecho);

                // Restaurar el estado rehecho
                Console.WriteLine($"[HistorialCambios] Rehacer. Restaurando estado: {estadoRehecho}");
                esCambioProgramatico = true; // Marcar como cambio programático
                textEditor.Buffer.Text = estadoRehecho;
                esCambioProgramatico = false; // Restablecer bandera
                ultimoEstado = estadoRehecho; // Actualizar último estado
            }
            else
            {
                Console.WriteLine("[HistorialCambios] No hay más cambios para rehacer.");
            }
        }
    }
}
