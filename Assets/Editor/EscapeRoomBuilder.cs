using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

[InitializeOnLoad]
public static class EscapeRoomBuilder
{
    const string Root = "Assets/";
    static EscapeRoomBuilder() { EditorApplication.delayCall += BuildFirstTime; }
    static void BuildFirstTime()
    {
        if (!System.IO.File.Exists(Root + "06_UI/Controls.txt") && !EditorApplication.isPlayingOrWillChangePlaymode)
            Build();
    }
    static Material Mat(string name, Color color, Texture2D texture = null)
    {
        string path = Root + "03_Materials/" + name + ".mat";
        var existing = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (existing != null) return existing;
        var material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        material.name = name;
        material.color = color;
        if (texture != null) { material.mainTexture = texture; material.mainTextureScale = new Vector2(6, 6); }
        material.SetFloat("_Smoothness", .25f);
        AssetDatabase.CreateAsset(material, path);
        return material;
    }
    static GameObject Box(string name, Vector3 position, Vector3 scale, Material material, Transform parent = null)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = name;
        go.transform.SetParent(parent, false);
        go.transform.localPosition = position;
        go.transform.localScale = scale;
        go.GetComponent<Renderer>().sharedMaterial = material;
        return go;
    }
    static void Label(string name, string text, Vector3 position, Vector3 euler, float size, Transform parent = null)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent, false);
        go.transform.localPosition = position;
        go.transform.localRotation = Quaternion.Euler(euler);
        var tm = go.AddComponent<TextMesh>();
        tm.text = text;
        tm.fontSize = 64;
        tm.characterSize = size;
        tm.anchor = TextAnchor.MiddleCenter;
        tm.alignment = TextAlignment.Center;
        tm.color = Color.white;
    }
    [MenuItem("Escape Room/Crear escenario completo")]
    public static void Build()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode) return;
        foreach (var folder in new[] {"00_Scenes", "01_Scripts", "02_Prefabs", "03_Materials", "04_Models", "05_Textures", "06_UI"})
            System.IO.Directory.CreateDirectory(Root + folder);
        AssetDatabase.Refresh();
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        var texture = new Texture2D(64, 64);
        for (int y = 0; y < 64; y++) for (int x = 0; x < 64; x++)
            texture.SetPixel(x, y, ((x / 16 + y / 16) % 2 == 0) ? new Color(.7f,.72f,.74f) : new Color(.4f,.43f,.46f));
        texture.Apply();
        System.IO.File.WriteAllBytes(Root + "05_Textures/FloorTiles.png", texture.EncodeToPNG());
        Object.DestroyImmediate(texture);
        AssetDatabase.ImportAsset(Root + "05_Textures/FloorTiles.png");
        texture = AssetDatabase.LoadAssetAtPath<Texture2D>(Root + "05_Textures/FloorTiles.png");
        var floor = Mat("Floor", new Color(.45f,.5f,.55f), texture);
        var wall = Mat("Walls", new Color(.16f,.23f,.3f));
        var wood = Mat("Wood", new Color(.32f,.14f,.06f));
        var teal = Mat("Teal", new Color(.02f,.6f,.59f));
        var gold = Mat("Gold", new Color(1,.68f,.12f));
        var paper = Mat("Paper", new Color(.85f,.73f,.43f));
        var dark = Mat("Dark", new Color(.03f,.045f,.055f));
        var game = new GameObject("GameManager").AddComponent<GameManager>();
        var room = new GameObject("Room").transform;
        Box("Floor", new Vector3(0,-.15f,1), new Vector3(12,.3f,14), floor, room);
        Box("WestWall", new Vector3(-6,2,0), new Vector3(.3f,4,10), wall, room);
        Box("EastWall", new Vector3(6,2,0), new Vector3(.3f,4,10), wall, room);
        Box("SouthWall", new Vector3(0,2,-5), new Vector3(12,4,.3f), wall, room);
        Box("NorthLeft", new Vector3(-3.6f,2,5), new Vector3(4.8f,4,.3f), wall, room);
        Box("NorthRight", new Vector3(3.6f,2,5), new Vector3(4.8f,4,.3f), wall, room);
        Box("DoorTop", new Vector3(0,3.6f,5), new Vector3(2.4f,.8f,.3f), teal, room);
        Box("Ceiling", new Vector3(0,4,0), new Vector3(12,.2f,10), wall, room);
        for (int i = 0; i < 5; i++) Box("WallTrim_"+i, new Vector3(-5.8f, .18f, -4+i*2), new Vector3(.08f,.2f,1.7f), teal, room);
        var desk = new GameObject("Desk");
        Box("Top", new Vector3(0,1.05f,0), new Vector3(2.5f,.15f,1.3f), wood, desk.transform);
        foreach (float x in new[]{-1f,1f}) foreach(float z in new[]{-.45f,.45f})
            Box("Leg", new Vector3(x,.5f,z),new Vector3(.15f,1,.15f), dark, desk.transform);
        PrefabUtility.SaveAsPrefabAsset(desk, Root+"02_Prefabs/Desk.prefab");
        desk.transform.position = new Vector3(-3.7f,0,1.3f);
        var note = Box("ClueNote",new Vector3(-3.7f,1.15f,1.3f),new Vector3(.8f,.035f,.5f),paper);
        note.AddComponent<ClueNote>();
        PrefabUtility.SaveAsPrefabAsset(note,Root+"02_Prefabs/ClueNote.prefab");
        Label("ClueSign","PISTA: 1 - 2 - 3 - 4",new Vector3(-3.7f,1.7f,1.85f),Vector3.zero,.025f);
        var chestObject = new GameObject("Chest");
        chestObject.transform.position = new Vector3(0,0,2.6f);
        var chest = chestObject.AddComponent<ChestController>();
        Box("Body",new Vector3(0,.45f,0),new Vector3(1.8f,.9f,1),wood,chestObject.transform);
        Box("Lock",new Vector3(0,.65f,-.54f),new Vector3(.2f,.3f,.08f),gold,chestObject.transform);
        var pivot = new GameObject("LidPivot").transform;
        pivot.SetParent(chestObject.transform,false); pivot.localPosition=new Vector3(0,.92f,.5f);
        Box("Lid",new Vector3(0,.07f,-.5f),new Vector3(1.9f,.14f,1.1f),wood,pivot);
        chest.lid=pivot;
        var key = new GameObject("Key"); key.transform.SetParent(chestObject.transform,false); key.transform.localPosition=new Vector3(0,1.12f,-.1f);
        key.AddComponent<KeyItem>(); var keyCollider=key.AddComponent<BoxCollider>(); keyCollider.size=new Vector3(.65f,.25f,.25f);
        Box("KeyShaft",Vector3.zero,new Vector3(.55f,.08f,.08f),gold,key.transform);
        Box("KeyTooth",new Vector3(.2f,-.06f,0),new Vector3(.08f,.17f,.08f),gold,key.transform);
        var ring=GameObject.CreatePrimitive(PrimitiveType.Sphere); ring.name="KeyHead"; ring.transform.SetParent(key.transform,false); ring.transform.localPosition=new Vector3(-.28f,0,0); ring.transform.localScale=new Vector3(.22f,.22f,.12f); ring.GetComponent<Renderer>().sharedMaterial=gold;
        PrefabUtility.SaveAsPrefabAsset(key,Root+"02_Prefabs/Key.prefab");
        chest.key=key; key.SetActive(false);
        PrefabUtility.SaveAsPrefabAsset(chestObject,Root+"02_Prefabs/Chest.prefab");
        var panel = Box("CodePanel",new Vector3(4,1.45f,2.3f),new Vector3(1.1f,1.2f,.22f),teal);
        var puzzle=panel.AddComponent<PuzzleManager>(); puzzle.chest=chest; game.puzzle=puzzle;
        Label("PanelLabel","CODIGO\n[E]",new Vector3(4,1.5f,2.16f),Vector3.zero,.045f);
        var doorObject=new GameObject("Door");
        var door=doorObject.AddComponent<DoorController>();
        var hinge=new GameObject("DoorHinge").transform; hinge.SetParent(doorObject.transform,false); hinge.localPosition=new Vector3(-1.2f,0,5);
        Box("DoorLeaf",new Vector3(1.2f,1.6f,0),new Vector3(2.35f,3.2f,.15f),teal,hinge);
        Box("Handle",new Vector3(2.05f,1.45f,-.15f),new Vector3(.25f,.08f,.15f),gold,hinge);
        door.hinge=hinge;
        PrefabUtility.SaveAsPrefabAsset(doorObject,Root+"02_Prefabs/Door.prefab");
        Label("ExitSign","SALIDA",new Vector3(0,3.55f,4.8f),Vector3.zero,.06f);
        var exit = new GameObject("ExitZone"); exit.transform.position=new Vector3(0,1.5f,6.3f);
        var exitCollider=exit.AddComponent<BoxCollider>();exitCollider.size=new Vector3(2.4f,3,1);exitCollider.isTrigger=true;exit.AddComponent<ExitZone>();
        var rb=exit.AddComponent<Rigidbody>();rb.isKinematic=true;rb.useGravity=false;
        var player=new GameObject("Player");player.transform.position=new Vector3(0,.1f,-2.8f);
        var controller=player.AddComponent<CharacterController>();controller.height=1.8f;controller.center=new Vector3(0,.9f,0);controller.radius=.3f;
        var cameraObject=new GameObject("PlayerCamera");cameraObject.tag="MainCamera";cameraObject.transform.SetParent(player.transform,false);cameraObject.transform.localPosition=new Vector3(0,1.6f,0);
        var camera=cameraObject.AddComponent<Camera>();camera.nearClipPlane=.05f;camera.farClipPlane=100;camera.backgroundColor=new Color(.04f,.06f,.1f);cameraObject.AddComponent<AudioListener>();
        var movement=player.AddComponent<PlayerController>();movement.view=cameraObject.transform;
        var interaction=player.AddComponent<PlayerInteraction>();interaction.playerCamera=camera;
        PrefabUtility.SaveAsPrefabAsset(player,Root+"02_Prefabs/Player.prefab");
        var sun=new GameObject("MainLight").AddComponent<Light>();sun.type=LightType.Directional;sun.intensity=.5f;sun.transform.rotation=Quaternion.Euler(45,-30,0);
        foreach(float x in new[]{-3f,3f}) {var light=new GameObject("CeilingLight").AddComponent<Light>();light.type=LightType.Point;light.range=12;light.intensity=5;light.color=new Color(1,.86f,.67f);light.transform.position=new Vector3(x,3.5f,0);}
        RenderSettings.ambientMode=AmbientMode.Flat;RenderSettings.ambientLight=new Color(.35f,.4f,.48f);
        var cube=GameObject.CreatePrimitive(PrimitiveType.Cube); var mesh=Object.Instantiate(cube.GetComponent<MeshFilter>().sharedMesh);mesh.name="RoomBlock";
        var meshPath=Root+"04_Models/RoomBlock.asset";
        if (AssetDatabase.LoadAssetAtPath<Mesh>(meshPath)==null) AssetDatabase.CreateAsset(mesh,meshPath); else Object.DestroyImmediate(mesh);
        Object.DestroyImmediate(cube);
        EditorSceneManager.SaveScene(scene,Root+"00_Scenes/MainScene.unity");
        EditorBuildSettings.scenes=new[]{new EditorBuildSettingsScene(Root+"00_Scenes/MainScene.unity",true)};
        System.IO.File.WriteAllText(Root+"06_UI/Controls.txt","ESCAPE ROOM\nWASD: mover\nMouse: mirar\nE: interactuar\nEsc: liberar cursor\nCodigo: 1234\nFlujo: pista > panel > cofre > llave > puerta > cruzar salida.\nInterfaz: GameManager.OnGUI, escala de referencia 1280x720.\n");
        AssetDatabase.SaveAssets();AssetDatabase.Refresh();
        Debug.Log("Escape Room: escenario, materiales, textura, modelos y prefabs creados.");
    }
}
