using UnityEngine;

[CreateAssetMenu(menuName = "Interaction/Note")]
public class NoteData : ScriptableObject
{
    [Header("Prompt")]
    public string HoverText = "[E] ";

    [Header("Content")]
    [TextArea(8, 20)]
    public string ContentText;
}