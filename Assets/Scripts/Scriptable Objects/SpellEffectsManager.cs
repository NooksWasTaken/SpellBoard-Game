using UnityEngine;

public class SpellEffectsManager : MonoBehaviour
{
    // this script will handle what each individual spell will do
    // these functions will be called by their respective spells
    public void Restore()
    {
        Debug.Log("PLAYER CASTED RESTORE!");

    }

    public void Purify()
    {
        Debug.Log("PLAYER CASTED PURIFY!");
    }
}
