using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateTutorialStep
{
    private const string TUTORIAL_STEP_PATH_PREF_KEY = "PendingTutorialStepPath";
    private const string PREFAB_PATH_PREF_KEY = "PendingPrefabPath";

    [MenuItem("Tutorial/Create New Tutorial Step")]
    public static void CreateNewTutorialStep()
    {
        string newAssetPath = EditorUtility.SaveFilePanelInProject(
            "Guardar Nuevo Tutorial Step",
            "NewTutorialStep",
            "asset",
            "Por favor, elige la ubicación y el nombre para el nuevo TutorialStep."
        );

        if (string.IsNullOrEmpty(newAssetPath))
        {
            return;
        }

        string newAssetName = Path.GetFileNameWithoutExtension(newAssetPath);
        string parentDirectory = Path.GetDirectoryName(newAssetPath);
        string newDirectoryPath = Path.Combine(parentDirectory, newAssetName);

        // Crear la nueva carpeta si no existe
        if (!Directory.Exists(newDirectoryPath))
        {
            Directory.CreateDirectory(newDirectoryPath);
            AssetDatabase.Refresh();
        }

        string scriptPath = Path.Combine(newDirectoryPath, newAssetName + ".cs");
        string prefabPath = Path.Combine(newDirectoryPath, newAssetName + ".prefab");
        string finalAssetPath = Path.Combine(newDirectoryPath, newAssetName + ".asset");

        // 1. Crear el C# Script
        string scriptTemplate = $@"using UnityEngine;
using System.Linq;

public class {newAssetName} : ActionStep
{{
    private void Start()
    {{

    }}

    private void Update()
    {{

    }}
    public override void StartStep(Actor[] actorsInStep, Actor[] actorsInTutorial,StepState stepState)
    {{
        base.StartStep(actorsInStep, actorsInTutorial,stepState);
    }}

}}";
        File.WriteAllText(scriptPath, scriptTemplate);
        AssetDatabase.Refresh();

        // 2. Crear el ScriptableObject
        TutorialStep newTutorialStep = ScriptableObject.CreateInstance<TutorialStep>();
        AssetDatabase.CreateAsset(newTutorialStep, finalAssetPath);
        AssetDatabase.SaveAssets();

        // 3. Guardar las rutas en EditorPrefs
        EditorPrefs.SetString(TUTORIAL_STEP_PATH_PREF_KEY, finalAssetPath);
        EditorPrefs.SetString(PREFAB_PATH_PREF_KEY, prefabPath);

        Debug.Log("Script y ScriptableObject creados. La creación del prefab y el enlace continuarán después de la compilación.");
    }

    [InitializeOnLoadMethod]
    private static void OnProjectCompile()
    {
        if (EditorPrefs.HasKey(TUTORIAL_STEP_PATH_PREF_KEY) && EditorPrefs.HasKey(PREFAB_PATH_PREF_KEY))
        {
            string newAssetPath = EditorPrefs.GetString(TUTORIAL_STEP_PATH_PREF_KEY);
            string prefabPath = EditorPrefs.GetString(PREFAB_PATH_PREF_KEY);

            EditorPrefs.DeleteKey(TUTORIAL_STEP_PATH_PREF_KEY);
            EditorPrefs.DeleteKey(PREFAB_PATH_PREF_KEY);

            EditorApplication.delayCall += () =>
            {
                CreateAssetsAfterCompilation(newAssetPath, prefabPath);
            };
        }
    }

    private static void CreateAssetsAfterCompilation(string newAssetPath, string prefabPath)
    {

        Debug.Log("newAssetPath:" + newAssetPath);
        Debug.Log("prefabPath:" + prefabPath);

        string []newAssetNameArray = Path.GetFileNameWithoutExtension(newAssetPath).Split("_");

        string newAssetName = "";

        for (int i = 2; i < newAssetNameArray.Length; i++)
        {
            newAssetName += newAssetNameArray[i] + "_";
        }

        newAssetName += newAssetNameArray[1] + "_" + newAssetNameArray[0];



        Debug.Log("newAssetName:" + newAssetName);

        System.Type scriptType = System.Type.GetType($"{newAssetName}, Assembly-CSharp");
        if (scriptType == null)
        {
            Debug.LogError("El tipo de script aún no está disponible.");
            return;
        }

        // 4. Crear el Prefab
        GameObject newPrefabGameObject = new GameObject(newAssetName);
        newPrefabGameObject.AddComponent(scriptType);
        PrefabUtility.SaveAsPrefabAsset(newPrefabGameObject, prefabPath);
        GameObject.DestroyImmediate(newPrefabGameObject);
        AssetDatabase.Refresh();

        // 5. Cargar los assets y enlazarlos
        TutorialStep tutorialStep = AssetDatabase.LoadAssetAtPath<TutorialStep>(newAssetPath);
        GameObject prefabStep = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

        if (tutorialStep != null && prefabStep != null)
        {
            tutorialStep.prefabStep = prefabStep;
            EditorUtility.SetDirty(tutorialStep);
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = tutorialStep;
            Debug.Log("¡TutorialStep, Prefab, y Script creados y enlazados exitosamente! 🎉");
        }
        else
        {
            Debug.LogError("No se pudo encontrar el TutorialStep o el Prefab para enlazar.");
        }
    }
}