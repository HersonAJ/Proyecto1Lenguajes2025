using System;
using System.Collections.Generic;

namespace Analizadores
{
    public class AnalizadorLexico
    {
        // Lista de errores para notificar al usuario
        private List<string> errores;

        public AnalizadorLexico()
        {
            errores = new List<string>();
        }

        // Método para analizar un token y determinar su tipo
        public string AnalizarToken(string token)
        {
            // 1. Validación de identificadores
            AnalizadorIdentificador analizadorIdentificador = new AnalizadorIdentificador();
            if (analizadorIdentificador.EsIdentificadorValido(token))
            {
                return "Identificador válido";
            }


            // Si el token no coincide con ninguno se marca cono no reconocido
            errores.Add($"Token no reconocido: {token}");
            return "Token no reconocido";
        }

        // Método para analizar una lista completa de tokens
        public void AnalizarTokens(List<string> tokens)
        {
            foreach (string token in tokens)
            {
                string resultado = AnalizarToken(token);
                Console.WriteLine($"Token: {token} -> Resultado: {resultado}");
            }

            // Mostrar errores por el momento 
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
