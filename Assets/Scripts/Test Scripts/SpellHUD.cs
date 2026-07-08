using UnityEngine;
using UnityEngine.UI;

public class SpellHUD : MonoBehaviour
{
    public static SpellHUD Instance { get; private set; }

    [SerializeField] private PuzzleSelector selector;
    [SerializeField] private Image spellIcon;

    [Header("Default Icon")]
    [SerializeField] private Sprite defaultIcon;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Refresh()
    {
        if (selector.CurrentPuzzle == null)
        {
            spellIcon.sprite = defaultIcon;
            return;
        }

        PlayerSpells spell = selector.CurrentPuzzle.GetCurrentRequiredSpell();

        if (spell == null || spell.spellIcon == null)
        {
            spellIcon.sprite = defaultIcon;
            return;
        }

        spellIcon.sprite = spell.spellIcon;
    }
}