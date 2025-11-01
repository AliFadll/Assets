using UnityEngine;
using UnityEditor;

public class AddMissingCollidersEditor : EditorWindow
{
    [MenuItem("Tools/Add Missing Colliders")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(AddMissingCollidersEditor));
    }

    void OnGUI()
    {
        GUILayout.Label("Add Missing Colliders to All Scene Objects", EditorStyles.boldLabel);

        if (GUILayout.Button("Scan and Add Colliders"))
        {
            AddColliders();
        }
    }

    private void AddColliders()
    {
        int addedCount = 0;
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // Skip objects in the Editor only (like Gizmos) and disabled objects if needed
            if (obj.hideFlags != HideFlags.None)
                continue;

            // Skip objects that already have a collider
            if (obj.GetComponent<Collider>() != null)
                continue;

            // Add a BoxCollider by default
            obj.AddComponent<BoxCollider>();
            addedCount++;
            Debug.Log("Added BoxCollider to: " + obj.name);
        }

        Debug.Log("Finished! Added " + addedCount + " colliders.");
    }
}
