using UnityEngine;

public class TileController : MonoBehaviour
{
    [SerializeField] private float launchForce = 10f;
    [SerializeField] private Rigidbody rb;

    private TileSpawner tileSpawner;
    private bool hasLaunched = false;

    void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();
    }

    public void Init(TileSpawner spawner)
    {
        tileSpawner = spawner;
    }

    public void Launch()
    {
        if (hasLaunched) return;

        rb.AddForce(Vector3.left * launchForce, ForceMode.Impulse); // Negative X axis
        hasLaunched = true;

        tileSpawner.SpawnNewTile(transform.position);
    }
}
