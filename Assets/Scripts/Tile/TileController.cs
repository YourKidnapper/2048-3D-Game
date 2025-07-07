using UnityEngine;

public class TileController : MonoBehaviour
{
    [SerializeField] private float launchForce = 10f;
    [SerializeField] private float mergeJumpForce = 3f;
    [SerializeField] private float requiredImpulseToMerge = 0.5f;

    private TileView tileView;
    private Rigidbody rb;
    private int tileValue;
    private bool hasLaunched = false;
    private bool readyToMerge = true;
    public bool HasLaunched => hasLaunched;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        tileView = GetComponent<TileView>();
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

        if (!readyToMerge || !otherTile.readyToMerge) return;
        if (tileValue != otherTile.tileValue) return;

        float impulseMagnitude = collision.impulse.magnitude;
        if (impulseMagnitude < requiredImpulseToMerge)
        {
            Debug.Log("Impulse too low for merge: " + impulseMagnitude);
            return;
        }

        MergeWith(otherTile);
    }

    private void MergeWith(TileController other)
    {
        tileValue *= 2;
        SetValue(tileValue);
        rb.AddForce(Vector3.up * mergeJumpForce, ForceMode.Impulse);
        Destroy(other.gameObject);
        readyToMerge = false;
        Invoke(nameof(EnableMerge), 0.1f);
    }

    private void EnableMerge()
    {
        readyToMerge = true;
    }

    public bool IsStopped()
    {
        return rb.linearVelocity.magnitude < 0.05f;
    }
}
