using UnityEngine;

public class KeyItem : MonoBehaviour, IInteractable
{
    public string Prompt => "Recoger llave";
    public void Interact() { GameManager.Instance.ObtainKey(); gameObject.SetActive(false); }
    void Update() { transform.Rotate(0, 45 * Time.deltaTime, 0); }
}
