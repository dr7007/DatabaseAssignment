using System.Runtime.CompilerServices;
using UnityEngine;

public class DontDestoryObject : MonoBehaviour
{
    private void Awake()
    {
        
        var obj = FindObjectsByType<DontDestoryObject>(FindObjectsSortMode.None);
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}