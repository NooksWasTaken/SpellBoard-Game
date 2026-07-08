using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class FragmentEntryUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text description;

    public void Initialize(PlayerFragmentManager.FragmentProgress progress)
    {
        icon.sprite = progress.fragment.fragmentIcon;

        description.text =
            $"{progress.collected} / {progress.fragment.fragmentsRequired} Fragments Collected";
    }
}
