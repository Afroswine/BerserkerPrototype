using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListHelper
{
    public static void LogListContents<T>(List<T> list)
    {
        Debug.Log("Logging Contents -----");
        if (list.Count > 0)
            list.ForEach(Print);
        else
            Debug.Log("The list is empty...");
        Debug.Log("----- Logging Completed");
        void Print(T obj) { Debug.Log(obj.ToString()); }
    }

    public static IEnumerator LogListContents<T>(List<T> list, float timeInterval, MonoBehaviour thisMB)
    {
        LogListContents(list);

        yield return new WaitForSeconds(timeInterval);

        thisMB.StartCoroutine(LogListContents(list, timeInterval, thisMB));
    }
}
