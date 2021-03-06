using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    void OnSceneGUI()
    {
        FieldOfView fov = (FieldOfView)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position,Vector3.up,Vector3.forward, 360, fov.g_ViewRadius);
        Vector3 viewAngleA = fov.DirFromAngle(-fov.g_ViewAngle / 2, false);
        Vector3 viewAngleB = fov.DirFromAngle(fov.g_ViewAngle / 2, false);


        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov.g_ViewRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov.g_ViewRadius);
    }

    
}
