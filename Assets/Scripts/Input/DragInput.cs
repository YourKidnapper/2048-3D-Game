using UnityEngine;

public class DragInput : MonoBehaviour
{
     [SerializeField] private float moveSpeed = 0.1f;
    [SerializeField] private float minZ = -4f, maxZ = 4f;
    [SerializeField] private TileSpawner tileSpawner;

    private Camera mainCamera;
    private Rigidbody rb;
    private TileController tileController;

    private bool isDragging = false;

    private void Awake()
    {
        mainCamera = Camera.main;
        tileSpawner = FindFirstObjectByType<TileSpawner>();
        SpawnAndAssignNewTile();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0)) isDragging = true;
        if (Input.GetMouseButtonUp(0)) LaunchForward();
        if (isDragging) MoveWithMouse();
#else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began) isDragging = true;
            else if (touch.phase == TouchPhase.Ended) LaunchForward();
            if (isDragging) MoveWithTouch(touch);
        }
#endif
    }

    private void MoveWithMouse()
    {
        Vector3 worldPos = GetWorldMousePosition();
        MoveTile(worldPos);
    }

    private void MoveWithTouch(Touch touch)
    {
        Vector3 screenPos = touch.position;
        screenPos.z = mainCamera.WorldToScreenPoint(tileController.transform.position).z;
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(screenPos);
        MoveTile(worldPos);
    }

    private void MoveTile(Vector3 worldPos)
    {
        float clampedZ = Mathf.Clamp(worldPos.z, minZ, maxZ);
        Vector3 targetPos = new Vector3(tileController.transform.position.x, tileController.transform.position.y, clampedZ);
        rb.MovePosition(Vector3.Lerp(tileController.transform.position, targetPos, moveSpeed));
    }

    private Vector3 GetWorldMousePosition()
    {
        Vector3 screenPos = Input.mousePosition;
        screenPos.z = mainCamera.WorldToScreenPoint(tileController.transform.position).z;
        return mainCamera.ScreenToWorldPoint(screenPos);
    }

    private void LaunchForward()
    {
        isDragging = false;
        tileController.Launch();
        SpawnAndAssignNewTile();
    }

    private void SpawnAndAssignNewTile()
    {
        if (!tileSpawner.CanSpawn) return;

        GameObject newTile = tileSpawner.SpawnNewTile();

        if (newTile == null) return;

        rb = newTile.GetComponent<Rigidbody>();
        tileController = newTile.GetComponent<TileController>();
    }
}
