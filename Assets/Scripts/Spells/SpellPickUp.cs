using UnityEngine;

public class SpellPickup : MonoBehaviour, IInteractable
{
    public Spell spellToAdd;

    // manually assign this in the inspector
    public SpellTypingSystem typingSystem;

    public void Interact()
    {
        // safety net: return if either typing system or spell is missing
        if (typingSystem == null)
        {
            Debug.LogError("SpellTypingSystem reference not assigned on SpellPickup!");
            return;
        }

        if (spellToAdd == null)
        {
            Debug.LogError("SpellPickup has no Spell assigned!");
            return;
        }

        // ensure the list exists
        if (typingSystem.spells == null)
            typingSystem.spells = new System.Collections.Generic.List<Spell>();

        // check by unique property (spellName) to avoid reference issues
        if (!typingSystem.spells.Exists(s => s != null && s.spellName == spellToAdd.spellName))
        {
            typingSystem.spells.Add(spellToAdd);
            Debug.Log("Spell added: " + spellToAdd.spellName);
            Debug.Log("Spell count: " + typingSystem.spells.Count);
        }
        else
        {
            Debug.Log("Spell already exists in the list.");
        }

        // optional: destroy the pickup after collecting
        // Destroy(gameObject);
    }
}