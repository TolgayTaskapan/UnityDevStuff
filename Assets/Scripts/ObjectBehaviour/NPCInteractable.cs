using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour, IInteractable
{
    public List<string> GetActions()
    {
        return new List<string> { "Talk", "Examine" };
    }
}
