using UnityEngine;

public abstract class BaseTile : MonoBehaviour
{
    protected Rigidbody rb;
    protected TileView tileView;
    protected int value;

    public int Value => value;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        tileView = GetComponent<TileView>();
    }

    public virtual void SetValue(int newValue)
    {
        value = newValue;
        tileView.ApplyVisual(newValue);
    }

    public virtual bool CanBeMerged()
    {
        return true;
    }

    public int GetValue()
    {
        return value;
    }
}
