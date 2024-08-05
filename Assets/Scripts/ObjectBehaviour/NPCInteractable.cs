using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour, IInteractable
{
    public List<string> GetActions()
    {
        return new List<string> { "Talk", "Examine" };
        
    }


    public void PerformAction(string action)
    {
        Debug.Log("PerformAction called with action: " + action);
        if (action == "Pick Up")
        {
            //PickUpItem();
        }
        else if (action == "Drop")
        {
            //DropItem();
        }
        else if (action == "Examine")
        {
            //ExamineItem();
        }
        else if (action == "Use")
        {
            //UseItem();
        }
        // Handle other actions
    }
}

