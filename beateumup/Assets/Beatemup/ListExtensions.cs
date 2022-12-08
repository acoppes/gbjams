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
    
    public static class CollectionExtensions
    {
        public static T GetItemOrLast<T>(this List<T> list, int index)
        {
            if (index >= list.Count)
                index = list.Count - 1;
            
            return list[index];
        }
        
        public static T GetItemOrLast<T>(this T[] array, int index)
        {
            if (index >= array.Length)
                index = array.Length - 1;
            
            return array[index];
        }
    }
}