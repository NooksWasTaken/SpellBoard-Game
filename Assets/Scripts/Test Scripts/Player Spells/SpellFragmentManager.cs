using System.Collections.Generic;
using UnityEngine;

public class PlayerFragmentManager : MonoBehaviour
{
    [System.Serializable]
    public class FragmentProgress
    {
        public SpellFragment fragment;
        public int collected;

        public int Required => fragment.fragmentsRequired;
    }

    [SerializeField]
    private List<FragmentProgress> progress = new();

    [SerializeField]
    private PlayerSpellBoard spellBoard;

    public bool AddFragment(SpellFragment fragment)
    {
        FragmentProgress entry =
            progress.Find(x => x.fragment == fragment);

        if (entry == null)
        {
            entry = new FragmentProgress();

            entry.fragment = fragment;
            entry.collected = 0;

            progress.Add(entry);
            UIAlertManager.Instance.SetJournalAlert(true);
        }

        entry.collected++;
        SpellJournal.Instance?.Refresh();

        Debug.Log($"{fragment.fragmentName}: {entry.collected}/{fragment.fragmentsRequired}");

        if (entry.collected >= fragment.fragmentsRequired)
        {
            if (spellBoard.AddSpell(fragment.completedSpell))
            {
                Debug.Log($"Completed spell: {fragment.completedSpell.spellName}");
            }

            progress.Remove(entry);
            SpellJournal.Instance?.Refresh();

            return true;
        }

        return false;
    }

    public int GetProgress(SpellFragment fragment)
    {
        FragmentProgress entry =
            progress.Find(x => x.fragment == fragment);

        return entry == null ? 0 : entry.collected;
    }

    public IReadOnlyList<FragmentProgress> GetFragmentProgress()
    {
        return progress;
    }
}