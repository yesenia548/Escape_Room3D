using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool HasKey { get; private set; }
    public bool Escaped { get; private set; }
    public bool CodePanelOpen;
    public bool BlocksMovement => CodePanelOpen || Escaped;
    public string Hint = "";
    public PuzzleManager puzzle;
    string message = "Haz clic para comenzar. Busca una pista en el escritorio.";
    string enteredCode = "";
    float messageUntil;
    float startTime;
    float finishTime;
    void Awake() { Instance = this; startTime = Time.time; messageUntil = Time.time + 12; }
    public void ShowMessage(string text) { message = text; messageUntil = Time.time + 6; }
    public void ObtainKey() { HasKey = true; ShowMessage("Llave obtenida. Ya puedes abrir la salida."); }
    public void Win()
    {
        if (Escaped) return;
        Escaped = true;
        finishTime = Time.time - startTime;
        UnlockCursor();
        Debug.Log("Escape completado: puzzle, cofre, llave y puerta verificados.");
    }
    public void OpenCodePanel() { enteredCode = ""; CodePanelOpen = true; UnlockCursor(); }
    void UnlockCursor() { Cursor.lockState = CursorLockMode.None; Cursor.visible = true; }
    void ClosePanel() { CodePanelOpen = false; Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false; }
    void OnGUI()
    {
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Screen.width / 1280f, Screen.height / 720f, 1));
        GUI.skin.label.fontSize = 20;
        GUI.skin.button.fontSize = 20;
        GUI.skin.textField.fontSize = 26;
        GUI.Box(new Rect(20, 20, 440, 100), "");
        GUI.Label(new Rect(38, 30, 410, 35), "ESCAPE ROOM  |  El archivo secreto");
        GUI.Label(new Rect(38, 70, 400, 35), HasKey ? "Inventario: llave de salida" : "Objetivo: pista > codigo > cofre > llave");
        GUI.Label(new Rect(25, 675, 1000, 30), "WASD: mover   |   Mouse: mirar   |   E: interactuar   |   Esc: liberar cursor");
        if (!BlocksMovement)
        {
            GUI.Label(new Rect(633, 345, 30, 30), "+");
            GUI.Label(new Rect(390, 530, 650, 40), Hint);
        }
        if (Time.time < messageUntil) { GUI.Box(new Rect(230, 585, 820, 65), ""); GUI.Label(new Rect(245, 598, 790, 45), message); }
        if (CodePanelOpen)
        {
            GUI.Box(new Rect(410, 190, 460, 300), "");
            GUI.Label(new Rect(445, 220, 400, 45), "PANEL DE SEGURIDAD - 4 DIGITOS");
            enteredCode = GUI.TextField(new Rect(465, 285, 350, 50), enteredCode, 4);
            if (GUI.Button(new Rect(465, 355, 350, 50), "VALIDAR CODIGO"))
            {
                if (puzzle.ValidateCode(enteredCode)) ClosePanel();
                else ShowMessage("Codigo incorrecto. Revisa la pista del escritorio.");
            }
            if (GUI.Button(new Rect(465, 420, 350, 40), "Volver")) ClosePanel();
        }
        if (Escaped)
        {
            GUI.Box(new Rect(340, 210, 600, 290), "");
            GUI.Label(new Rect(445, 250, 480, 50), "¡ESCAPE COMPLETADO!");
            GUI.Label(new Rect(440, 310, 480, 45), "Tiempo: " + Mathf.CeilToInt(finishTime) + " segundos");
            if (GUI.Button(new Rect(450, 390, 380, 60), "Jugar otra vez")) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    void OnDestroy() { if (Instance == this) Instance = null; }
}
