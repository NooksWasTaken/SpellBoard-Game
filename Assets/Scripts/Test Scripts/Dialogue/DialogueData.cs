using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Dialogue")]
public class DialogueData : ScriptableObject
{
    public List<DialogueLine> lines = new();
}

// customized element for the list
[Serializable]
public class DialogueLine
{
    [TextArea(2, 5)]
    public string text;

    [Header("Characters per second")]
    public float typingSpeed = 30f;     // value for how fast each character reveals itself

    [Header("Text Duration")]
    public float displayTime = 2f;      // how long each string will displayed
}