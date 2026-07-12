using UnityEngine;

[CreateAssetMenu(menuName = "Spell System/Spell Fragment")]
public class SpellFragment : ScriptableObject
{
    [Header("Unlocks")]
    public PlayerSpells completedSpell;

    [Header("Fragment")]
    public string fragmentName;

    public Sprite fragmentIcon;

    public int fragmentsRequired = 3;
}