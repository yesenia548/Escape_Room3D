using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public Camera playerCamera;
    public float interactionDistance = 4f;
    void Update()
    {
        var game = GameManager.Instance;
        if (game == null || game.BlocksMovement) return;
        game.Hint = "";
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, interactionDistance))
        {
            foreach (var component in hit.collider.GetComponentsInParent<MonoBehaviour>())
            {
                if (!(component is IInteractable item)) continue;
                game.Hint = "[E] " + item.Prompt;
                if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame) item.Interact();
                break;
            }
        }
    }
}
