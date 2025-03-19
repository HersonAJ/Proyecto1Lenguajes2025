using System;
using System.Collections.Generic;
using Logica;

namespace Analizadores
{
    public class AnalizadorLexico
    {
        // Lista para almacenar tokens válidos
        private List<Token> tokens;
        
        // Lista de errores para notificar al usuario
        private List<string> errores;

        public AnalizadorLexico()
        {
            tokens = new List<Token>();
            errores = new List<string>();
        }

        // Método para analizar un token individual y determinar su tipo
        public Token AnalizarToken(string token, int posicion)
        {
            // 1. Validación de identificadores
            AnalizadorIdentificador analizadorIdentificador = new AnalizadorIdentificador();
            Token tokenReconocido = analizadorIdentificador.ProcesarIdentificador(token, posicion);

            if (tokenReconocido != null)
            {
                return tokenReconocido; // Retorna el token si es válido
            }

            // Si el token no coincide con ninguno, registrar el error
            errores.Add($"Error: Token no reconocido -> '{token}' en posición {posicion}");
            return null!; // Retorna null si no es válido
        }

        // Método para analizar una lista completa de tokens
        public void AnalizarTokens(List<string> lexemas)
        {
            tokens.Clear();
            errores.Clear();
            
            int posicion = 0;

            foreach (string lexema in lexemas)
            {
                Token token = AnalizarToken(lexema, posicion);

                if (token != null)
                {
                    tokens.Add(token); // Agregar a la lista de tokens válidos
                }

                posicion++; // Avanzar la posición (puedes ajustar si necesitas líneas/columnas)
            }

            // Mostrar los tokens válidos
            if (tokens.Count > 0)
            {
                Console.WriteLine("Tokens reconocidos:");
                foreach (var token in tokens)
                {
                    Console.WriteLine(token);
                }
            }

            // Mostrar errores si los hay
            if (errores.Count > 0)
            {
                Console.WriteLine("\nErrores encontrados:");
                foreach (string error in errores)
                {
                    Console.WriteLine(error);
                }
            }
        }
    }
}
