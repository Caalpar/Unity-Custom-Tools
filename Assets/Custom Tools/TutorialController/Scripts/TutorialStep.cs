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
    [HideInInspector]
    public string audioEnumTypeName;
    [HideInInspector]
    public string audioEnumValueName;
    [HideInInspector]
    public int audioEnumValueIndex;

    public bool audio= true;
    public int audioIndex;
    public GameObject prefabStep;
    public float stepDuration;
    public CompletionCondition completionCondition;
    public Actor[] actors;
}
