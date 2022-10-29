using UnityEngine;

namespace Beatemup
{
    public static class TransformExtensions
    {
        public static Transform FindInHierarchy(this Transform t, string name)
        {
            if (t.gameObject.name.Equals(name))
                return t;

            for (var i = 0; i < t.childCount; i++) {
                var c = t.GetChild(i);
                if (c == null)
                    continue;
                var result = c.FindInHierarchy(name);

                if (result != null)
                    return result;
            }

            return null;
        }
    }
}