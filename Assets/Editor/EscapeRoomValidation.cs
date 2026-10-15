using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class EscapeRoomValidation
{
    [MenuItem("Escape Room/Validar assets y referencias")]
    public static void Validate()
    {
        int errors = 0;
        foreach (var name in new[] {"Player", "Chest", "Key", "Door", "Desk", "ClueNote"})
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/02_Prefabs/" + name + ".prefab");
            if (prefab == null) { Debug.LogError("Falta prefab: " + name); errors++; continue; }
            foreach (Transform child in prefab.GetComponentsInChildren<Transform>(true))
                if (GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(child.gameObject) > 0)
                { Debug.LogError("Script perdido en: " + name); errors++; }
        }
        var scene = EditorSceneManager.GetActiveScene();
        if (scene.path != "Assets/00_Scenes/MainScene.unity") { Debug.LogError("Abre MainScene para validar sus referencias."); return; }
        var game = Object.FindAnyObjectByType<GameManager>();
        var player = Object.FindAnyObjectByType<PlayerController>();
        var door = Object.FindAnyObjectByType<DoorController>();
        if (game == null || game.puzzle == null || game.puzzle.chest == null || game.puzzle.chest.key == null || game.puzzle.chest.lid == null)
        { Debug.LogError("Referencias incompletas: GameManager / puzzle / cofre / llave / tapa."); errors++; }
        if (player == null || player.view == null || player.GetComponent<PlayerInteraction>().playerCamera == null)
        { Debug.LogError("Referencias incompletas del jugador."); errors++; }
        if (door == null || door.hinge == null) { Debug.LogError("Falta puerta o bisagra."); errors++; }
        Debug.Log("Validacion de assets terminada. Errores: " + errors + ". La prueba completa en Play sigue siendo necesaria.");
    }
}

