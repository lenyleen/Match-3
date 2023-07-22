using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PathCreator))]
public class PathEditor : Editor
{
    PathCreator creator;
    Path path;
    private RaycastHit2D hit;
    /// <summary>
    /// Implemented method from Unity.Editor
    /// </summary>
    void OnSceneGUI()
    {
        Input();
        Draw();
    } 
    /// <summary>
    /// Detects mouse input and creates a new point
    /// </summary>
    void Input()
    {
        Event guiEvent = Event.current;
        Vector2 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;
        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift)
        {
            Undo.RecordObject(creator, "Add segment");
            path.AddSegment(mousePos);
        }

        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.alt)
        {
            hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if(hit.transform == null) return;
            Undo.RecordObject(creator, "Reassign segment");
            path.ReassignPoints(hit.transform.position,path.Points[3]);
        }
    }
    /// <summary>
    /// Draws lines between anchor point and control points, and Bezier curves between anchor points
    /// </summary>
    void Draw()
    {

        for (int i = 0; i < path.NumSegments; i++)
        {
            Vector2[] points = path.GetPointsInSegment(i);
            Handles.color = Color.black;
            Handles.DrawLine(points[1], points[0]);
            Handles.DrawLine(points[2], points[3]);
            Handles.DrawBezier(points[0], points[3], points[1], points[2], Color.blue, null, 4);
        }

        Handles.color = Color.red;
        for (int i = 0; i < path.NumPoints; i++)
        {
            var fmh_59_62_638074040650867655 = Quaternion.identity; Vector2 newPos = Handles.FreeMoveHandle(path[i], .3f, Vector2.zero, Handles.CylinderHandleCap);
            if (path[i] != newPos)
            {
                Undo.RecordObject(creator, "Move point");
                path.MovePoint(i, newPos);
            }
        }
    }
    void OnEnable()
    {
        creator = (PathCreator)target;
        if (creator.path == null)
        {
            creator.CreatePath(Vector3.zero);
        }
        path = creator.path;
    }
}
