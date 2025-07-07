using UnityEngine;

public class StaticTile : BaseTile
{
    protected override void Awake()
    {
        base.Awake();
        rb.isKinematic = false;
    }

    public override bool CanBeMerged()
    {
        return true;
    }
}

