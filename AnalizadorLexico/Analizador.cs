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
            analizadorLexico.Tokens.Clear();
            analizadorLexico.Errores.Clear();

            string[] lineas = texto.Split(new[] { '\n' }, StringSplitOptions.None);
            int fila = 1; // Comienza en la fila 1

            List<string> resultado = new List<string>();
            resultado.Add("Tokens reconocidos:");

            bool dentroComentarioBloque = false;
            string bufferComentarioBloque = string.Empty;
            int filaInicioComentarioBloque = 0;
            int columnaInicioComentarioBloque = 0;

            foreach (string linea in lineas)
            {
                int columna = 1;
                string buffer = string.Empty;
                bool dentroLiteral = false;
                char delimitadorLiteral = '\0';

                for (int i = 0; i < linea.Length; i++)
                {
                    char actual = linea[i];

                    if (dentroComentarioBloque)
                    {
                        bufferComentarioBloque += actual;
                        if (i < linea.Length - 1 && actual == '*' && linea[i + 1] == '/')
                        {
                            bufferComentarioBloque += '/';
                            Token tokenBloque = new Token("ComentarioBloque", bufferComentarioBloque, filaInicioComentarioBloque, columnaInicioComentarioBloque);
                            resultado.Add(tokenBloque.ToString());
                            analizadorLexico.Tokens.Add(tokenBloque);
                            dentroComentarioBloque = false;
                            bufferComentarioBloque = string.Empty;
                            columna += 2;
                            i++;
                        }
                    }
                    else if (dentroLiteral)
                    {
                        buffer += actual;

                        if (actual == delimitadorLiteral)
                        {
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
                        dentroComentarioBloque = true;
                        bufferComentarioBloque = "/*";
                        filaInicioComentarioBloque = fila;
                        columnaInicioComentarioBloque = columna;
                        columna += 2;
                        i++;
                    }
                    else if (actual == '#')
                    {
                        // Procesar cualquier buffer acumulado antes del comentario
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

                        // Crear el token para el comentario simple
                        string comentario = linea.Substring(i).Trim(); // Extraer el comentario desde '#'
                        Token tokenComentario = new Token("ComentarioSimple", comentario, fila, columna);
                        resultado.Add(tokenComentario.ToString());
                        analizadorLexico.Tokens.Add(tokenComentario);
                        break; // Terminar procesamiento de la línea, ya que todo lo posterior es parte del comentario
                    }
                    else if ("()[]{}".Contains(actual))
                    {
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

                        Token tokenAgrupacion = analizadorLexico.AnalizarToken(actual.ToString(), fila, columna);
                        if (tokenAgrupacion != null)
                        {
                            resultado.Add(tokenAgrupacion.ToString());
                        }

                        columna++;
                    }
                    else if (".,;:".Contains(actual))
                    {
                        if (actual == '.' && !string.IsNullOrWhiteSpace(buffer) &&
                            char.IsDigit(buffer[buffer.Length - 1]) &&
                            i < linea.Length - 1 &&
                            char.IsDigit(linea[i + 1]))
                        {
                            buffer += actual;
                            continue;
                        }

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

                        Token tokenPuntuacion = analizadorLexico.AnalizarToken(actual.ToString(), fila, columna);
                        if (tokenPuntuacion != null)
                        {
                            resultado.Add(tokenPuntuacion.ToString());
                        }
                        columna++;
                    }
                    else if (actual == '"' || actual == '\'')
                    {
                        dentroLiteral = true;
                        delimitadorLiteral = actual;
                        buffer += actual;
                    }
                    else if (actual == ' ' || actual == '\t')
                    {
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
                        buffer += actual;
                    }
                }

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
