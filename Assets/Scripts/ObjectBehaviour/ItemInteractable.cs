using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteractable : MonoBehaviour, IInteractable
{
    public List<string> GetActions()
    {
        return new List<string> { "Pick Up", "Examine", "Use" };
    }
}
