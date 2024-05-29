using UnityEngine;
[CreateAssetMenu(fileName ="NewSkin",menuName ="Tools/Skins")]
public class SkinSO : ScriptableObject
{
    public Sprite sprite;
    public string nombre;
    public float cost;
    public int activo;
}
