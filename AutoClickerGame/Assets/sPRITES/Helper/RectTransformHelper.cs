using UnityEngine;

public static class RectTransformHelper
{
    public static void AjustarPosicionPorTamañoDePantalla(RectTransform rectTransform, Vector2 posicionMenorA800x480, Vector2 posicionMenorA1920x1080, Vector2 posicionMenorA2160x1080)
    {
        // Obtener el tamaño de la pantalla
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Definir las dimensiones de pantalla para cada caso
        Vector2 dimensiones800x480 = new Vector2(800, 480);
        Vector2 dimensiones1920x1080 = new Vector2(1920, 1080);
        Vector2 dimensiones2160x1080 = new Vector2(2160, 1080);

        // Determinar la posición según el tamaño de la pantalla
        if (screenWidth <= dimensiones800x480.x || screenHeight <= dimensiones800x480.y)
        {
            rectTransform.anchoredPosition = posicionMenorA800x480;
        }
        else if (screenWidth <= dimensiones1920x1080.x || screenHeight <= dimensiones1920x1080.y)
        {
            rectTransform.anchoredPosition = posicionMenorA1920x1080;
        }
        else if (screenWidth <= dimensiones2160x1080.x || screenHeight <= dimensiones2160x1080.y)
        {
            rectTransform.anchoredPosition = posicionMenorA2160x1080;
        }
    }
}
