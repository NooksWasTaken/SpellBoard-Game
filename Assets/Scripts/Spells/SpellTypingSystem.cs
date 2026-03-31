using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpellTypingSystem : MonoBehaviour
{
    public TMP_InputField spellInput;   // references the input field where the player can type spells
    public List<Spell> spells = new List<Spell>(); // list of spells the player can cast (always initialized)

    public SpellEffectsManager spellEffectsManager; // reference to yes
    private int previousSpellCount = 0;

    void Awake()
    {
        // find the manager in the scene automatically if not assigned
        if (spellEffectsManager == null)
        {
            spellEffectsManager = GetComponent<SpellEffectsManager>();

            if (spellEffectsManager == null)
                spellEffectsManager = GetComponentInChildren<SpellEffectsManager>();

            if (spellEffectsManager == null)
            {
                Debug.LogError("SpellEffectsManager.cs not found on player or children!");
            }
        }

        if (spells == null)
            spells = new List<Spell>();

        previousSpellCount = spells.Count; // initialize previous count
    }

    void OnEnable()
    {
        // When the player presses ENTER in the input field, unity calls the CheckSpell function
        if (spellInput != null)
            spellInput.onSubmit.AddListener(CheckSpell);

        // automatically focus the input field so the player can start typing immediately
        if (spellInput != null)
            spellInput.ActivateInputField();
    }

    void OnDisable()
    {
        // this function will run when the UI is disabled
        ClearInput();
    }

    private void Update()
    {
        if (spells.Count > previousSpellCount)
        {
            Debug.Log($"Spell list increased! New count: {spells.Count}");
            previousSpellCount = spells.Count;
        }
    }

    // runs immediately when the player presses ENTER on the input field
    void CheckSpell(string typedSpell)
    {
        // auto converts typed words to lowercase and remove extra spaces
        typedSpell = typedSpell?.ToLower().Trim();

        // variable if the player typed a valid spell
        bool spellFound = false;

        if (!string.IsNullOrEmpty(typedSpell))
        {
            // loop through every available spell in the list
            foreach (Spell spell in spells)
            {
                // skip null elements
                if (spell == null)
                    continue;

                // skip spells with null or empty incantation
                if (string.IsNullOrEmpty(spell.incantation))
                    continue;

                // compare the typed word with the spell incantation
                if (typedSpell == spell.incantation.ToLower())
                {
                    // cast if a match is found
                    CastSpell(spell);
                    spellFound = true;
                    break;
                }
            }
        }

        // for debugging, if spell is not found, then obviously it's invalid
        if (!spellFound)
        {
            Debug.Log("Invalid Spell");
        }

        // clear the field when spell is cast or invalid input
        ClearInput();
    }

    // this function runs when a valid spell is cast
    void CastSpell(Spell spell)
    {
        if (spell == null) return;

        // log for debugging
        Debug.Log("Spell Cast: " + spell.spellName);

        // call the spell effects manager to perform the spell effect based on puzzle type
        if (spellEffectsManager != null)
        {
            spellEffectsManager.CastSpellEffect(spell.puzzleType);
        }

        // optional: call the event attached to the spell (VFX, UI, etc.)
        if (spell.onSpellCast != null)
            spell.onSpellCast.Invoke();
    }

    // clears the field so player can type another spell
    void ClearInput()
    {
        if (spellInput == null) return;

        // remove text from the field
        spellInput.text = "";

        // reactivate the input field so typing continues immediately
        spellInput.ActivateInputField();
    }
}