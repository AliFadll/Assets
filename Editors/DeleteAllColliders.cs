using UnityEngine;
using UnityEditor;

public class DeleteAllCollidersEditor : EditorWindow
{
    [MenuItem("Tools/Delete All Colliders")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(DeleteAllCollidersEditor));
    }

    void OnGUI()
    {
        GUILayout.Label("Delete All Colliders from Scene Objects", EditorStyles.boldLabel);

        if (GUILayout.Button("Delete Colliders"))
        {
            DeleteColliders();
        }
    }

    private void DeleteColliders()
    {
        int deletedCount = 0;
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // Skip hidden/editor-only objects
            if (obj.hideFlags != HideFlags.None) continue;

            // Delete BoxCollider if present
            BoxCollider bc = obj.GetComponent<BoxCollider>();
            if (bc != null)
            {
                DestroyImmediate(bc, true);
                deletedCount++;
            }

            // Delete MeshCollider if present
            MeshCollider mc = obj.GetComponent<MeshCollider>();
            if (mc != null)
            {
                DestroyImmediate(mc, true);
                deletedCount++;
            }
        }

        Debug.Log("Finished! Deleted " + deletedCount + " colliders from the scene.");
    }
}
