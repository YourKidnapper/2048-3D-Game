using UnityEngine;

public class PlayerTile : BaseTile
{
    [SerializeField] private float launchForce = 10f;
    [SerializeField] private float mergeJumpForce = 3f;
    [SerializeField] private float requiredImpulseToMerge = 0.5f;

    private bool hasLaunched = false;
    private bool readyToMerge = true;

    public bool HasLaunched => hasLaunched;

    public event System.Action<BaseTile> OnTileDestroyed;

    public void Launch()
    {
        if (hasLaunched) return;

        rb.AddForce(Vector3.left * launchForce, ForceMode.Impulse);
        hasLaunched = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasLaunched) return;

        BaseTile other = collision.gameObject.GetComponent<BaseTile>();
        if (other == null || other == this) return;

        if (!readyToMerge || !other.CanBeMerged()) return;
        if (value != other.Value) return;

        float impulse = collision.impulse.magnitude;
        if (impulse < requiredImpulseToMerge)
        {
            return;
        }

        MergeWith(other);
    }

    private void MergeWith(BaseTile other)
    {
        value *= 2;
        SetValue(value);
        rb.AddForce(Vector3.up * mergeJumpForce, ForceMode.Impulse);
        SoundManager.Instance.PlaySFX(SoundManager.Instance.mergeClip);

        Destroy(other.gameObject);
        ScoreManager.Instance?.AddScore(value / 2);

        readyToMerge = false;
        Invoke(nameof(EnableMerge), 0.1f);
    }

    private void EnableMerge()
    {
        readyToMerge = true;
    }

    private void OnDestroy()
    {
        OnTileDestroyed?.Invoke(this);
    }
}
