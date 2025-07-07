using TMPro;
using UnityEngine;

public class TileCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tileCountText;

    public void UpdateTileCount(int count, int max)
    {
        tileCountText.text = $"Tiles: {count} / {max}";
    }
}