using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Scattered Spell Scrolls")]
    [Header("Restore")]
    public int restoreTornScroll = 0;
    public Spell assembledSpell;

    [Space]

    [Header("Spell Typing System Script Reference")]
    public SpellTypingSystem typingSystem;

    private void Update()
    {
        if (restoreTornScroll == 3)
        {
            RestoreScrollCompleted();
        }
    }

    public void RestoreScrollCompleted()
    {
        if (!typingSystem.spells.Exists(s => s != null && s.spellName == assembledSpell.spellName))
        {
            typingSystem.spells.Add(assembledSpell);
            Debug.Log("Spell added: " + assembledSpell.spellName);
            Debug.Log("Spell count: " + typingSystem.spells.Count);
        }
    }
}
