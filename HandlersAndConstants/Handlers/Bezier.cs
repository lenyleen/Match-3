using UnityEngine;

public static class Bezier
{
    /// <summary>
    /// Calculates the movement of an object on Bezier curve
    /// </summary>
    /// <param name="p1">First point of path</param>
    /// <param name="p2">Second point of path</param>
    /// <param name="p3">Third point of path</param>
    /// <param name="p4"> Fourth point of path</param>
    /// <param name="t">Time of moving</param>
    /// <returns>New position of object on bezier curve</returns>
    public static Vector3 GetPoint(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float t)
    {
        var p12 = Vector3.Lerp(p1, p2, t);
        var p23 = Vector3.Lerp(p2, p3, t);
        var p34 = Vector3.Lerp(p3, p4, t);

        var p123 = Vector3.Lerp(p12, p23, t);
        var p234 = Vector3.Lerp(p23, p34, t);

        var p1234 = Vector3.Lerp(p123, p234, t);
        return p1234;
    }

    public static Vector3 GetPoint(Vector2[] positions, float t)
    {
        var p12 = Vector2.Lerp(positions[0], positions[1], t);
        var p23 = Vector3.Lerp(positions[1], positions[2], t);
        var p34 = Vector3.Lerp(positions[2], positions[3], t);
        
        var p123 = Vector3.Lerp(p12, p23, t);
        var p234 = Vector3.Lerp(p23, p34, t);
        
        var p1234 = Vector3.Lerp(p123, p234, t);
        return p1234;
    }
}
