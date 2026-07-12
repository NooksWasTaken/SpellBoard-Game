using UnityEngine;

[CreateAssetMenu(menuName = "Spell System/Player Spell")]
public class PlayerSpells : ScriptableObject
{
    [Header("Spell Name")]
    public string spellName;

    [Header("Spell Activation Word.")]
    public string keyword;

    [TextArea]
    public string description;

    [Header("UI")]
    public Sprite spellIcon;

    [Header("Spell VFX")]
    public ParticleSystem castEffect;
}