using UnityEngine;

public class UIElementsManager : MonoBehaviour
{
    [SerializeField]
    public GameObject[] uiElements;
    
    public void OnModelVisibilityChanged(bool newVal)
    {
        var isModelVisibleStr = newVal ? "true" : "false";
        MyLogger.Log($"isModelVisible: {isModelVisibleStr}");
        
        foreach (var element in uiElements)
        {
            if (element != null)
            {
                element.SetActive(newVal);
            }
        }
    }
    
    public void OnAnimationStateChanged(AnimationState newVal)
    {
        var str = newVal == AnimationState.Walk ? "Walk" : "Idle";
        MyLogger.Log($"Animation state: {str}");
    }
}