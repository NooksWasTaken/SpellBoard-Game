using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Puzzle : MonoBehaviour
{
    [Header("Puzzle Settings")]
    public PuzzleType puzzleType;           // will help in distinguishing what spell is needed
    public bool CollideWithPlayer = true;   // configure via inspector, tick if you want the puzzle to have collision
    public bool IsObjectHidden = false;     // configure via inspector, tick if the purpose of the object requires to be hidden at first
    public float fadeDuration = 2f;         // duration of the fade effect

    [Header("Puzzle Events to Run")]
    public UnityEvent onPuzzleSolved;

    private void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        Material mat = renderer.material;
        Color color = mat.color;

        // set puzzle settings based on inspector settings (Collision and Visibility)
        gameObject.layer = CollideWithPlayer ? 7 : 8;
        color.a = IsObjectHidden ? 0f : 1f;

        mat.color = color;
    }

    public void OnPuzzleSolved()
    {
        onPuzzleSolved.Invoke(); // triggers event(s) assigned in the inspector
        Debug.Log("Puzzle Function running");
    }

    
    public void FadeIn()
    {
        StartCoroutine(FadeInCoroutine());
    }

    // fade in the object by gradually increasing its transparency
    private IEnumerator FadeInCoroutine()
    {
        // reference the renderer component of the object
        Renderer renderer = GetComponent<Renderer>();

        // make sure material supports transparency
        Material mat = renderer.material;
        Color color = mat.color;
        color.a = 0f;
        mat.color = color;

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsed / fadeDuration);
            mat.color = color;
            yield return null;
        }

        // make sure alpha is fully opaque at the end
        color.a = 1f;
        mat.color = color;

        yield return new WaitForSeconds(0.5f);

        // set the object to collide with player after being fully visible
        gameObject.layer = 7;
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutCoroutine());
    }

    // make the object fade out gradually 
    private IEnumerator FadeOutCoroutine()
    {
        // reference the renderer component of the object
        Renderer renderer = GetComponent<Renderer>();

        Material mat = renderer.material;
        Color color = mat.color;

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            // increase time elapse per frame
            elapsed += Time.deltaTime;

            // fade math
            // subtract current alpha with quotient of elapsed and duration
            // clamping it makes it not do wierd stuff
            color.a = 1f - Mathf.Clamp01(elapsed / fadeDuration);
            mat.color = color;
            yield return null;
        }

        color.a = 0f;
        mat.color = color;

        yield return new WaitForSeconds(0.5f);

        // fully disable the object after opacity is at 0
        this.gameObject.SetActive(false);
    }


}