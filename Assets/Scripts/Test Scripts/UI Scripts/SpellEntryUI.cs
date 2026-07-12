using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellEntryUI : MonoBehaviour
{
    [SerializeField] private Image spellIcon;
    [SerializeField] private TMP_Text description;

    public void Initialize(PlayerSpells spell)
    {
        spellIcon.sprite = spell.spellIcon;
        description.text = spell.description;
    }
}