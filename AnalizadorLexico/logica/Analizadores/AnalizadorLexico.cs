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
            try
            {
                //1. validacion de identificadores
                AutomataIdentificador analizadorIdentificador = new AutomataIdentificador();
                Token tokenReconocido = analizadorIdentificador.ProcesarIdentificador(token, fila, columna);

                if (tokenReconocido != null)
                {
                    tokens.Add(tokenReconocido); // Agregar el token a la lista
                    return tokenReconocido; // Retorna el token si es un identificador válido
                }

                // 2. Validación de decimales
                AutomataDecimal analizadorDecimal = new AutomataDecimal();
                tokenReconocido = analizadorDecimal.ProcesarDecimal(token, fila, columna);

                if (tokenReconocido != null)
                {
                    tokens.Add(tokenReconocido); // Agregar el token a la lista
                    return tokenReconocido; // Retorna el token si es un número decimal válido
                }                

                // 3. Validación de enteros
                AutomataEntero analizadorEntero = new AutomataEntero();
                tokenReconocido = analizadorEntero.ProcesarEntero(token, fila, columna);

                if (tokenReconocido != null)
                {
                    tokens.Add(tokenReconocido); // Agregar el token a la lista
                    return tokenReconocido; // Retorna el token si es un entero válido
                }

                // 4. Validar palabras reservadas
                AutomataPalabraReservada analizadorPalabraReservada = new AutomataPalabraReservada();
                tokenReconocido = analizadorPalabraReservada.ProcesarPalabraReservada(token, fila, columna);

                if (tokenReconocido != null)
                {
                    tokens.Add(tokenReconocido); // Agregar el token a la lista
                    return tokenReconocido;
                }

                // 5. Validar literales con comillas simples y dobles
                AutomataLiteral analizadorLiteral = new AutomataLiteral();
                tokenReconocido = analizadorLiteral.ProcesarLiteral(token, fila, columna);

                if (tokenReconocido != null)
                {
                    tokens.Add(tokenReconocido); // Agregar el token a la lista
                    return tokenReconocido;
                }

                // 6. Validar signos de puntuación
                AutomataSignosPuntuacion analizadorSignosPuntuacion = new AutomataSignosPuntuacion();
                tokenReconocido = analizadorSignosPuntuacion.ProcesarSignoPuntuacion(token, fila, columna);

                if (tokenReconocido != null)
                {
                    tokens.Add(tokenReconocido); // Agregar el token a la lista
                    return tokenReconocido;
                }

                // 7. Validar un operador aritmético
                AutomataAritmetico analizadorAritmetico = new AutomataAritmetico();
                tokenReconocido = analizadorAritmetico.ProcesarOperadorAritmetico(token, fila, columna);

                if (tokenReconocido != null)
                {
                    tokens.Add(tokenReconocido); // Agregar el token a la lista
                    return tokenReconocido;
                }

                // 8. Validar un operador relacional o de asignación
                AutomataRelacionalAsignacion analizadorRelacionalAsignacion = new AutomataRelacionalAsignacion();
                tokenReconocido = analizadorRelacionalAsignacion.ProcesarOperador(token, fila, columna);

                if (tokenReconocido != null)
                {
                    tokens.Add(tokenReconocido); // Agregar el token a la lista
                    return tokenReconocido;
                }

                // 9. Validar operadores lógicos
                AutomataLogico analizadorLogico = new AutomataLogico();
                tokenReconocido = analizadorLogico.ProcesarOperadorLogico(token, fila, columna);

                if (tokenReconocido != null)
                {
                    tokens.Add(tokenReconocido); // Agregar el token a la lista
                    return tokenReconocido;
                }

                // 10. Validar signos de agrupación
                AutomataAgrupacion analizadorAgrupacion = new AutomataAgrupacion();
                tokenReconocido = analizadorAgrupacion.ProcesarSignoAgrupacion(token, fila, columna);

                if (tokenReconocido != null)
                {
                    tokens.Add(tokenReconocido); // Agregar el token a la lista
                    return tokenReconocido;
                }

                // 11. Validar comentario de una sola línea
                AutomataComentarioSimple analizadorComentarioSimple = new AutomataComentarioSimple();
                tokenReconocido = analizadorComentarioSimple.ProcesarComentarioSimple(token, fila, columna);

                if (tokenReconocido != null)
                {
                    tokens.Add(tokenReconocido); // Agregar el token a la lista
                    return tokenReconocido;
                }

                // 12. Validar comentarios en bloque
                AutomataComentarioBloque analizadorComentarioBloque = new AutomataComentarioBloque();
                tokenReconocido = analizadorComentarioBloque.ProcesarComentarioBloque(token, fila, columna);

                if (tokenReconocido != null)
                {
                    tokens.Add(tokenReconocido); // Agregar el token a la lista
                    return tokenReconocido;
                }

                // 13. Si el token no coincide con ninguno, registrar el error
                errores.Add($"Error: Token no reconocido -> '{token}' en fila {fila}, columna {columna}");
                return null!; // Retorna null si no es válido
            }
            catch (Exception ex)
            {
                errores.Add($"Error durante el análisis del token '{token}' en fila {fila}, columna {columna}: {ex.Message}");
                return null!;
            }
        }

        public List<Token> Tokens
        {
            get { return tokens; }
        }

        public List<string> Errores
        {
            get { return errores; }
        }
    }
}
