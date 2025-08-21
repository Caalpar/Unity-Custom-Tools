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
    public bool audio= true;
    public int audioIndex;
    public GameObject visualAidPrefab;
    public float stepDuration;
    public CompletionCondition completionCondition;
}
