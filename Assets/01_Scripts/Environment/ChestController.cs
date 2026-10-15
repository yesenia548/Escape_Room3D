using UnityEngine;

public class ChestController : MonoBehaviour, IInteractable
{
    public Transform lid;
    public GameObject key;
    public bool unlocked;
    public bool opened;
    public string Prompt => opened ? "Cofre abierto" : "Abrir cofre";
    public void Unlock() { unlocked = true; }
    public void Interact()
    {
        if (!unlocked) { GameManager.Instance.ShowMessage("Cofre cerrado. Resuelve el codigo en el panel."); return; }
        opened = true;
        if (!GameManager.Instance.HasKey) key.SetActive(true);
        GameManager.Instance.ShowMessage("Cofre abierto. Recoge la llave dorada con E.");
    }
    void Update()
    {
        if (opened) lid.localRotation = Quaternion.RotateTowards(lid.localRotation, Quaternion.Euler(-105, 0, 0), 100 * Time.deltaTime);
    }
}
