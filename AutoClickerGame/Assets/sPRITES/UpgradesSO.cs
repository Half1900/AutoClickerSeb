using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

[CreateAssetMenu(fileName ="NewUpgrade",menuName ="Tools/Upgrades")]
public class UpgradesSO : ScriptableObject
{
    public string name;
    public float cost;
    public float clickPerSecondBonus;
    public float clickPerTouchBonus;
    public TypeUpgrade TypeUpgrade = TypeUpgrade.None;
}
public enum TypeUpgrade
{
    None,
    Second,
    Touch
}
