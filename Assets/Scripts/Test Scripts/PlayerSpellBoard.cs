using System.Collections.Generic;
using UnityEngine;

public class PlayerSpellBoard : MonoBehaviour
{
    [SerializeField]
    private List<PlayerSpells> unlockedSpells = new();

    // checks if the player currently has a specific spell
    public bool HasSpell(PlayerSpells spell)
    {
        return unlockedSpells.Contains(spell);
    }

    // locates specific spell in the list, converts any upper cases to lowwer cases
    public PlayerSpells FindSpell(string keyword)
    {
        foreach (PlayerSpells spell in unlockedSpells)
        {
            if (spell.keyword.ToLower() == keyword.ToLower())
                return spell;
        }

        return null;
    }

    /*
    public void UnlockSpell(PlayerSpells spell)
    {
        if (!unlockedSpells.Contains(spell))
            unlockedSpells.Add(spell);
    }
    */

    // function for adding a spell to the list
    public bool AddSpell(PlayerSpells spell)
    {
        if (spell == null)
            return false;

        if (unlockedSpells.Contains(spell))
            return false;

        unlockedSpells.Add(spell);

        if (SpellJournal.Instance != null)
        {
            SpellJournal.Instance.Refresh();
        }

        Debug.Log($"Learned spell: {spell.spellName}");

        return true;
    }

    public IReadOnlyList<PlayerSpells> GetSpells()
    {
        return unlockedSpells;
    }
}