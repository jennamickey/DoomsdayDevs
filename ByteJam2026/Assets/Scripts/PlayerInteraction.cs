using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private InteractionUI interactionUI;
    [SerializeField] private float interactionDistance = 6f;

    private IInteractable currentInteractable;

    private void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            currentInteractable = hit.collider.GetComponent<IInteractable>();

            if (currentInteractable != null)
            {
                interactionUI.ShowPrompt(currentInteractable.GetPrompt());

                if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
                {
                    currentInteractable.Interact();
                }

                return;
            }
        }

        currentInteractable = null;
        interactionUI.HidePrompt();
    }
}
