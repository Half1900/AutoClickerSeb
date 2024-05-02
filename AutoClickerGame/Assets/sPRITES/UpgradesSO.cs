using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
[CreateAssetMenu(fileName ="NewUpgrade",menuName ="Tools/Upgrades"), System.Serializable]
public class UpgradesSO : ScriptableObject
{
    public string nombre;
    public float cost;
    public float clickPerSecondBonus;
    public float clickPerTouchBonus;
    public bool Activado = false; 
}
