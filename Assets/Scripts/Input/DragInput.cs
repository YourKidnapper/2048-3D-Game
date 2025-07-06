using UnityEngine;

public class DragInput : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float moveSpeed = 0.1f;

    private bool isDragging = false;
    private bool hasLaunched = false;

    private Camera mainCamera;
    [SerializeField] private TileController tileController;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        if (tileController == null) tileController = GetComponent<TileController>();
    }

    void Update()
    {
        if (hasLaunched) return;

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
            isDragging = true;

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            LaunchForward();
        }

        if (isDragging)
            MoveWithMouse();
#else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
                isDragging = true;

            else if (touch.phase == TouchPhase.Ended)
            {
                isDragging = false;
                LaunchForward();
            }

            if (isDragging)
                MoveWithTouch(touch);
        }
#endif
    }

    void MoveWithMouse()
    {
        Vector3 screenPos = Input.mousePosition;
        screenPos.z = mainCamera.WorldToScreenPoint(transform.position).z;

        Vector3 worldPos = mainCamera.ScreenToWorldPoint(screenPos);
        Vector3 targetPos = new Vector3(transform.position.x, transform.position.y, worldPos.z); // Only Z axis changes (left/right)

        rb.MovePosition(Vector3.Lerp(transform.position, targetPos, moveSpeed));
    }

    void MoveWithTouch(Touch touch)
    {
        Vector3 screenPos = touch.position;
        screenPos.z = mainCamera.WorldToScreenPoint(transform.position).z;

        Vector3 worldPos = mainCamera.ScreenToWorldPoint(screenPos);
        Vector3 targetPos = new Vector3(worldPos.x, transform.position.y, transform.position.z);
        rb.MovePosition(Vector3.Lerp(transform.position, targetPos, moveSpeed));
    }

    void LaunchForward()
    {
        if (hasLaunched) return;

        tileController.Launch();
        hasLaunched = true;
    }
}
