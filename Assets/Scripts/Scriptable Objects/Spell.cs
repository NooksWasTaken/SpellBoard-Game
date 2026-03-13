using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewSpell", menuName = "Spell System/Spell")]
public class Spell : ScriptableObject
{
    [Header("Spell Name")]
    public string spellName;        // this is very obvious

    [Header("Spell Incantation")]
    public string incantation;      // this the word the player must type

    [Header("Puzzle Type")]
    public PuzzleType puzzleType;   // this will determine what puzzle this spell is used on

    [Header("Spell Icon")]
    // put icon variable here

    [Header("Spell VFX")]
    // put spell vfx here

    public UnityEvent onSpellCast; // the event triggered when spell is casted
}