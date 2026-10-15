using UnityEngine;

public class DoorController : MonoBehaviour, IInteractable
{
    public Transform hinge;
    bool open;
    public string Prompt => "Abrir puerta de salida";
    public void Interact()
    {
        if (!GameManager.Instance.HasKey) { GameManager.Instance.ShowMessage("Necesitas la llave del cofre."); return; }
        open = true;
        GameManager.Instance.ShowMessage("Puerta abierta. Cruza el umbral para escapar.");
    }
    void Update()
    {
        if (open) hinge.localRotation = Quaternion.RotateTowards(hinge.localRotation, Quaternion.Euler(0, -100, 0), 80 * Time.deltaTime);
    }
}
