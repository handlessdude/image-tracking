using UnityEngine;
using UnityEngine.InputSystem;

public class TrackedImageModelManager : MonoBehaviour
{
    private GameObject trackedImageModel;
    private Animator modelAnimator;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        trackedImageModel = GameObject.FindGameObjectWithTag("TrackedImageModel");
        if (trackedImageModel != null)
        {
            modelAnimator = trackedImageModel.GetComponent<Animator>();
        }
        
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            var touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            HandleTouch(touchPosition);
        }
    }

    private void HandleTouch(Vector2 touchPosition)
    {
        Ray ray = mainCamera.ScreenPointToRay(touchPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject == trackedImageModel && modelAnimator != null)
            {
                modelAnimator.SetTrigger("Walk");
            }
        }
    }

    private float ANGLE_STEP = 45f;
    
    public void RotateLeft()
    {
        if (trackedImageModel != null)
        {
            // trackedImageModel.transform.GetChild(0).Rotate(Vector3.up, -ANGLE_STEP);
            Quaternion rotation = Quaternion.Euler(0, -ANGLE_STEP, 0); 
            trackedImageModel.transform.GetChild(0).rotation = trackedImageModel.transform.GetChild(0).rotation * rotation;
        }
    }

    public void RotateRight()
    {
        if (trackedImageModel != null)
        {
            // trackedImageModel.transform.GetChild(0).Rotate(Vector3.up, ANGLE_STEP);
            Quaternion rotation = Quaternion.Euler(0, ANGLE_STEP, 0);
            trackedImageModel.transform.GetChild(0).rotation = trackedImageModel.transform.GetChild(0).rotation * rotation;
        }
    }

    public void ScaleUp()
    {
        if (trackedImageModel != null)
        {
            trackedImageModel.transform.GetChild(0).localScale *= 1.1f;
        }
    }

    public void ScaleDown()
    {
        if (trackedImageModel != null)
        {
            trackedImageModel.transform.GetChild(0).localScale *= 0.9f;
        }
    }
}
