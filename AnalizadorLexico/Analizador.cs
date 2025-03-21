using System;
using Logica;
using Analizadores;

public class Analizador
{
    public string AnalizarTexto(string texto)
    {
        string resultado = ""; // Resultado acumulado
        AnalizadorLexico analizador = new AnalizadorLexico();

        int posicionActual = 0;
        bool dentroDeLiteral = false;
        char comillaInicio = '\0';
        string tokenActual = "";

        for (int i = 0; i < texto.Length; i++)
        {
            char caracter = texto[i];

            if (dentroDeLiteral)
            {
                // Acumular caracteres dentro del literal
                tokenActual += caracter;

                if (caracter == comillaInicio) // Comilla de cierre encontrada
                {
                    // Procesar el literal completo
                    Token resultadoToken = analizador.AnalizarToken(tokenActual, posicionActual);
                    if (resultadoToken != null)
                    {
                        // Si es válido, agregar al resultado
                        resultado += $"Token: {resultadoToken.Tipo}, Valor: {resultadoToken.Valor}, Posición: {resultadoToken.Posicion}\n";
                    }
                    else
                    {
                        // Si no es válido, agregar un error
                        resultado += $"Error: Literal no válido -> {tokenActual} en posición {posicionActual}\n";
                    }

                    // Reiniciar el estado
                    dentroDeLiteral = false;
                    comillaInicio = '\0';
                    tokenActual = "";
                    posicionActual = i + 1;
                }
            }
            else
            {
                if (caracter == '#' || (caracter == '/' && i + 1 < texto.Length && texto[i + 1] == '*'))
                {
                    // Detecta el inicio de un comentario
                    if (caracter == '#') // Comentario de una sola línea
                    {
                        while (i < texto.Length && texto[i] != '\n')
                        {
                            tokenActual += texto[i];
                            i++;
                        }

                        // Procesar el comentario completo
                        Token resultadoToken = analizador.AnalizarToken(tokenActual, posicionActual);
                        if (resultadoToken != null)
                        {
                            resultado += $"Token: {resultadoToken.Tipo}, Valor: {resultadoToken.Valor}, Posición: {posicionActual}\n";
                        }
                        else
                        {
                            resultado += $"Error: Comentario no válido -> {tokenActual} en posición {posicionActual}\n";
                        }

                        // Reiniciar el token y posición
                        tokenActual = "";
                        posicionActual = i;
                    }
                    else if (caracter == '/' && texto[i + 1] == '*') // Comentario en bloque
                    {
                        // Captura el inicio del comentario bloque
                        tokenActual += "/*";
                        i += 2; // Avanza después de /*

                        while (i < texto.Length)
                        {
                            tokenActual += texto[i];
                            if (texto[i] == '*' && i + 1 < texto.Length && texto[i + 1] == '/')
                            {
                                // Fin del comentario en bloque
                                tokenActual += '/';
                                i++; // Avanzar después de */
                                break;
                            }
                            i++;
                        }

                        // Procesar el comentario completo
                        Token resultadoToken = analizador.AnalizarToken(tokenActual, posicionActual);
                        if (resultadoToken != null)
                        {
                            resultado += $"Token: {resultadoToken.Tipo}, Valor: {resultadoToken.Valor}, Posición: {posicionActual}\n";
                        }
                        else
                        {
                            resultado += $"Error: Comentario en bloque no válido -> {tokenActual} en posición {posicionActual}\n";
                        }

                        // Reiniciar el token y posición
                        tokenActual = "";
                        posicionActual = i + 1;
                    }
                }
                else if (caracter == '"' || caracter == '\'') // Inicio de un literal
                {
                    dentroDeLiteral = true;
                    comillaInicio = caracter;
                    tokenActual += caracter; // Incluir la comilla inicial
                }
                else if (char.IsWhiteSpace(caracter))
                {
                    // Procesar token acumulado fuera del literal
                    if (!string.IsNullOrEmpty(tokenActual))
                    {
                        Token resultadoToken = analizador.AnalizarToken(tokenActual, posicionActual);
                        if (resultadoToken != null)
                        {
                            resultado += $"Token: {resultadoToken.Tipo}, Valor: {resultadoToken.Valor}, Posición: {resultadoToken.Posicion}\n";
                        }
                        else
                        {
                            resultado += $"Error: Token no reconocido -> '{tokenActual}' en posición {posicionActual}\n";
                        }
                        tokenActual = ""; // Reiniciar el token
                    }
                    posicionActual = i + 1; // Actualizar posición
                }
                else
                {
                    // Acumular caracteres para el token actual
                    tokenActual += caracter;
                }
            }
        }

        // Procesar cualquier token sobrante
        if (!string.IsNullOrEmpty(tokenActual))
        {
            Token resultadoToken = analizador.AnalizarToken(tokenActual, posicionActual);
            if (resultadoToken != null)
            {
                resultado += $"Token: {resultadoToken.Tipo}, Valor: {resultadoToken.Valor}, Posición: {resultadoToken.Posicion}\n";
            }
            else
            {
                resultado += $"Error: Token no reconocido -> '{tokenActual}' en posición {posicionActual}\n";
            }
        }

        return resultado; // Devuelve el resultado del análisis como una cadena
    }
}
