using Alchemy.Inspector;
using UnityEngine;

// [CreateAssetMenu(fileName = "LoggerScriptableObject", menuName = "ScriptableObjects/LoggerScriptableObject")]
public class LoggerScriptableObject : ScriptableObject
{
    [Button]
    public void Log(string message)
    {
        Debug.Log(message);
    }

    [Button]
    public void LogWarning(string message)
    {
        Debug.LogWarning(message);
    }

    [Button]
    public void LogError(string message)
    {
        Debug.LogError(message);
    }
}