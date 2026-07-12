using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string sceneName;

    public void LoadScene()
    {
        StartCoroutine(LoadSceneRoutine());
    }

    private IEnumerator LoadSceneRoutine()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (SlideInTransition.Instance != null)
        {
            SlideInTransition.Instance.PlayTransition();

            while (SlideInTransition.Instance.IsPlaying)
                yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }
}