using System;
using System.Collections.Generic;
using Logica;
using Analizadores;

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
            // 1. Validación de decimales
            AnalizadorDecimal analizadorDecimal = new AnalizadorDecimal();
            Token tokenReconocido = analizadorDecimal.ProcesarDecimal(token, posicion);

            if (tokenReconocido != null)
            {
                return tokenReconocido; // Retorna el token si es un número decimal válido
            }

            // 2. Validación de enteros
            AnalizadorEntero analizadorEntero = new AnalizadorEntero();
            tokenReconocido = analizadorEntero.ProcesarEntero(token, posicion);

            if (tokenReconocido != null)
            {
                return tokenReconocido; // Retorna el token si es un entero válido
            }

            // 3. Validación de identificadores
            AnalizadorIdentificador analizadorIdentificador = new AnalizadorIdentificador();
            tokenReconocido = analizadorIdentificador.ProcesarIdentificador(token, posicion);

            if (tokenReconocido != null)
            {
                return tokenReconocido; // Retorna el token si es un identificador válido
            }

            //4. validar palabras reservadas
            AnalizadorPalabraReservada analizadorPalabraReservada = new AnalizadorPalabraReservada();
            tokenReconocido = analizadorPalabraReservada.ProcesarPalabraReservada(token, posicion);

            if (tokenReconocido != null)
            {
                return tokenReconocido;
            }

            //5. validar literales con comillas simples y dobles
            AnalizadorLiteral analizadorLiteral = new AnalizadorLiteral();
            tokenReconocido = analizadorLiteral.ProcesarLiteral(token, posicion);

            if (tokenReconocido != null)
            {
                return tokenReconocido;
            }



            //6. validar signos de puntuacion
            AnalizadorSignosPuntuacion analizadorSignosPuntuacion = new AnalizadorSignosPuntuacion();
            tokenReconocido = analizadorSignosPuntuacion.ProcesarSignoPuntuacion(token, posicion);

            if (tokenReconocido != null)
            {
                return tokenReconocido;
            }

            //7. validar un operador aritmetico 
            AnalizadorAritmetico analizadorAritmetico = new AnalizadorAritmetico();
            tokenReconocido = analizadorAritmetico.ProcesarOperadorAritmetico(token, posicion);

            if (tokenReconocido != null)
            {
                return tokenReconocido;
            }

            //8. validar un operador racionar y de asignacion
            AnalizadorRelacionalAsignacion analizadorRelacionalAsignacion = new AnalizadorRelacionalAsignacion();
            tokenReconocido = analizadorRelacionalAsignacion.ProcesarOperador(token , posicion);

            if (tokenReconocido != null) 
            {
                return tokenReconocido;
            }

            // 9 valida los operadores logicos
            AnalizadorLogico analizadorLogico = new AnalizadorLogico();
            tokenReconocido = analizadorLogico.ProcesarOperadorLogico(token, posicion);

            if (tokenReconocido != null)
            {
                return tokenReconocido;
            }

            //10 validar signos de agrupacion 
            AnalizadorAgrupacion analizadorAgrupacion = new AnalizadorAgrupacion();
            tokenReconocido = analizadorAgrupacion.ProcesarSignoAgrupacion(token, posicion);

            if (tokenReconocido != null)
            {
                return tokenReconocido;
            }

            // validar comentario de una sola linea
            AnalizadorComentarioSimple analizadorComentarioSimple = new AnalizadorComentarioSimple();
            tokenReconocido = analizadorComentarioSimple.ProcesarComentarioSimple(token, posicion);

            if (tokenReconocido != null) 
            {
                return tokenReconocido;
            }

            // 12. Si el token no coincide con ninguno, registrar el error
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

                posicion++; // Avanzar la posición (ajustar filas y columnas)
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
