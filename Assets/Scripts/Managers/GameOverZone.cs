using UnityEngine;

public class GameOverZone : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        TileController tile = other.GetComponent<TileController>();
        if (tile == null) return;

        if (tile.HasLaunched && tile.IsStopped())
        {
            Debug.Log("Game over: tile stopped in danger zone");
            GameManager.Instance.TriggerGameOver();
        }
    }
}
