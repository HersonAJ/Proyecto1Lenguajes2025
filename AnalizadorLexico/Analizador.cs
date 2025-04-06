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
        get { return analizadorLexico.Tokens; }
    }

    public List<string> Errores
    {
        get { return analizadorLexico.Errores; }
    }

    public string AnalizarTexto(string texto)
    {
        try
        {
            // Reiniciar tokens y errores
            analizadorLexico.Tokens.Clear();
            analizadorLexico.Errores.Clear();

            // Separar el texto en líneas
            string[] lineas = texto.Split(new[] { '\n' }, StringSplitOptions.None);
            int fila = 1; // Comienza en la fila 1

            List<string> resultado = new List<string>();
            resultado.Add("Tokens reconocidos:");

            // Variables para manejo de comentarios en bloque
            bool dentroComentarioBloque = false;
            string bufferComentarioBloque = string.Empty; //agrupara caracter por caracter
            int filaInicioComentarioBloque = 0;
            int columnaInicioComentarioBloque = 0;

            // Procesar cada línea
            foreach (string linea in lineas)
            {
                int columna = 1;
                string buffer = string.Empty;  // Acumula caracteres para formar lexemas simples
                bool dentroLiteral = false;
                char delimitadorLiteral = '\0';

                for (int i = 0; i < linea.Length; i++) //ciclo for para recorrer toda la entrada 
                {
                    char actual = linea[i]; //lectura de caracter por caracter 

                    if (dentroComentarioBloque)
                    {
                        // Acumular contenido del comentario en bloque
                        bufferComentarioBloque += actual;
                        if (i < linea.Length - 1 && actual == '*' && linea[i + 1] == '/')
                        {
                            // Fin del comentario en bloque: generar lexema completo
                            bufferComentarioBloque += '/';
                            Token tokenBloque = analizadorLexico.AnalizarToken(bufferComentarioBloque, filaInicioComentarioBloque, columnaInicioComentarioBloque);
                            if (tokenBloque != null)
                            {
                                resultado.Add(tokenBloque.ToString());
                            }
                            // Reiniciar variables del comentario en bloque
                            dentroComentarioBloque = false;
                            bufferComentarioBloque = string.Empty;
                            columna += 2; // Avanzar por los dos caracteres "*/"
                            i++;
                        }
                    }
                    else if (dentroLiteral)
                    {
                        // Acumular literal
                        buffer += actual;
                        if (actual == delimitadorLiteral)
                        {
                            // Fin del literal: se genera el lexema completo y se delega su análisis
                            Token tokenReconocido = analizadorLexico.AnalizarToken(buffer, fila, columna);
                            if (tokenReconocido != null)
                            {
                                resultado.Add(tokenReconocido.ToString());
                            }
                            columna += buffer.Length;
                            buffer = string.Empty;
                            dentroLiteral = false;
                            delimitadorLiteral = '\0';
                        }
                    }
                    else if (i < linea.Length - 1 && actual == '/' && linea[i + 1] == '*')
                    {
                        // Inicio de comentario en bloque: genera el lexema parcial, se delega su análisis al completarlo
                        dentroComentarioBloque = true;
                        bufferComentarioBloque = "/*";
                        filaInicioComentarioBloque = fila;
                        columnaInicioComentarioBloque = columna;
                        columna += 2;
                        i++;
                    }
                    else if (actual == '#')
                    {
                        // Si hay contenido en buffer pendiente, generar lexema y delegar su análisis
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

                        // Generar lexema completo para comentario simple desde '#' hasta el final de la línea
                        string comentario = linea.Substring(i).Trim();
                        Token tokenComentario = analizadorLexico.AnalizarToken(comentario, fila, columna);
                        if (tokenComentario != null)
                        {
                            resultado.Add(tokenComentario.ToString());
                        }
                        // Finalizar el procesamiento de la línea
                        break;
                    }
                    else if ("()[]{}".Contains(actual))
                    {
                        // Si hay contenido previo, genera su lexema primero
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

                        // Generar lexema para signo de agrupación
                        Token tokenAgrupacion = analizadorLexico.AnalizarToken(actual.ToString(), fila, columna);
                        if (tokenAgrupacion != null)
                        {
                            resultado.Add(tokenAgrupacion.ToString());
                        }
                        columna++;
                    }
                    else if (".,;:".Contains(actual))
                    {
                        // Caso especial: el punto puede pertenecer a un número decimal
                        if (actual == '.' && !string.IsNullOrWhiteSpace(buffer) &&
                            char.IsDigit(buffer[buffer.Length - 1]) &&
                            i < linea.Length - 1 &&
                            char.IsDigit(linea[i + 1]))
                        {
                            buffer += actual;
                            continue;
                        }

                        // Si hay contenido en buffer, generar lexema antes de procesar el signo
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

                        // Generar lexema para signo de puntuación
                        Token tokenPuntuacion = analizadorLexico.AnalizarToken(actual.ToString(), fila, columna);
                        if (tokenPuntuacion != null)
                        {
                            resultado.Add(tokenPuntuacion.ToString());
                        }
                        columna++;
                    }
                    else if (actual == '"' || actual == '\'')
                    {
                        // Inicio de un literal: acumula el delimitador y activa modo literal
                        dentroLiteral = true;
                        delimitadorLiteral = actual;
                        buffer += actual;
                    }
                    else if (actual == ' ' || actual == '\t')
                    {
                        // Espacio o tabulación indica fin de lexema: si hay contenido, generar lexema
                        if (!string.IsNullOrWhiteSpace(buffer))
                        {
                            Token tokenReconocido = analizadorLexico.AnalizarToken(buffer, fila, columna);
                            if (tokenReconocido != null)
                            {
                                resultado.Add(tokenReconocido.ToString());
                            }
                            columna += buffer.Length + 1;
                            buffer = string.Empty;
                        }
                        else
                        {
                            columna++;
                        }
                    }
                    else
                    {
                        // Acumular caracter para formar un lexema
                        buffer += actual;
                    }
                } // Fin de procesamiento de la línea

                // Al finalizar la línea, si existe un buffer pendiente, generar el lexema
                if (!string.IsNullOrWhiteSpace(buffer) && !dentroComentarioBloque)
                {
                    Token tokenReconocido = analizadorLexico.AnalizarToken(buffer, fila, columna);
                    if (tokenReconocido != null)
                    {
                        resultado.Add(tokenReconocido.ToString());
                    }
                }
                fila++;
            }

            // Después de procesar todas las líneas, agregar los errores (si existiesen)
            if (analizadorLexico.Errores.Count > 0)
            {
                resultado.Add("\nErrores encontrados:");
                foreach (string error in analizadorLexico.Errores)
                {
                    resultado.Add(error);
                }
            }

            return string.Join("\n", resultado);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error inesperado: {ex.Message}");
            return "Se produjo un error";
        }
    }
}
