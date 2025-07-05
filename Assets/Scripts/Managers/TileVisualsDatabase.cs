using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileVisualsDatabase", menuName = "2048/TileVisualsDatabase")]
public class TileVisualsDatabase : ScriptableObject
{
    public List<TileVisualSettingsSO> tileSettings;

    private Dictionary<int, TileVisualSettingsSO> lookup;

    public TileVisualSettingsSO GetVisuals(int value)
    {
        if (lookup == null)
        {
            lookup = new Dictionary<int, TileVisualSettingsSO>();
            foreach (var setting in tileSettings)
            {
                lookup[setting.value] = setting;
            }
        }

        if (lookup.TryGetValue(value, out var result))
            return result;

        Debug.LogWarning($"No visuals found for tile value: {value}");
        return null;
    }
}
