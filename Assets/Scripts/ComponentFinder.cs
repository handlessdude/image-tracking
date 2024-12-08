using UnityEngine;

public static class ComponentFinder
{
    public static T FindComponentInChildrenRecursive<T>(GameObject obj) where T : Component
    {
        // Check if the component exists on the current object
        T component = obj.GetComponent<T>();
        if (component != null)
        {
            return component;
        }

        // Recursively search through all children
        foreach (Transform child in obj.transform)
        {
            component = FindComponentInChildrenRecursive<T>(child.gameObject);
            if (component != null)
            {
                return component;
            }
        }

        // Return null if not found
        return null;
    }
}