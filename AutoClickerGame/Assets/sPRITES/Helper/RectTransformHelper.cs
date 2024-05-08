using UnityEngine;

public static class RectTransformHelper
{
    public static void AjustarPosicionPorTamañoDePantalla(RectTransform rectTransform, Vector2 posicionMenorA800x480, Vector2 posicionMenorA1920x1080, Vector2 posicionMenorA2960x1440, Vector2 posicionMayorATodos)
    {
        // Obtener el tamaño de la pantalla
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Definir las dimensiones de pantalla para cada caso
        Vector2 dimensiones800x480 = new Vector2(800f, 480f);
        Vector2 dimensiones1920x1080 = new Vector2(1920f, 1080f);
        Vector2 dimensiones2960x1440 = new Vector2(2960f, 1440f);

        // Determinar la posición según el tamaño de la pantalla
        if (screenWidth <= dimensiones800x480.x || screenHeight <= dimensiones800x480.y)
        {
            rectTransform.anchoredPosition = posicionMenorA800x480;
        }
        else if (screenWidth <= dimensiones1920x1080.x || screenHeight <= dimensiones1920x1080.y)
        {
            rectTransform.anchoredPosition = posicionMenorA1920x1080;
        }
        else if (screenWidth <= dimensiones2960x1440.x || screenHeight <= dimensiones2960x1440.y)
        {
            rectTransform.anchoredPosition = posicionMenorA2960x1440;
        }
    }
}
