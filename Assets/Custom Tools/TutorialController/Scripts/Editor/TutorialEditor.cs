using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(Tutorial))]
public class TutorialEditor : Editor
{
    private const string TUTORIAL_STEP_PREF_KEY = "PendingTutorialStepPath";
    private const string PREFAB_PATH_PREF_KEY = "PendingPrefabPath";
    private const string TUTORIAL_PATH_PREF_KEY = "CurrentTutorialPath";
    private Tutorial currentTutorial;

    // 👈 Agrega el atributo [SerializeField] para que Unity guarde su valor
    [SerializeField]
    private string newStepName = "New Step";

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        currentTutorial = (Tutorial)target;

        EditorGUILayout.Space(20);

        // 👈 Usa un EditorGUILayout.TextField normal
        newStepName = EditorGUILayout.TextField("Step Name", newStepName);

        if (GUILayout.Button("Add New Tutorial Step"))
        {
            if (string.IsNullOrEmpty(newStepName) || string.IsNullOrWhiteSpace(newStepName))
            {
                EditorUtility.DisplayDialog("Error", "El nombre del paso no puede estar vacío.", "Ok");
                return;
            }

            if (string.IsNullOrEmpty(AssetDatabase.GetAssetPath(currentTutorial)))
            {
                EditorUtility.DisplayDialog("Error", "Por favor, guarda el Tutorial ScriptableObject en tu proyecto antes de agregar pasos.", "Ok");
                return;
            }
            CreateNewTutorialStep();
        }
    }

    private void CreateNewTutorialStep()
    {
        if (currentTutorial.steps == null)
        {
            currentTutorial.steps = new TutorialStep[0];
        }

        string currentTutorialPath = AssetDatabase.GetAssetPath(currentTutorial);

        // Sanitizar el nombre para nombres de archivos válidos
        string sanitizedName = newStepName.Replace(" ", "_").Trim();
        string finalStepName = sanitizedName + "_Step_" + (currentTutorial.steps.Length + 1);
        string finalStepNameDirectory = (currentTutorial.steps.Length + 1) + "_Step_" + sanitizedName;

        string parentDirectory = Path.GetDirectoryName(currentTutorialPath);
        string newDirectoryPath = Path.Combine(parentDirectory, finalStepNameDirectory);

        if (!Directory.Exists(newDirectoryPath))
        {
            Directory.CreateDirectory(newDirectoryPath);
            AssetDatabase.Refresh();
        }

        string scriptPath = Path.Combine(newDirectoryPath, finalStepName + ".cs");
        string prefabPath = Path.Combine(newDirectoryPath, finalStepNameDirectory + ".prefab");
        string finalAssetPath = Path.Combine(newDirectoryPath, finalStepNameDirectory + ".asset");

        string scriptTemplate = $@"using UnityEngine;
using System.Linq;

public class {finalStepName} : ActionStep
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

        TutorialStep newTutorialStep = ScriptableObject.CreateInstance<TutorialStep>();
        AssetDatabase.CreateAsset(newTutorialStep, finalAssetPath);
        AssetDatabase.SaveAssets();

        EditorPrefs.SetString(TUTORIAL_STEP_PREF_KEY, finalAssetPath);
        EditorPrefs.SetString(PREFAB_PATH_PREF_KEY, prefabPath);
        EditorPrefs.SetString(TUTORIAL_PATH_PREF_KEY, currentTutorialPath);

        TutorialStep[] newArrSteps = new TutorialStep[currentTutorial.steps.Length + 1];

        for (int i = 0; i < currentTutorial.steps.Length; i++)
        {
            newArrSteps[i] = currentTutorial.steps[i];
        }

        newArrSteps[newArrSteps.Length - 1] = newTutorialStep;

        currentTutorial.steps = newArrSteps;

        Debug.Log("Script y ScriptableObject del paso creados. La creación del prefab y el enlace continuarán después de la compilación.");
    }
}