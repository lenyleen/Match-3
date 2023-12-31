using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Path
{
    [SerializeField, HideInInspector] 
    private List<Vector2> points;

    public Path(Vector2 centre)
    {
        points = new List<Vector2>
        {
            centre+Vector2.left,
            centre+(Vector2.left+Vector2.up)*.5f,
            centre + (Vector2.right+Vector2.down)*.5f,
            centre + Vector2.right
        };
    }
    /// <summary>
    /// Calling an object by index returns the value in the list by index
    /// </summary>
    /// <param name="i"></param>
    public Vector2 this[int i] => points[i];
    public int NumPoints => points.Count;
    public Vector2[] Points => points.ToArray();
    public int NumSegments => (points.Count - 4) / 3 + 1;
    /// <summary>
    /// Adds new segment by calculating positions of new control points and new anchor point 
    /// </summary>
    public void AddSegment(Vector2 anchorPos)
    {
        points.Add(points[points.Count - 1] * 2 - points[points.Count - 2]);
        points.Add((points[points.Count - 1] + anchorPos) * .5f);
        points.Add(anchorPos);
    }
    /// <summary>
    /// Returns an array of positions of points in the segment starting from point with index
    /// </summary>
    /// <returns></returns>
    public Vector2[] GetPointsInSegment(int i)
    {
        return new Vector2[] { points[i * 3], points[i * 3 + 1], points[i * 3 + 2], points[i * 3 + 3] };
    }
    /// <summary>
    /// Moves selected anchor point and related control points
    /// </summary>
    /// <param name="i">Index of movable point</param>
    /// <param name="pos">New position of selected anchor point</param>
    public void MovePoint(int i, Vector2 pos)
    {
        Vector2 deltaMove = pos - points[i];
        points[i] = pos;

        if (i % 3 == 0)
        {
            if (i + 1 < points.Count)
            {
                points[i + 1] += deltaMove;
            }
            if (i - 1 >= 0)
            {
                points[i - 1] += deltaMove;
            }
        }
        else
        {
            bool nextPointIsAnchor = (i + 1) % 3 == 0;
            int correspondingControlIndex = (nextPointIsAnchor) ? i + 2 : i - 2;
            int anchorIndex = (nextPointIsAnchor) ? i + 1 : i - 1;

            if (correspondingControlIndex >= 0 && correspondingControlIndex < points.Count)
            {
                float dst = (points[anchorIndex] - points[correspondingControlIndex]).magnitude;
                Vector2 dir = (points[anchorIndex] - pos).normalized;
                points[correspondingControlIndex] = points[anchorIndex] - dir * dst;
            }
        }
    }
    /// <summary>
    /// Reassign positions of first and last points depending on collectible position
    /// </summary>
    /// <param name="newStartPosition"> Position of collectible</param>
    /// <param name="newEndPositon"> Position of anchor</param>
    public void ReassignPoints(Vector2 newStartPosition, Vector2 newEndPosition)
    {
        MovePoint(0,newStartPosition);
        MovePoint(3,newEndPosition);
    }
}
