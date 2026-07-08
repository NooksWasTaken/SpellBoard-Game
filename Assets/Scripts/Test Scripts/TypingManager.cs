using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class TypingManager : MonoBehaviour
{
    [Header("Player References")]
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private PlayerSpellBoard spellBoard;
    [SerializeField] private PuzzleSelector selector;
    [SerializeField] private PlayerController playerController;

    [Header("Spell Typing HUD")]
    [SerializeField] private GameObject selectPuzzleText;
    [SerializeField] private GameObject typingPanel;
    [SerializeField] private GameObject castingUI;
    [SerializeField] private Image timerFill;

    [Header("Spell Typing Variables")]
    [SerializeField] private float castTimeLimit = 5f;      // time before the player fails spell typing
    [SerializeField] private ParticleSystem timeoutEffect;  // particle effect to play on the puzzle when the player fails

    private float timer;
    private bool timerRunning;

    private void Awake()
    {
        
    }
    private void Update()
    {
        // prevent this entire section running if the player isn't in a casting state
        if (playerController.CurrentState != PlayerController.PlayerState.Casting)
        {
            StopCastingTimer();
            return;
        }

        if (timerRunning)
        {
            timer -= Time.deltaTime;
            timerFill.fillAmount = timer / castTimeLimit;


            if (timer <= 0f)
            {
                CastingFailed();
            }
        }

        // Keep the input field focused while casting.
        if (!inputField.isFocused)
        {
            EventSystem.current.SetSelectedGameObject(inputField.gameObject);
            inputField.ActivateInputField();
        }

        // Submit spell on Enter.
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            SubmitSpell();
        }
    }

    public void SubmitSpell()
    {
        string input = inputField.text.Trim();

        PlayerSpells spell = spellBoard.FindSpell(input);

        if (spell == null)
        {
            Debug.Log("Unknown spell.");

            if (selector.CurrentPuzzle != null)
            {
                PlayFailureEffect();
            }

            ClearAndFocus();
            return;
        }

        if (selector.CurrentPuzzle != null)
        {
            PuzzleResult result = selector.CurrentPuzzle.TrySolve(spell);
            SpellChainPuzzle chainPuzzle = selector.CurrentPuzzleObject as SpellChainPuzzle;

            if (chainPuzzle != null)
            {
                SpellChainHUD.Instance.UpdateProgress(
                    chainPuzzle.GetCurrentSpellIndex(),
                    chainPuzzle.GetSpellCount());
            }

            switch (result)
            {
                case PuzzleResult.Failed:

                    PlayFailureEffect();
                    break;

                case PuzzleResult.Progress:

                    SpawnSpellEffect(spell);
                    SpellHUD.Instance.Refresh();
                    break;

                case PuzzleResult.Solved:

                    SpawnSpellEffect(spell);

                    SpellChainHUD.Instance.Hide();

                    StopCastingTimer();

                    selector.DeselectCurrentPuzzle();

                    playerController.SetMoveState();

                    ClearAndFocus();

                    return;
            }
        }
        else
        {
            Debug.Log("No puzzle selected.");
        }

        ClearAndFocus();
    }

    private void CastingFailed()
    {
        StopCastingTimer();

        PlayFailureEffect();

        selector.DeselectCurrentPuzzle();

        playerController.SetMoveState();

        ClearAndFocus();
    }

    public void StartCastingTimer()
    {
        timer = selector.CurrentPuzzle.GetTimeLimit();
        castTimeLimit = timer;

        timerRunning = true;

        typingPanel.SetActive(true);
        timerFill.fillAmount = 1f;
    }

    public void StopCastingTimer()
    {
        timerRunning = false;
        timerFill.fillAmount = 1f;
    }

    private void ClearAndFocus()
    {
        inputField.text = "";

        EventSystem.current.SetSelectedGameObject(inputField.gameObject);
        inputField.ActivateInputField();
    }

    public void SubmitFocus()
    {
        inputField.text = "";

        EventSystem.current.SetSelectedGameObject(inputField.gameObject);
        inputField.ActivateInputField();
    }

    public void ShowWaitingUI()
    {
        selectPuzzleText.SetActive(true);
        typingPanel.SetActive(false);
    }

    public void ShowTypingUI()
    {
        selectPuzzleText.SetActive(false);
        typingPanel.SetActive(true);
    }

    private void SpawnSpellEffect(PlayerSpells spell)
    {
        Debug.Log($"SpawnSpellEffect: {spell.name}");

        if (spell.castEffect == null)
        {
            Debug.LogWarning($"{spell.name} has no cast effect assigned.");
            return;
        }

        Transform target = selector.CurrentPuzzleObject.transform;

        ParticleSystem effect = Instantiate(spell.castEffect, target.position, Quaternion.identity);

        Destroy(effect.gameObject,
            effect.main.duration + effect.main.startLifetime.constantMax);
    }

    private void PlayFailureEffect()
    {
        if (timeoutEffect == null || selector.CurrentPuzzleObject == null)
            return;

        ParticleSystem effect = Instantiate(timeoutEffect, selector.CurrentPuzzleObject.transform.position, Quaternion.identity);

        Destroy(effect.gameObject, effect.main.duration + effect.main.startLifetime.constantMax);
    }
}