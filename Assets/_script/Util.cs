using System.Collections.Generic;
namespace Util
{
    public class ListUtils
    {
        public static List<T> GetDistinctElems<T>(List<T> list)
        {
            List<T> distinctElems = new List<T>();
            foreach (T elem in list)
            {
                if (!distinctElems.Contains(elem))
                {
                    distinctElems.Add(elem);
                }
            }
            return distinctElems;
        }
    }
}