using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Helper : MonoBehaviour
{
    public static List<T> FindAll<T>()
    {
        List<T> interfaces = new List<T>();
        GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (var rootGameObject in rootGameObjects)
        {
            T[] childrenInterfaces = rootGameObject.GetComponentsInChildren<T>();
            foreach (var childInterface in childrenInterfaces)
            {
                interfaces.Add(childInterface);
            }
        }
        return interfaces;
    }

    public static List<T> Find<T>(GameObject rootGameObject)
    {
        List<T> interfaces = new List<T>();
        T[] childrenInterfaces = rootGameObject.GetComponentsInChildren<T>();
        foreach (var childInterface in childrenInterfaces)
        {
            interfaces.Add(childInterface);
        }

        return interfaces;
    }
}
