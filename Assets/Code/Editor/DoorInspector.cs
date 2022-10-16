using System;
using Code.Environment;
using UnityEngine;
using UnityEditor;

namespace Code.Editor
{
    [CustomEditor(typeof(DoorController))]
    public class DoorInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DoorController door = (DoorController)target;
            
            if(GUILayout.Button("Toggle door"))
            {
                if (!Application.isPlaying) Debug.LogWarning("Animation disabled in editor");
                door.ToggleDoor();
            }
            
            if(GUILayout.Button("Open door"))
            {
                if (!Application.isPlaying) Debug.LogWarning("Animation disabled in editor");
                door.OpenDoor();
            }
            
            if(GUILayout.Button("Close door"))
            {
                if (!Application.isPlaying) Debug.LogWarning("Animation disabled in editor");
                door.CloseDoor();
            }
        }
    }
}