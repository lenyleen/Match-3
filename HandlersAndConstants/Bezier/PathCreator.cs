using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCreator : MonoBehaviour
{
    [HideInInspector] 
    public Path path;
    /// <summary>
    /// Сreates a path at the position of this GameObject
    /// </summary>
    public void CreatePath(Vector3 bezierAnchorPosition)
    {
        path = new Path(bezierAnchorPosition);
    }

}
