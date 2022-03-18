using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CompleteData
{
    public bool[] isComplete;
    public CompleteData(ControlScenes complete)
    {
        isComplete = complete.isComplete;
    }
}