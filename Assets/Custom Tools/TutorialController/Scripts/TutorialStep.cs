using UnityEngine;
using UnityEngine.Video;

public enum CompletionCondition
{
    TimeBased,
    ActionBased
}

[CreateAssetMenu(fileName = "TutorialStep", menuName = "Tutorial/TutorialStep")]
public class TutorialStep : ScriptableObject
{
    
    int audioIndex;

    public int AudioIndex { set { audioIndex = value; } get { return audioIndex; } }
    
    [HideInInspector]
    public string audioEnumTypeName;
    [HideInInspector]
    public string audioEnumValueName;

    public bool audio= true;
    public GameObject prefabStep;
    public float stepDuration;
    public CompletionCondition completionCondition;
    public Actor[] actors;
}
