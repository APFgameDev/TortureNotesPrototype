using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NS_Utilities.NS_CollectionExtensions
{
    public static class Extensions
    {
        static public bool HasValue<T>(this T[] array, T value)
        {
            if (array == null)
                return false;

            for (int i = 0; i < array.Length; i++)
                if (array[i].Equals(value))
                    return true;

            return false;
        }
        static public bool IsInRange<T>(this T[] array, int i)
        {
            if (i >= 0 && i < array.Length)
                return true;
            else
                return false;
        }

        static public bool IsInRange<T>(this List<T> array, int i)
        {
            if (i >= 0 && i < array.Count)
                return true;
            else
                return false;
        }

        static public T Search<T>(this T[] array, Predicate<T> predicate)
        {
            for (int i = 0; i < array.Length; i++)
                if (predicate(array[i]))
                    return array[i];
            return array[0];
        }

        static public T FindBest<T>(this T[] array, Comparison<T> comparison)
        {
            T best = array[0];

            for (int i = 0; i < array.Length; i++)
            {
                best = array[i];

                for (int j = 1; j < array.Length; j++)
                    if(comparison(best, array[j]) < 0)
                        break;
            }

                return best;
        }

        static public int FindBestIndex<T>(this T[] array, Comparison<T> comparison)
        {
            int best = 0;

            for (int i = 0; i < array.Length; i++)
            {
                best = i;

                for (int j = 1; j < array.Length; j++)
                    if (comparison(array[i], array[j]) < 0)
                        break;
            }

            return best;
        }

        static public O[] ConvertArray<I,O>(this I[] input, System.Func<I,O> convertFunc)
        {
            O[] output = new O[input.Length];

            for (int i = 0; i < input.Length; i++)
                output[i] = convertFunc(input[i]);

            return output;
        }

        static public O[] ConvertArray<O>(this System.Array  input, System.Func<object, O> convertFunc)
        {
            O[] output = new O[input.Length];

            for (int i = 0; i < input.Length; i++)
                output[i] = convertFunc(input.GetValue(i));

            return output;
        }

        static public T[] GetRange<T>(this T[] array, int[] indices)
        {
            T[] output = new T[indices.Length];

            for (int i = 0; i < indices.Length; i++)
                if (indices[i] > 0 && indices[i] < array.Length)
                    output[i] = array[indices[i]];
            return output;
        }
    }
}
