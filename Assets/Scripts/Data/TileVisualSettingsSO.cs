using UnityEngine;

[CreateAssetMenu(fileName = "TileVisualSettings", menuName = "2048/TileVisualSettings")]
public class TileVisualSettingsSO : ScriptableObject
{
    public int value;
    public Color color;
    public string displayText;
    public Material material;
}
