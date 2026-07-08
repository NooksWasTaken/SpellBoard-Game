using UnityEngine;

[RequireComponent(typeof(Outline))]
public class PuzzleVisual : MonoBehaviour
{
    private Outline outline;

    private bool isHovered;
    private bool isSelected;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }

    public void HoverEnter()
    {
        isHovered = true;
        UpdateOutline();
    }

    public void HoverExit()
    {
        isHovered = false;
        UpdateOutline();
    }

    public void Select()
    {
        isSelected = true;
        UpdateOutline();
    }

    public void Deselect()
    {
        isSelected = false;
        UpdateOutline();
    }

    private void UpdateOutline()
    {
        outline.enabled = isHovered || isSelected;
    }
}