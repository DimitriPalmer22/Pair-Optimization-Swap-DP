using Alchemy.Inspector;
using UnityEngine;
using UnityEngine.Events;

// [CreateAssetMenu(fileName = "LoggerScriptableObject", menuName = "ScriptableObjects/LoggerScriptableObject")]
public class LoggerScriptableObject : ScriptableObject
{
    public UnityEvent<string> onBroadcastLog;
    
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
    
    [Button]
    public void BroadcastLog(string message)
    {
        onBroadcastLog?.Invoke(message);
    }
}