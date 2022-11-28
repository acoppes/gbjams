using System.Collections.Generic;

namespace Beatemup
{
    public static class ListExtensions
    {
        public static T Random<T>(this List<T> list)
        {
            if (list.Count == 0)
                return default;
            return list[UnityEngine.Random.Range(0, list.Count)];
        }
    }
}