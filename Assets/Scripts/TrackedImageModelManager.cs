using UnityEngine;
using UnityEngine.InputSystem;

public enum AnimationState
{
    Idle,
    Walk ,
    Unknown
}

public class TrackedImageModelManager : MonoBehaviour
{
    private GameObject trackedImageModel;
    private Animator modelAnimator;

    private string IS_WALKING_FLAG_NAME = "IsWalking";
    
    private float DEBOUNCE_DURATION = 0.2f; // Time in seconds between toggles
    
    private float lastIsWalkingToggleTime = 0f;
    
    public bool isModelVisible { get; private set; } = false;
    
    public AnimationState animationState { get; private set; } = AnimationState.Unknown;
    
    void Update()
    {
        trackedImageModel = GameObject.FindGameObjectWithTag("TrackedImageModel");
        if (trackedImageModel != null)
        {
            modelAnimator = ComponentFinder.FindComponentInChildrenRecursive<Animator>(trackedImageModel);

            UpdateIsModelVisible();
        }
        
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            HandleTouchWithDebounce();
        }
    }

    private void HandleTouchWithDebounce()
    {
        if (Time.time - lastIsWalkingToggleTime >= DEBOUNCE_DURATION)
        {
            var isToggled = HandleTouch();
            if (isToggled)
            {
                 lastIsWalkingToggleTime = Time.time;
            }
        }
    }
    
    private bool HandleTouch()
    {
        var touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        var ray = Camera.main.ScreenPointToRay(touchPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject == trackedImageModel && modelAnimator != null)
            {
                var isWalking = modelAnimator.GetBool(IS_WALKING_FLAG_NAME);
                var isWalkingNew = !isWalking;
                animationState = isWalkingNew ? AnimationState.Walk : AnimationState.Idle;
                
                // todo: вырезать
                var str = animationState == AnimationState.Walk ? "Walk" : "Idle";
                MyLogger.Log($"Animation state: {str}");
                
                modelAnimator.SetBool(IS_WALKING_FLAG_NAME, isWalkingNew);
                return true;
            }
        }
        return false;
    }

    private void UpdateIsModelVisible()
    {
        if (trackedImageModel != null && Camera.main != null)
        {
            var oldIsModelVisible = isModelVisible;
            
            Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
            Bounds modelBounds = ComponentFinder.FindComponentInChildrenRecursive<Renderer>(trackedImageModel).bounds;

            isModelVisible = GeometryUtility.TestPlanesAABB(frustumPlanes, modelBounds);

            if (oldIsModelVisible != isModelVisible)
            {
                var isModelVisibleStr = isModelVisible ? "true" : "false";
                MyLogger.Log($"isModelVisible: {isModelVisibleStr}");
            }
        }
    }
    
    private float ANGLE_STEP = 45f;
    
    public void RotateLeft()
    {
        if (trackedImageModel != null)
        {
            // trackedImageModel.transform.GetChild(0).Rotate(Vector3.up, ANGLE_STEP);
            Quaternion rotation = Quaternion.Euler(0, ANGLE_STEP, 0); 
            trackedImageModel.transform.GetChild(0).rotation = trackedImageModel.transform.GetChild(0).rotation * rotation;
        }
    }

    public void RotateRight()
    {
        if (trackedImageModel != null)
        {
            // trackedImageModel.transform.GetChild(0).Rotate(Vector3.up, -ANGLE_STEP);
            Quaternion rotation = Quaternion.Euler(0, -ANGLE_STEP, 0);
            trackedImageModel.transform.GetChild(0).rotation = trackedImageModel.transform.GetChild(0).rotation * rotation;
        }
    }

    public void ScaleUp()
    {
        if (trackedImageModel != null)
        {
            // trackedImageModel.transform.GetChild(0).localScale *= 1.1f;
            trackedImageModel.transform.localScale *= 1.1f; // for collider
        }
    }

    public void ScaleDown()
    {
        if (trackedImageModel != null)
        {
            // trackedImageModel.transform.GetChild(0).localScale *= 0.9f;
            trackedImageModel.transform.localScale *= 0.9f; // for collider
        }
    }
}
