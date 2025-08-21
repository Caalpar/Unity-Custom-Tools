using UnityEngine;
using System.Collections.Generic;

public enum ErrorType
{
    StepError,
    GlobalError,
    AreaViolation,
    BoundaryExit
}

public class ErrorManager2 : MonoBehaviour
{
    private Dictionary<ErrorType, int> errorCounts;

    void Start()
    {
        errorCounts = new Dictionary<ErrorType, int>();
        foreach (ErrorType errorType in System.Enum.GetValues(typeof(ErrorType)))
        {
            errorCounts[errorType] = 0;
        }
    }

    public void LogError(ErrorType errorType, string errorMessage)
    {
        errorCounts[errorType]++;
        Debug.Log($"Error logged: {errorMessage}. Total count for {errorType}: {errorCounts[errorType]}");
    }

    public int GetErrorCount(ErrorType errorType)
    {
        return errorCounts[errorType];
    }

    public void ResetErrorCount(ErrorType errorType)
    {
        errorCounts[errorType] = 0;
    }

    public void ResetAllErrors()
    {
        foreach (ErrorType errorType in System.Enum.GetValues(typeof(ErrorType)))
        {
            errorCounts[errorType] = 0;
        }
    }
}
