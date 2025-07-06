using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Transform tileParent;
    [SerializeField] private Transform spawnPoint;

    private void Start()
    {
        SpawnNewTile(spawnPoint.position);
    }

    public void SpawnNewTile(Vector3 position)
    {
        GameObject newTile = Instantiate(tilePrefab, spawnPoint.position, Quaternion.identity, tileParent);

        TileController controller = newTile.GetComponent<TileController>();
        controller.Init(this);

        int value = Random.value < 0.75f ? 2 : 4;
        newTile.GetComponent<TileView>().ApplyVisual(value);
    }

}
