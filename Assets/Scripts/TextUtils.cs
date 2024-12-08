using UnityEngine;
using TMPro;

public class TextUtility : MonoBehaviour
{
    public static void DebugLog(string newLineText)
    {
        GameObject textObject = GameObject.FindWithTag("DebugConsole");
        
        if (textObject != null && textObject.TryGetComponent<TMP_Text>(out TMP_Text tmpText))
        {
            tmpText.text += "\n" + newLineText;
        }
        else
        {
            Debug.LogWarning($"No TextMeshPro text object found with tag: DebugConsole");
        }
    }
}