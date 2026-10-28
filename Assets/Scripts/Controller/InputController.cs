using Interfaces;
using UnityEngine;

namespace Controller
{
    public class InputController : MonoBehaviour
    {
        public new Camera camera;

        private void Awake()
        {
            camera = Camera.main;
        }

        private void Update()
        {
            if (Application.platform == RuntimePlatform.WindowsEditor) 
            {
                if (!Input.GetMouseButtonDown(0)) return;
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                if (!Physics.Raycast(ray, out RaycastHit hitData)) return;
                HandleClickable(hitData);
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                if (Input.touchCount != 1) return;
                Touch touch = Input.GetTouch(0);
                if (touch.phase != TouchPhase.Began) return;
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                if (!Physics.Raycast(ray, out RaycastHit hitData)) return;
                HandleClickable(hitData);
            }
        }

        private void HandleClickable(RaycastHit hitData)
        {
            if(hitData.transform.TryGetComponent<IClickable> (out IClickable clickable)) 
            {
                clickable.OnClick();
            }
        }
    }
}
