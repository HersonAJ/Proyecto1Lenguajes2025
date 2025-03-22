using System;
using System.Collections.Generic;
using Analizadores;
using Logica;

public class Analizador
{
    private AnalizadorLexico analizadorLexico;

    public Analizador()
    {
        analizadorLexico = new AnalizadorLexico();
    }

    public List<Token> Tokens
    {
        get { return analizadorLexico.Tokens;}
    }

    // Método para analizar el texto completo
    public string AnalizarTexto(string texto)
    {
        // Divide el texto en líneas para análisis por fila
        string[] lineas = texto.Split(new[] { '\n' }, StringSplitOptions.None);
        int fila = 1; // Comienza en la fila 1

        // Lista de resultado final
        List<string> resultado = new List<string>();

        // Mostrar encabezado de tokens reconocidos
        resultado.Add("Tokens reconocidos:");

        bool dentroComentarioBloque = false; // Indica si estamos dentro de un comentario en bloque
        string bufferComentarioBloque = string.Empty; // Acumular el contenido del comentario en bloque
        int filaInicioComentarioBloque = 0; // Fila donde empieza el comentario en bloque
        int columnaInicioComentarioBloque = 0; // Columna donde empieza el comentario en bloque

        foreach (string linea in lineas)
        {
            int columna = 1; // Reiniciar columna al inicio de cada línea
            string buffer = string.Empty;
            bool dentroLiteral = false; // Para rastrear si estamos dentro de un literal
            char delimitadorLiteral = '\0'; // Para rastrear el tipo de comilla (' o ")

            for (int i = 0; i < linea.Length; i++)
            {
                char actual = linea[i];

                // Manejo de comentario en bloque
                if (dentroComentarioBloque)
                {
                    bufferComentarioBloque += actual; // Agregar al buffer del comentario
                    if (i < linea.Length - 1 && actual == '*' && linea[i + 1] == '/')
                    {
                        // Fin del comentario en bloque
                        bufferComentarioBloque += '/';
                        resultado.Add(new Token("ComentarioBloque", bufferComentarioBloque, filaInicioComentarioBloque, columnaInicioComentarioBloque).ToString());
                        dentroComentarioBloque = false;
                        bufferComentarioBloque = string.Empty;
                        columna += 2; // Avanzar dos posiciones por "*/"
                        i++; // Saltar el siguiente carácter
                    }
                }
                else if (dentroLiteral)
                {
                    buffer += actual; // Agregar el carácter al buffer del literal

                    // Verificar si se cierra el literal
                    if (actual == delimitadorLiteral)
                    {
                        // Crear un token para el literal y reiniciar el estado
                        Token tokenReconocido = analizadorLexico.AnalizarToken(buffer, fila, columna);
                        if (tokenReconocido != null)
                        {
                            resultado.Add(tokenReconocido.ToString()); // Agregar token válido
                        }

                        columna += buffer.Length; // Avanzar columna según la longitud del literal
                        buffer = string.Empty;
                        dentroLiteral = false;
                        delimitadorLiteral = '\0';
                    }
                }
                else if (i < linea.Length - 1 && actual == '/' && linea[i + 1] == '*')
                {
                    // Inicio de un comentario en bloque
                    dentroComentarioBloque = true;
                    bufferComentarioBloque = "/*";
                    filaInicioComentarioBloque = fila;
                    columnaInicioComentarioBloque = columna;
                    columna += 2; // Avanzar dos posiciones por "/*"
                    i++; // Saltar el siguiente carácter
                }
                else if (linea.TrimStart().StartsWith("#"))
                {
                    // Comentario simple
                    resultado.Add(new Token("ComentarioSimple", linea.Trim(), fila, columna).ToString());
                    break; // No procesar más en esta línea
                }
                else if ("()[]{}".Contains(actual)) // Manejo de signos de agrupación
                {
                    // Procesar el buffer acumulado antes del signo de agrupación
                    if (!string.IsNullOrWhiteSpace(buffer))
                    {
                        Token tokenReconocido = analizadorLexico.AnalizarToken(buffer, fila, columna);
                        if (tokenReconocido != null)
                        {
                            resultado.Add(tokenReconocido.ToString());
                        }
                        columna += buffer.Length;
                        buffer = string.Empty;
                    }

                    // Crear un token para el signo de agrupación
                    Token tokenAgrupacion = analizadorLexico.AnalizarToken(actual.ToString(), fila, columna);
                    if (tokenAgrupacion != null)
                    {
                        resultado.Add(tokenAgrupacion.ToString());
                    }

                    columna++; // Avanzar la columna para el signo de agrupación
                }
                else if (actual == '"' || actual == '\'')
                {
                    // Verificar inicio de un literal
                    dentroLiteral = true;
                    delimitadorLiteral = actual;
                    buffer += actual; // Agregar el delimitador inicial al buffer
                }
                else if (actual == ' ' || actual == '\t') // Separadores
                {
                    if (!string.IsNullOrWhiteSpace(buffer))
                    {
                        // Procesar cualquier token acumulado antes del espacio/tab
                        Token tokenReconocido = analizadorLexico.AnalizarToken(buffer, fila, columna);
                        if (tokenReconocido != null)
                        {
                            resultado.Add(tokenReconocido.ToString());
                        }
                        columna += buffer.Length + 1; // Avanzar columna según el token y el espacio
                        buffer = string.Empty;
                    }
                    else
                    {
                        columna++; // Avanzar por espacio/tab vacío
                    }
                }
                else
                {
                    buffer += actual; // Continuar acumulando caracteres para un token
                }
            }

            // Procesar cualquier token restante en el buffer al final de la línea
            if (!string.IsNullOrWhiteSpace(buffer) && !dentroComentarioBloque)
            {
                Token tokenReconocido = analizadorLexico.AnalizarToken(buffer, fila, columna);
                if (tokenReconocido != null)
                {
                    resultado.Add(tokenReconocido.ToString());
                }
            }

            fila++; // Avanzar a la siguiente fila
        }

        // Mostrar errores encontrados
        if (analizadorLexico.Errores.Count > 0)
        {
            resultado.Add("\nErrores encontrados:");
            foreach (string error in analizadorLexico.Errores)
            {
                resultado.Add(error);
            }
        }

        return string.Join("\n", resultado); // Devolver el resultado como cadena
    }
}
