using System;
using UnityEngine;

public class PuzzleSelector : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private LayerMask puzzleLayer;
    [SerializeField] private TypingManager typingManager;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float PuzzleSelectRange = 10f;

    private PuzzleVisual hoveredVisual;
    public MonoBehaviour CurrentPuzzleObject { get; private set; }

    public IPuzzle CurrentPuzzle { get; private set; }

    private void Update()
    {
        if (playerController.CurrentState != PlayerController.PlayerState.Casting)
        {
            if (hoveredVisual != null)
            {
                hoveredVisual.HoverExit();
                hoveredVisual = null;
            }

            return;
        }

        CheckHoveredPuzzle();
        if (Input.GetMouseButtonDown(0))
        {
            SelectPuzzle();
        }

        if (Input.GetMouseButtonDown(1))
        {
            DeselectCurrentPuzzle();
        }
    }

    private void SelectPuzzle()
    {
        if (CurrentPuzzle != null)
            CurrentPuzzle.Deselect();

        CurrentPuzzle = null;
        CurrentPuzzleObject = null;
        typingManager.SubmitFocus();

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, PuzzleSelectRange, puzzleLayer))
        {
            CurrentPuzzle = hit.collider.GetComponent<IPuzzle>();

            if (CurrentPuzzle != null)
            {
                CurrentPuzzleObject = hit.collider.GetComponent<MonoBehaviour>();

                CurrentPuzzle.Select();
                typingManager.ShowTypingUI();
                SpellHUD.Instance.Refresh();

                if (playerController.CurrentState == PlayerController.PlayerState.Casting)
                {
                    typingManager.ShowTypingUI();
                    typingManager.StartCastingTimer();
                }
            }
        }

        SpellChainPuzzle chainPuzzle = CurrentPuzzleObject as SpellChainPuzzle;

        if (chainPuzzle != null)
        {
            SpellChainHUD.Instance.UpdateProgress(
                chainPuzzle.GetCurrentSpellIndex(),
                chainPuzzle.GetSpellCount());
        }
        else
        {
            SpellChainHUD.Instance.Hide();
        }
    }

    public void DeselectCurrentPuzzle()
    {
        if (CurrentPuzzle != null)
        {
            typingManager.StopCastingTimer();
            typingManager.ShowWaitingUI();
            SpellChainHUD.Instance.Hide();

            CurrentPuzzle.Deselect();

            CurrentPuzzle = null;
            CurrentPuzzleObject = null;

            SpellHUD.Instance.Refresh();
        }
    }

    private void CheckHoveredPuzzle()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, PuzzleSelectRange, puzzleLayer))
        {
            IPuzzle puzzle = hit.collider.GetComponent<IPuzzle>();

            if (puzzle != null)
            {
                PuzzleVisual visual = hit.collider.GetComponent<PuzzleVisual>();

                if (hoveredVisual != visual)
                {
                    hoveredVisual?.HoverExit();

                    hoveredVisual = visual;
                    hoveredVisual?.HoverEnter();

                    Debug.Log("Hovering: " + hit.collider.name);
                }

                return;
            }
        }

        hoveredVisual?.HoverExit();
        hoveredVisual = null;
    }
}