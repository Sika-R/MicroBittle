using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MG_BlocksEngine2.Utils
{
    public static class BE2_ArrayUtils
    {
        public static void Resize<T>(ref T[] array, int size)
        {
            T[] tempArray = array;
            array = new T[size];
            for (int i = 0; i < tempArray.Length; i++)
            {
                if (size > i)
                    array[i] = tempArray[i];
            }
        }

        public static void Add<T>(ref T[] array, T value)
        {
            int length = array.Length;
            Resize<T>(ref array, length + 1);
            array[length] = value;
        }

        public static T[] AddReturn<T>(T[] array, T value)
        {
            int length = array.Length;
            T[] newArray = array;
            Resize<T>(ref newArray, length + 1);
            newArray[length] = value;
            return newArray;
        }

        public static void Remove<T>(ref T[] array, T value)
        {
            List<T> list = new List<T>();
            list.AddRange(array);
            list.Remove(value);
            array = list.ToArray();
        }

        public static T[] FindAll<T>(ref T[] array, System.Predicate<T> match)
        {
            List<T> list = new List<T>();
            list.AddRange(array);
            return list.FindAll(match).ToArray();
        }

        public static T Find<T>(ref T[] array, System.Predicate<T> match)
        {
            List<T> list = new List<T>();
            list.AddRange(array);
            return list.Find(match);
        }
    }
}