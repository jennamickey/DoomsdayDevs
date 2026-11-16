using UnityEngine;
using TMPro;

public class InteractionUI : MonoBehaviour
{
    [SerializeField] private GameObject interactionPanel;
    [SerializeField] private TMP_Text interactionText;

    private void Start()
    {
        HidePrompt();
    }

    public void ShowPrompt(string prompt)
    {
        interactionPanel.SetActive(true);
        interactionText.text = prompt;
    }

    public void HidePrompt()
    {
        interactionPanel.SetActive(false);
    }
}
