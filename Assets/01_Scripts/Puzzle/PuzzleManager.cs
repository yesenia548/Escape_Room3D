using UnityEngine;

public class PuzzleManager : MonoBehaviour, IInteractable
{
    [SerializeField] string correctCode = "1234";
    public ChestController chest;
    public bool Solved { get; private set; }
    public string Prompt => Solved ? "Panel desbloqueado" : "Ingresar codigo";
    public void Interact()
    {
        if (Solved) GameManager.Instance.ShowMessage("El cofre esta desbloqueado. Abrelo y recoge la llave.");
        else GameManager.Instance.OpenCodePanel();
    }
    public bool ValidateCode(string code)
    {
        if (code != correctCode) return false;
        Solved = true;
        chest.Unlock();
        GameManager.Instance.ShowMessage("Codigo correcto. El cofre esta desbloqueado.");
        return true;
    }
}
