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
        public Token AnalizarToken(string token, int fila, int columna)
        {
            // 1. Validación de decimales
            AnalizadorDecimal analizadorDecimal = new AnalizadorDecimal();
            Token tokenReconocido = analizadorDecimal.ProcesarDecimal(token, fila, columna);

            if (tokenReconocido != null)
            {
                return tokenReconocido; // Retorna el token si es un número decimal válido
            }

            // 2. Validación de enteros
            AnalizadorEntero analizadorEntero = new AnalizadorEntero();
            tokenReconocido = analizadorEntero.ProcesarEntero(token, fila, columna);

            if (tokenReconocido != null)
            {
                return tokenReconocido; // Retorna el token si es un entero válido
            }

            // 3. Validación de identificadores
            AnalizadorIdentificador analizadorIdentificador = new AnalizadorIdentificador();
            tokenReconocido = analizadorIdentificador.ProcesarIdentificador(token, fila, columna);

            if (tokenReconocido != null)
            {
                return tokenReconocido; // Retorna el token si es un identificador válido
            }

            // 4. Validar palabras reservadas
            AnalizadorPalabraReservada analizadorPalabraReservada = new AnalizadorPalabraReservada();
            tokenReconocido = analizadorPalabraReservada.ProcesarPalabraReservada(token, fila, columna);

            if (tokenReconocido != null)
            {
                return tokenReconocido;
            }

            // 5. Validar literales con comillas simples y dobles
            AnalizadorLiteral analizadorLiteral = new AnalizadorLiteral();
            tokenReconocido = analizadorLiteral.ProcesarLiteral(token, fila, columna);

            if (tokenReconocido != null)
            {
                return tokenReconocido;
            }

            // 6. Validar signos de puntuación
            AnalizadorSignosPuntuacion analizadorSignosPuntuacion = new AnalizadorSignosPuntuacion();
            tokenReconocido = analizadorSignosPuntuacion.ProcesarSignoPuntuacion(token, fila, columna);

            if (tokenReconocido != null)
            {
                return tokenReconocido;
            }

            // 7. Validar un operador aritmético
            AnalizadorAritmetico analizadorAritmetico = new AnalizadorAritmetico();
            tokenReconocido = analizadorAritmetico.ProcesarOperadorAritmetico(token, fila, columna);

            if (tokenReconocido != null)
            {
                return tokenReconocido;
            }

            // 8. Validar un operador relacional o de asignación
            AnalizadorRelacionalAsignacion analizadorRelacionalAsignacion = new AnalizadorRelacionalAsignacion();
            tokenReconocido = analizadorRelacionalAsignacion.ProcesarOperador(token, fila, columna);

            if (tokenReconocido != null) 
            {
                return tokenReconocido;
            }

            // 9. Validar operadores lógicos
            AnalizadorLogico analizadorLogico = new AnalizadorLogico();
            tokenReconocido = analizadorLogico.ProcesarOperadorLogico(token, fila, columna);

            if (tokenReconocido != null)
            {
                return tokenReconocido;
            }

            // 10. Validar signos de agrupación
            AnalizadorAgrupacion analizadorAgrupacion = new AnalizadorAgrupacion();
            tokenReconocido = analizadorAgrupacion.ProcesarSignoAgrupacion(token, fila, columna);

            if (tokenReconocido != null)
            {
                return tokenReconocido;
            }

            // 11. Validar comentario de una sola línea
            AnalizadorComentarioSimple analizadorComentarioSimple = new AnalizadorComentarioSimple();
            tokenReconocido = analizadorComentarioSimple.ProcesarComentarioSimple(token, fila, columna);

            if (tokenReconocido != null) 
            {
                return tokenReconocido;
            }

            // 12. Validar comentarios en bloque
            AnalizadorComentarioBloque analizadorComentarioBloque = new AnalizadorComentarioBloque();
            tokenReconocido = analizadorComentarioBloque.ProcesarComentarioBloque(token, fila, columna);

            if (tokenReconocido != null)
            {
                return tokenReconocido;
            }

            // 13. Si el token no coincide con ninguno, registrar el error
            errores.Add($"Error: Token no reconocido -> '{token}' en fila {fila}, columna {columna}");
            return null!; // Retorna null si no es válido
        }

        // Método para analizar una lista completa de tokens
        public void AnalizarTokens(List<string> lexemas, int filaInicial)
        {
            tokens.Clear();
            errores.Clear();
            
            int fila = filaInicial; // Inicio de la fila
            int columna = 1; // Reiniciar columna

            foreach (string lexema in lexemas)
            {
                Token token = AnalizarToken(lexema, fila, columna);

                if (token != null)
                {
                    tokens.Add(token); // Agregar a la lista de tokens válidos
                }

                columna += lexema.Length + 1; // Avanzar la columna según el tamaño del lexema más un espacio
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

        public List<Token> Tokens{
            get { return tokens;}
        }

        public List<string> Errores 
        {
            get { return errores; }
        }
    }
}
