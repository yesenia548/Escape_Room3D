using System;
using System.IO;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class EscapeRoomSmokeTest
{
    const string Flag = "EscapeRoom.SmokeTest";
    static int phase;
    static double next;
    static string report;
    static EscapeRoomSmokeTest()
    {
        EditorApplication.playModeStateChanged += OnPlayState;
        EditorApplication.update += Tick;
    }
    [MenuItem("Escape Room/Probar flujo de juego automaticamente")]
    public static void Run()
    {
        if (EditorApplication.isPlaying) return;
        SessionState.SetBool(Flag, true);
        EditorApplication.isPlaying = true;
    }
    static void OnPlayState(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredPlayMode && SessionState.GetBool(Flag, false))
        {
            report = "PRUEBA AUTOMATIZADA DE ESTADOS EN PLAY\nNo sustituye la prueba manual de teclado, mouse y raycast.\n";
            phase = 1; next = EditorApplication.timeSinceStartup + .5;
        }
    }
    static void Check(bool condition, string label)
    {
        if (!condition) throw new Exception(label);
        report += "PASS: " + label + "\n";
    }
    static void Tick()
    {
        if (!EditorApplication.isPlaying || phase == 0 || EditorApplication.timeSinceStartup < next) return;
        try
        {
            var game = GameManager.Instance;
            if (phase == 1)
            {
                Check(game != null && game.puzzle != null, "Referencias iniciales");
                var chest = game.puzzle.chest;
                Check(!game.HasKey && !game.Escaped && !chest.unlocked && !chest.key.activeSelf, "Estado inicial bloqueado");
                chest.Interact();
                Check(!chest.opened && !chest.key.activeSelf, "Cofre rechaza apertura sin codigo");
                UnityEngine.Object.FindAnyObjectByType<DoorController>().Interact();
                phase = 2; next = EditorApplication.timeSinceStartup + .3;
            }
            else if (phase == 2)
            {
                var door = UnityEngine.Object.FindAnyObjectByType<DoorController>();
                Check(Quaternion.Angle(door.hinge.localRotation, Quaternion.identity) < .1f, "Puerta rechaza apertura sin llave");
                Check(!game.puzzle.ValidateCode("0000") && !game.puzzle.chest.unlocked, "Codigo incorrecto rechazado");
                Check(game.puzzle.ValidateCode("1234") && game.puzzle.chest.unlocked, "Codigo correcto desbloquea cofre");
                game.puzzle.chest.Interact();
                Check(game.puzzle.chest.opened && game.puzzle.chest.key.activeSelf, "Cofre abierto revela llave");
                game.puzzle.chest.key.GetComponent<KeyItem>().Interact();
                Check(game.HasKey && !game.puzzle.chest.key.activeSelf, "Llave entra al inventario");
                door.Interact();
                phase = 3; next = EditorApplication.timeSinceStartup + .5;
            }
            else if (phase == 3)
            {
                var door = UnityEngine.Object.FindAnyObjectByType<DoorController>();
                Check(Quaternion.Angle(door.hinge.localRotation, Quaternion.identity) > 5, "Puerta se anima con llave");
                Check(Quaternion.Angle(game.puzzle.chest.lid.localRotation, Quaternion.identity) > 5, "Tapa del cofre se anima");
                var player = UnityEngine.Object.FindAnyObjectByType<PlayerController>();
                var controller = player.GetComponent<CharacterController>();
                controller.enabled = false; player.transform.position = new Vector3(0,.1f,6.3f); controller.enabled = true;
                phase = 4; next = EditorApplication.timeSinceStartup + 1;
            }
            else
            {
                Check(game.Escaped, "Trigger de salida completa el escape");
                Finish(true);
            }
        }
        catch (Exception error) { report += "FAIL: " + error.Message + "\n"; Finish(false); }
    }
    static void Finish(bool success)
    {
        phase = 0;
        SessionState.SetBool(Flag, false);
        report += success ? "RESULTADO: PASS\n" : "RESULTADO: FAIL\n";
        File.WriteAllText(Path.GetFullPath(Path.Combine(Application.dataPath,"../../Pruebas_Juego.txt")), report);
        if (success) Debug.Log("Prueba automatizada de estados: PASS"); else Debug.LogError(report);
        EditorApplication.isPlaying = false;
    }
}

