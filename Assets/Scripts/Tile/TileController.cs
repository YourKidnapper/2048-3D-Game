using UnityEngine;

public class TileController : MonoBehaviour
{
     [SerializeField] private float launchForce = 10f;
    [SerializeField] private float mergeJumpForce = 3f;
    [SerializeField] private float mergeCheckRadius = 1f;

    private TileView tileView;
    private Rigidbody rb;
    private int tileValue;
    private bool hasLaunched = false;
    private bool readyToMerge = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        tileView = GetComponent<TileView>();
    }

    private void Update()
    {
        if (!readyToMerge || rb == null) return;

        if (rb.linearVelocity.magnitude < 0.05f)
            CheckForAdditionalMerges();
    }

    public void Init(TileSpawner spawner, int value)
    {
        SetValue(value);
    }

    public void Launch()
    {
        if (hasLaunched) return;

        rb.AddForce(Vector3.left * launchForce, ForceMode.Impulse);
        hasLaunched = true;
    }

    public void SetValue(int value)
    {
        tileValue = value;
        tileView.ApplyVisual(value);
    }

    private void OnCollisionEnter(Collision collision)
    {
        TileController otherTile = collision.gameObject.GetComponent<TileController>();

        if (otherTile == null || otherTile == this) return;

        if (tileValue == otherTile.tileValue && readyToMerge && otherTile.readyToMerge)
            MergeWith(otherTile);
    }

    private void MergeWith(TileController other)
    {
        tileValue *= 2;
        SetValue(tileValue);
        rb.AddForce(Vector3.up * mergeJumpForce, ForceMode.Impulse);
        Destroy(other.gameObject);
        Invoke(nameof(CheckForAdditionalMerges), 0.1f);
    }

    private void CheckForAdditionalMerges()
    {
        Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, mergeCheckRadius);

        foreach (var col in nearbyColliders)
        {
            TileController neighbor = col.GetComponent<TileController>();

            if (neighbor != null && neighbor != this &&
                neighbor.tileValue == tileValue &&
                neighbor.readyToMerge)
            {
                MergeWith(neighbor);
                return;
            }
        }

        readyToMerge = true;
    }
}
