using UnityEngine;
using TMPro;

public class TileView : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private TextMeshPro text;

    public void ApplyVisual(int value)
    {
        meshRenderer.material.color = GetColorByValue(value);
        text.text = value.ToString();
    }

    private Color GetColorByValue(int value)
    {
        int power = (int)Mathf.Log(value, 2);

        float hue = (power * 0.1f) % 1f;
        float saturation = 0.6f;
        float valueBrightness = 1f;

        return Color.HSVToRGB(hue, saturation, valueBrightness);
    }
}
