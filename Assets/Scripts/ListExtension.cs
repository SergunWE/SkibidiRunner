using System.Collections.Generic;
using UnityEngine;

namespace SkibidiRunner
{
    public static class ListExtension
    {
        public static T GetRandomItem<T>(this List<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }
    }
}