using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public static class LaboratorySetup
{
    static LaboratorySetup() { EditorApplication.delayCall += Prepare; }
    static void Prepare()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isCompiling) return;
        if (System.IO.File.Exists("Assets/00_Scenes/MainScene.unity")) return;

        EditorSettings.serializationMode = SerializationMode.ForceText;
        VersionControlSettings.mode = "Visible Meta Files";
        var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
        EditorSceneManager.SaveScene(scene, "Assets/00_Scenes/MainScene.unity");
        EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene("Assets/00_Scenes/MainScene.unity", true) };
        AssetDatabase.SaveAssets();
        Debug.Log("Laboratorio: MainScene creada. Caso 01 pendiente de Git y evidencias.");
    }
}


