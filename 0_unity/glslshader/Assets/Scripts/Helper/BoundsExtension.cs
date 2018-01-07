using UnityEngine;
using System.Collections;

public static class BoundsExtension
{
    public static bool ContainBounds(this Bounds bounds, Bounds target)
    {
        return bounds.Contains(target.min) && bounds.Contains(target.max);
    }

    public static Vector3 LeakingDirection(this Bounds bounds, Bounds target)
    {
        Vector3 leakingOffset = new Vector3();
        if (!bounds.Contains(target.min))
        {
            if (target.min.x < bounds.min.x)
            {
                leakingOffset.x = bounds.min.x - target.min.x;
            }
            if (target.min.y > bounds.min.y)
            {
                leakingOffset.y = bounds.min.y - target.min.y;
            }
            if (target.min.z < bounds.min.z)
            {
                leakingOffset.z = bounds.min.z - target.min.z;
            }
        }

        if (!bounds.Contains(target.max))
        {
            if (target.max.x > bounds.max.x)
            {
                leakingOffset.x = bounds.max.x - target.max.x;
            }
            if (target.max.y < bounds.max.y)
            {
                leakingOffset.y = bounds.max.y - target.max.y;
            }
            if (target.max.z > bounds.max.z)
            {
                leakingOffset.z = bounds.max.z - target.max.z;
            }
        }
        return leakingOffset;
    }
}
