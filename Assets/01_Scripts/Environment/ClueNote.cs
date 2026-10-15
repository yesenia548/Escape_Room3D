using UnityEngine;

public class ClueNote : MonoBehaviour, IInteractable
{
    public string Prompt => "Leer pista";
    public void Interact() { GameManager.Instance.ShowMessage("PISTA: uno, dos, tres, cuatro. Introduce 1234 en el panel."); }
}
