using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerRects : MonoBehaviour
{
    public RectTransform Ajolote;
    public RectTransform UI;

    /*private void Start()
    {
        RectTransformHelper.AjustarPosicionPorTamañoDePantalla(Ajolote,new Vector2(266.8488f, -80.8119f),new Vector2(292.2957f, -80.8119f),new Vector2(379.6707f, -80.8119f),new Vector2(728.1925f, -80.8119f));
        RectTransformHelper.AjustarPosicionPorTamañoDePantalla(UI,new Vector2(-32.95801f, -47.90137f),new Vector2(-4.954315f, -47.90137f),new Vector2(85.56201f, -47.90137f),new Vector2(434.0838f, -47.90137f));
    }*/

    private void Update()
    {
        RectTransformHelper.AjustarPosicionPorTamañoDePantalla(Ajolote, new Vector2(266.8488f, -80.8119f), new Vector2(292.2957f, -80.8119f), new Vector2(379.6707f, -80.8119f), new Vector2(728.1925f, -80.8119f));
        RectTransformHelper.AjustarPosicionPorTamañoDePantalla(UI, new Vector2(-32.95801f, -47.90137f), new Vector2(-4.954315f, -47.90137f), new Vector2(85.56201f, -47.90137f), new Vector2(434.0838f, -47.90137f));
    }
}
