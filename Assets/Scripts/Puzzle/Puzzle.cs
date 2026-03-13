using UnityEngine;
using UnityEngine.Events;

public class Puzzle : MonoBehaviour
{
    public PuzzleType puzzleType;       // will help in distinguishing what spell is needed
    public UnityEvent onPuzzleSolved;

    public void OnPuzzleSolved()
    {
        onPuzzleSolved.Invoke(); // triggers event(s) assigned in the inspector
        Debug.Log("Puzzle Function running");
    }
}