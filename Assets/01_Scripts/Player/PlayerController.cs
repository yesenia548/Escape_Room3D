using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float speed = 4.2f;
    public Transform view;
    float pitch;
    float fall;
    CharacterController controller;
    void Awake() { controller = GetComponent<CharacterController>(); }
    void Update()
    {
        if (Keyboard.current == null || Mouse.current == null) return;
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if (GameManager.Instance == null || GameManager.Instance.BlocksMovement) return;
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if (Cursor.lockState != CursorLockMode.Locked) return;
        Vector2 delta = Mouse.current.delta.ReadValue() * .1f;
        transform.Rotate(0, delta.x, 0);
        pitch = Mathf.Clamp(pitch - delta.y, -75, 75);
        view.localRotation = Quaternion.Euler(pitch, 0, 0);
        var k = Keyboard.current;
        float x = (k.dKey.isPressed ? 1 : 0) - (k.aKey.isPressed ? 1 : 0);
        float z = (k.wKey.isPressed ? 1 : 0) - (k.sKey.isPressed ? 1 : 0);
        Vector3 motion = Vector3.ClampMagnitude(transform.right * x + transform.forward * z, 1) * speed;
        fall = controller.isGrounded ? -2f : fall + Physics.gravity.y * Time.deltaTime;
        controller.Move((motion + Vector3.up * fall) * Time.deltaTime);
    }
    void OnDisable() { Cursor.lockState = CursorLockMode.None; Cursor.visible = true; }
}
