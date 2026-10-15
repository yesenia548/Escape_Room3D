using UnityEngine;

public class ExitZone : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null && GameManager.Instance.HasKey && GameManager.Instance.puzzle.Solved)
            GameManager.Instance.Win();
    }
}
