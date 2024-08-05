using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    List<string> GetActions();
    void PerformAction(string action); // Define PerformAction in the interface
}
