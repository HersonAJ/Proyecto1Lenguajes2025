using System;
using Logica;
using Analizadores;

public class Analizador
{
    public string AnalizarTexto(string texto)
    {
        try
        {
        string resultado = ""; // Resultado acumulado de los tokens analizados
        AnalizadorLexico analizador = new AnalizadorLexico();

        int posicionActual = 0; // Posición actual del carácter analizado
        bool dentroDeLiteral = false; // Indica si estamos dentro de un literal
        char comillaInicio = '\0'; // Delimitador de inicio de literal
        string tokenActual = ""; // Acumula el token en construcción

        // Bucle principal para analizar el texto carácter por carácter
        for (int i = 0; i < texto.Length; i++)
        {
            char caracter = texto[i];

            if (dentroDeLiteral)
            {
                // *** Análisis de Literales ***
                // Estamos dentro de un literal, acumulamos caracteres hasta encontrar la comilla de cierre
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

                    // Reiniciar el estado para salir del literal
                    dentroDeLiteral = false;
                    comillaInicio = '\0';
                    tokenActual = "";
                    posicionActual = i + 1;
                }
            }
            else
            {
                // *** Inicio del análisis de comentarios (simples y en bloque) ***
                if (caracter == '#' || (caracter == '/' && i + 1 < texto.Length && texto[i + 1] == '*'))
                {
                    // Comentarios simples
                    if (caracter == '#')
                    {
                        while (i < texto.Length && texto[i] != '\n')
                        {
                            tokenActual += texto[i];
                            i++;
                        }

                        // Procesar el comentario completo como un token
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
                    // Comentarios en bloque
                    else if (caracter == '/' && texto[i + 1] == '*')
                    {
                        tokenActual += "/*"; // Agregar inicio del comentario
                        i += 2; // Avanzar después de /*

                        while (i < texto.Length)
                        {
                            tokenActual += texto[i];
                            if (texto[i] == '*' && i + 1 < texto.Length && texto[i + 1] == '/')
                            {
                                // Fin del comentario en bloque
                                tokenActual += '/';
                                i++;
                                break;
                            }
                            i++;
                        }

                        // Procesar el comentario en bloque como un token
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
                // *** Inicio del análisis de literales ***
                else if (caracter == '"' || caracter == '\'') // Identifica inicio de un literal
                {
                    dentroDeLiteral = true;
                    comillaInicio = caracter;
                    tokenActual += caracter; // Incluir la comilla inicial
                }
                // *** Manejo de separadores (espacios en blanco) ***
                else if (char.IsWhiteSpace(caracter))
                {
                    // Procesar token acumulado antes del espacio
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
                // *** Manejo de signos de agrupación ***
                else if ("()[]{}".Contains(caracter))
                {
                    // Procesar el token acumulado antes del signo
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

                    // Procesar el signo de agrupación como un token individual
                    Token signoToken = analizador.AnalizarToken(caracter.ToString(), i);
                    if (signoToken != null)
                    {
                        resultado += $"Token: {signoToken.Tipo}, Valor: {signoToken.Valor}, Posición: {i}\n";
                    }
                    else
                    {
                        resultado += $"Error: Token no reconocido -> '{caracter}' en posición {i}\n";
                    }

                    posicionActual = i + 1;
                }
                // *** Acumulación de otros tokens (identificadores, números, etc.) ***
                else
                {
                    tokenActual += caracter;
                }
            }
        }

        // *** Procesar cualquier token sobrante ***
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
    catch (Exception ex)
    {
        //si algo falla
        return $"Error critico durante el analisis: {ex.Message}";
    }
}
}