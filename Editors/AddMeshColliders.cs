using UnityEngine;
using UnityEditor;

public class ReplaceBoxWithMeshColliderEditor : EditorWindow
{
    [MenuItem("Tools/Replace BoxColliders With MeshColliders")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ReplaceBoxWithMeshColliderEditor));
    }

    void OnGUI()
    {
        GUILayout.Label("Replace BoxColliders with MeshColliders", EditorStyles.boldLabel);

        if (GUILayout.Button("Replace Colliders"))
        {
            ReplaceColliders();
        }
    }

    private void ReplaceColliders()
    {
        int replacedCount = 0;

        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // Skip hidden/editor-only objects
            if (obj.hideFlags != HideFlags.None) continue;

            // Only consider objects with a MeshFilter
            MeshFilter mf = obj.GetComponent<MeshFilter>();
            if (mf == null) continue;

            // Remove existing BoxCollider if present
            BoxCollider bc = obj.GetComponent<BoxCollider>();
            if (bc != null)
            {
                DestroyImmediate(bc, true);
            }

            // Skip if there’s already a MeshCollider
            if (obj.GetComponent<MeshCollider>() != null) continue;

            // Add MeshCollider
            MeshCollider mc = obj.AddComponent<MeshCollider>();
            mc.convex = false; // For static environment objects
            mc.isTrigger = false;

            replacedCount++;
            Debug.Log("Added MeshCollider to: " + obj.name);
        }

        Debug.Log("Finished! Replaced colliders on " + replacedCount + " objects.");
    }
}
