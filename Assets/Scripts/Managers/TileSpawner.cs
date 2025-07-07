using UnityEngine;
using System;

public class TileSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerTilePrefab;
    [SerializeField] private GameObject staticTilePrefab;

    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private Transform[] staticTilePositions;

    [SerializeField] private float spawnCooldown = 0.3f;

    private float spawnTimer = 0f;
    private bool spawnRequested = false;

    public event Action<GameObject> OnTileSpawned;

    private void Start()
    {
        SpawnStaticTiles();
    }

    private void Update()
    {
        if (spawnRequested)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0f)
            {
                spawnRequested = false;
                SpawnPlayerTile();
            }
        }
    }

    public void RequestPlayerTile()
    {
        if (spawnRequested) return;

        spawnRequested = true;
        spawnTimer = spawnCooldown;
    }

    private void SpawnPlayerTile()
    {
        GameObject tileObj = Instantiate(playerTilePrefab, playerSpawnPoint.position, Quaternion.identity);
        PlayerTile tile = tileObj.GetComponent<PlayerTile>();

        int value = UnityEngine.Random.value < 0.75f ? 2 : 4;
        tile.SetValue(value);

        GameManager.Instance.RegisterTile(tile);
        OnTileSpawned?.Invoke(tileObj);
    }

    private void SpawnStaticTiles()
    {
        foreach (var pos in staticTilePositions)
        {
            GameObject tileObj = Instantiate(staticTilePrefab, pos.position, Quaternion.identity);
            StaticTile tile = tileObj.GetComponent<StaticTile>();

            int value = UnityEngine.Random.Range(0, 2) == 0 ? 2 : 4;
            tile.SetValue(value);
        }
    }
}
