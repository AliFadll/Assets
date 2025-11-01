using UnityEngine;
using UnityEditor;

public class FixCollidersEditor : EditorWindow
{
    [MenuItem("Tools/Fix Colliders")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(FixCollidersEditor));
    }

    void OnGUI()
    {
        GUILayout.Label("Disable 'Is Trigger' on all Colliders", EditorStyles.boldLabel);

        if (GUILayout.Button("Fix All Colliders"))
        {
            DisableTriggerOnAllColliders();
        }
    }

    void DisableTriggerOnAllColliders()
    {
        int fixedCount = 0;
        Collider[] allColliders = FindObjectsOfType<Collider>();

        foreach (Collider col in allColliders)
        {
            if (col.isTrigger)
            {
                col.isTrigger = false;
                fixedCount++;
                Debug.Log("Disabled Is Trigger on: " + col.gameObject.name);
            }
        }

        Debug.Log("Finished! Fixed " + fixedCount + " colliders.");
    }
}
