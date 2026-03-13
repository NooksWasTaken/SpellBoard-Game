using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpellTypingSystem : MonoBehaviour
{
    public TMP_InputField spellInput;   // references the input field where the player can type spells
    public List<Spell> spells;          // list of spells the player can cast

    public SpellEffectsManager spellEffectsManager; // reference to yes

    void Awake()
    {
        // find the manager in the scene automatically if not assigned
        if (spellEffectsManager == null)
        {
            spellEffectsManager = FindFirstObjectByType<SpellEffectsManager>();
            if (spellEffectsManager == null)
            {
                Debug.LogError("SpellEffectsManager.cs not found");
            }
        }
    }

    void OnEnable()
    {
        // When the player presses ENTER in the input field, unity calls the CheckSpell function
        spellInput.onSubmit.AddListener(CheckSpell);

        // automatically focus the input field so the player can start typing immediately
        spellInput.ActivateInputField();
    }

    void OnDisable()
    {
        // this function will run when the UI is disabled
        ClearInput();
    }

    // runs immediately when the player presses ENTER on the input field
    void CheckSpell(string typedSpell)
    {
        // auto converts typed words to lowercase and remove extra spaces (ignores upper case entirely)
        typedSpell = typedSpell.ToLower().Trim();

        // variable if the player typed a valid spell
        bool spellFound = false;

        // loop through every available spell in the list
        foreach (Spell spell in spells)
        {
            // compare the typed word with the spell incantation
            if (typedSpell == spell.incantation.ToLower())
            {
                // cast if a match is found
                CastSpell(spell);
                spellFound = true;
                break;
            }
        }

        // for debugging, if spell is not found, then obviously its invalid
        if (!spellFound)
        {
            Debug.Log("Invalid Spell");
        }

        // clear the field when spell is cast
        ClearInput();
    }

    // this function runs when a valid spell is cast
    void CastSpell(Spell spell)
    {
        // log for debugging
        Debug.Log("Spell Cast: " + spell.spellName);

        // call the scene manager to perform the spell effect based on puzzle type
        if (spellEffectsManager != null)
        {
            spellEffectsManager.CastSpellEffect(spell.puzzleType);
        }

        // optional: call the event attached to the spell (VFX, UI, etc.)
        spell.onSpellCast.Invoke();
    }

    // clears the field so player can type another spell
    void ClearInput()
    {
        // remove text from the field
        spellInput.text = "";

        // reactivate the input field so typing continues immediately
        spellInput.ActivateInputField();
    }
}