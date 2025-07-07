using UnityEngine;

public class DragInput : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float minZ = -4f, maxZ = 4f;
    [SerializeField] private TileSpawner tileSpawner;

    private Camera mainCamera;
    private Rigidbody rb;
    private TileController tileController;

    private bool isDragging = false;
    private bool isReadyToLaunch = false;
    private Vector3? targetPosition = null;

    private void Awake()
    {
        mainCamera = Camera.main;
        tileSpawner = FindFirstObjectByType<TileSpawner>();
        tileSpawner.OnTileSpawned += AssignNewTile;
    }

    private void Start()
    {
        tileSpawner.RequestSpawn();
    }

    private void Update()
    {
        if (!isReadyToLaunch || tileController == null) return;

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

    private void FixedUpdate()
    {
        if (!isReadyToLaunch || !targetPosition.HasValue || rb == null) return;

        rb.MovePosition(Vector3.Lerp(rb.position, targetPosition.Value, Time.fixedDeltaTime * moveSpeed));
    }

    private void MoveWithMouse()
    {
        Vector3 worldPos = GetWorldMousePosition();
        SetTargetPosition(worldPos);
    }

    private void MoveWithTouch(Touch touch)
    {
        Vector3 screenPos = touch.position;
        screenPos.z = mainCamera.WorldToScreenPoint(tileController.transform.position).z;
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(screenPos);
        SetTargetPosition(worldPos);
    }

    private void SetTargetPosition(Vector3 worldPos)
    {
        float clampedZ = Mathf.Clamp(worldPos.z, minZ, maxZ);
        targetPosition = new Vector3(rb.position.x, rb.position.y, clampedZ);
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
        targetPosition = null;

        if (tileController != null)
        {
            tileController.Launch();
            tileController = null;
            rb = null;
            isReadyToLaunch = false;
        }

        tileSpawner.RequestSpawn();
    }

    private void AssignNewTile(GameObject newTile)
    {
        tileController = newTile.GetComponent<TileController>();
        rb = newTile.GetComponent<Rigidbody>();

        if (tileController == null || rb == null)
        {
            Debug.LogError("TileController or Rigidbody missing!");
            return;
        }
        isReadyToLaunch = true;
    }
}
