using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSPApp
{
    static class Util
    {
        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }

        public static T RandomElement<T>(this IList<T> list, Random generator)
        {
            return list[generator.Next(list.Count)];
        }

        public static List<T> GetRandomElements<T>(this IEnumerable<T> list, int elementsCount)
        {
            return list.OrderBy(arg => Guid.NewGuid()).Take(elementsCount).ToList();
        }

        public static List<T> Shuffled<T>(this List<T> oldList, Random generator)
        {
            List<T> newList = new List<T>(oldList);

            int n = newList.Count;
            while (n > 1)
            {
                n--;
                int k = generator.Next(n + 1);
                T value = newList[k];
                newList[k] = newList[n];
                newList[n] = value;
            }

            return newList;
        }
    }
}
