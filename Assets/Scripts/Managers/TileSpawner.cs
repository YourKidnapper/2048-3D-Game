using UnityEngine;

public class TileSpawner : MonoBehaviour
{
   [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Transform tileParent;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnDelay = 0.5f;

    private bool canSpawn = true;

    private void Start()
    {
        SpawnNewTile();
    }

    public bool CanSpawn => canSpawn;

    public GameObject SpawnNewTile()
    {
        if (!canSpawn) return null;

        canSpawn = false;
        Invoke(nameof(EnableSpawn), spawnDelay);

        GameObject newTile = Instantiate(tilePrefab, spawnPoint.position, Quaternion.identity, tileParent);

        if (newTile.TryGetComponent(out TileController controller) &&
            newTile.TryGetComponent(out TileView view))
        {
            int value = Random.value < 0.75f ? 2 : 4;
            controller.Init(this, value);
            view.ApplyVisual(value);
            return newTile;
        }

        Debug.LogError("Tile prefab missing required components");
        Destroy(newTile);
        return null;
    }

    private void EnableSpawn() => canSpawn = true;

}
