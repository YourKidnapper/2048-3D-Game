using UnityEngine;
using TMPro;

public class TileView : MonoBehaviour
{
    [SerializeField] private TextMeshPro[] allTexts;
    [SerializeField] private MeshRenderer meshRenderer;

    public void ApplyVisual(int value)
    {
        string valueText = value.ToString();

        foreach (var text in allTexts)
            text.text = valueText;

        meshRenderer.material.color = GetColorByValue(value);
    }

    private Color GetColorByValue(int value)
    {
        int power = (int)Mathf.Log(value, 2);
        float hue = (power * 0.1f) % 1f;
        return Color.HSVToRGB(hue, 0.6f, 1f);
    }
    
}
