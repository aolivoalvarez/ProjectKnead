// SCRIPT TAKEN FROM A VIDEO BY SASQUATCH B STUDIOS
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SceneChangeDoorScript))]
public class LabelHandle : Editor
{
    private static GUIStyle labelStyle;

    private void OnEnable()
    {
        labelStyle = new GUIStyle();
        labelStyle.normal.textColor = Color.yellow;
        labelStyle.alignment = TextAnchor.MiddleCenter;
    }

    private void OnSceneGUI()
    {
        SceneChangeDoorScript door = (SceneChangeDoorScript)target;

        Handles.BeginGUI();
        Handles.Label(door.transform.position + new Vector3(0f, 0f, 0f), door.currentDoorPosition.ToString(), labelStyle);
        Handles.EndGUI();
    }
}
