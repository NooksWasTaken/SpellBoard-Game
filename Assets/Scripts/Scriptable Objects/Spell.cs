using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewSpell", menuName = "Spell System/Spell")]
public class Spell : ScriptableObject
{
    [Header("Spell Name")]
    public string spellName;    // this is very obvious

    [Header("Spell Incantation")]
    public string incantation;  // this the word the player must type

    [Header("Spell Icon")]
    // put icon variable here

    [Header("Spell VFX")]
    // put spell vfx here

    public UnityEvent onSpellCast; // the event triggered when spell is casted
}