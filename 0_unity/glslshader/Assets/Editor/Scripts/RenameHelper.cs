using UnityEditor;
using UnityEngine;

class RenameHelper
{
    [MenuItem("Toolbox/Rename based on position")]
    public static void RenameBasedOnPosition()
    {
        GameObject[] objs = Selection.gameObjects;
        foreach (GameObject obj in objs)
        {
            obj.name = string.Format("{0},{1}", Mathf.RoundToInt(obj.GetComponent<Transform>().position.x), Mathf.RoundToInt(obj.GetComponent<Transform>().position.y));
        }
    }
}