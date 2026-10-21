using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    [Header("Camera Map Bounds")]
    [SerializeField] private float mapBoundsMinX = -7f;
    [SerializeField] private float mapBoundsMaxX = 3f;
    [SerializeField] private float mapBoundsMinZ = -7f;
    [SerializeField] private float mapBoundsMaxZ = 3f;

    [Header("Camera Zoom")]
    [SerializeField, Range(1f, 10f)] private float minZoom = 1f;
    [SerializeField, Range(1f, 10f)] private float maxZoom = 5f;
    [SerializeField, Range(1f, 50f)] private float smoothSpeed = 20f;
    [SerializeField, Range(1f, 10f)] private float zoomStrength = 5f;
    [SerializeField, Range(0.1f, 0.5f)] private float camSnapBackDirection = 0.1f;

    private Camera cam;
    private Vector3 touchStart;
    private float targetZoom;
    private float zoomVelocity;
    private Vector3 camOffset;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        targetZoom = cam.orthographicSize;
        camOffset = cam.transform.position;
    }

    private void Update()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                HandleMobileInput();
                break;
            case RuntimePlatform.WindowsEditor:
                HandleDevInput();
                break;
            default:
                HandleDevInput();
                break;
        }
    }

    private void HandleMobileInput()
    {
        switch (Input.touchCount)
        {
            case <= 0:
                return;
            case 1:
                Touch touch = Input.GetTouch(0);
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        HandlePanStart(touch.position);
                        break;
                    case TouchPhase.Moved:
                        HandlePanMove(touch.position);
                        break;
                    case TouchPhase.Ended:
                        HandlePanEnd();
                        break;
                }
                break;
            case 2:
                Touch touchOne = Input.GetTouch(0);
                Touch touchTwo = Input.GetTouch(1);
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
                Vector2 touchTwoPrevPos = touchTwo.position - touchTwo.deltaPosition;

                float prevMagnitude = (touchOnePrevPos - touchTwoPrevPos).magnitude;
                float touchMagnitude = (touchOne.position - touchTwo.position).magnitude;
                float difference = (prevMagnitude - touchMagnitude) / 100f;

                HandleZoom(difference);

                break;
        }
    }

    private void HandleDevInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandlePanStart(Input.mousePosition);
        }
        if (Input.GetMouseButton(0))
        {
            HandlePanMove(Input.mousePosition);
        }
        if (Input.GetMouseButtonUp(0))
        {
            HandlePanEnd();
        }

        HandleZoom(Input.GetAxis("Mouse ScrollWheel"));
    }

    private void HandlePanStart(Vector2 pos)
    {
        touchStart = cam.ScreenToWorldPoint(pos);
    }

    private void HandlePanMove(Vector2 delta)
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        Vector3 direction = touchStart - cam.ScreenToWorldPoint(delta);
        direction.y = 0;
        transform.position += direction;
    }

    private void HandlePanEnd()
    {
        Vector3 camPos = cam.transform.position;
        camPos.x = Mathf.Clamp(camPos.x, mapBoundsMinX, mapBoundsMaxX);
        camPos.z = Mathf.Clamp(camPos.z, mapBoundsMinZ, mapBoundsMaxZ);
        camPos.y = camOffset.y;
        transform.DOMove(camPos, camSnapBackDirection).SetEase(Ease.OutQuart);
    }

    private void HandleZoom(float zoomInput)
    {
        if (zoomInput != 0)
        {
            targetZoom -= zoomInput * zoomStrength;
            targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
        }

        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, targetZoom, ref zoomVelocity, smoothSpeed * Time.deltaTime);
    }
}