using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentPrefabController : MonoBehaviour
{
    public List<GameObject> childPrefabs; // List to hold references to child prefabs

    private List<GameObject> instantiatedChildren = new List<GameObject>();

    // Example method to instantiate all child prefabs
    public void InstantiateChildren()
    {
        foreach (GameObject childPrefab in childPrefabs)
        {
            if (childPrefab != null)
            {
                GameObject instantiatedChild = Instantiate(childPrefab, transform);
                instantiatedChildren.Add(instantiatedChild);
            }
        }
    }

    // Method to add a new child prefab reference in the Inspector
    public void AddChild(GameObject newChild)
    {
        if (newChild != null && !childPrefabs.Contains(newChild))
        {
            childPrefabs.Add(newChild);
        }
    }

    // Method to remove a child prefab reference by index
    public void RemoveChildAt(int index)
    {
        if (index >= 0 && index < childPrefabs.Count)
        {
            childPrefabs.RemoveAt(index);
        }
    }

    // Example method to destroy instantiated children
    public void DestroyInstantiatedChildren()
    {
        foreach (GameObject child in instantiatedChildren)
        {
            if (child != null)
            {
                Destroy(child);
            }
        }
        instantiatedChildren.Clear();
    }
}
