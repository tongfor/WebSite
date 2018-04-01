using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class ListHelper
    {
        public static List<T> Shuffle<T>(this List<T> oldList)
        {
            Random random = new Random();
            List<T> newList = new List<T>();
            foreach (T item in oldList)
            {
                newList.Insert(random.Next(newList.Count), item);
            }
            return newList;
        }
    }
}
