using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(fileName ="NewUpgrade",menuName ="Tools/Upgrades"), System.Serializable]
public class UpgradesSO : ScriptableObject
{
    public string nombre;
    public float cost;
    public float clickPerSecondBonus;
    public float clickPerTouchBonus;
    public bool Activado = false; 
    /*public Mejoras nivel = Mejoras.Uno;
    public void AumentarNivel()
    {
        if (nivel == Mejoras.Uno)
        {
            cost *= 1.5f;
            nivel = Mejoras.Dos;
        }else if (nivel == Mejoras.Dos)
        {
            cost *= 1.5f;
            nivel = Mejoras.Tres;
        }else if (nivel == Mejoras.Tres)
        {
            cost *= 1.5f;
            nivel = Mejoras.Cuatro;
        }else if (nivel == Mejoras.Cuatro)
        {
            cost *= 1.5f;
            nivel = Mejoras.Termino;
        }
    }*/
}
public enum Mejoras
{
    Uno,
    Dos,
    Tres,
    Cuatro,
    Termino
}
