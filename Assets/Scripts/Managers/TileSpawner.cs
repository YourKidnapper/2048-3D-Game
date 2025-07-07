using System;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Transform tileParent;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnDelay = 0.5f;

    private bool canSpawn = true;
    private bool spawnRequested = false;

    public event Action<GameObject> OnTileSpawned;

    public void RequestSpawn()
    {
        if (canSpawn)
        {
            SpawnNewTile();
        }
        else
        {
            spawnRequested = true;
            Debug.Log("Spawn requested — waiting for delay...");
        }
    }

    private void SpawnNewTile()
    {
        canSpawn = false;
        GameObject newTile = Instantiate(tilePrefab, spawnPoint.position, Quaternion.identity, tileParent);

        if (newTile.TryGetComponent(out TileController controller) &&
            newTile.TryGetComponent(out TileView view))
        {
            int value = UnityEngine.Random.value < 0.75f ? 2 : 4;
            controller.Init(this, value);
            view.ApplyVisual(value);
            OnTileSpawned?.Invoke(newTile);
        }
        else
        {
            Debug.LogError("Tile prefab is missing required components");
            Destroy(newTile);
        }

        Invoke(nameof(EnableSpawn), spawnDelay);
    }

    private void EnableSpawn()
    {
        canSpawn = true;

        if (spawnRequested)
        {
            spawnRequested = false;
            SpawnNewTile();
        }
    }
}
