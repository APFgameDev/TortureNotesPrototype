using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NS_ArrayExtensions
{
    public class DebugTestLogger
    {
        // logs stack to unity debugger
        static public bool TryValue<I, K>(Dictionary<I, K> type, I key, out K value, System.Action onFailed = null)
        {
            if (type == null || key == null)
                Debug.LogWarning("Type or Key is null" + "\n\n");
            bool hasValue = type.TryGetValue(key, out value);

            if (hasValue == false)
                Debug.LogWarning(type.ToString() + " did not contain value " + key.ToString() + "\n\n");

            if (hasValue == false && onFailed != null)
                onFailed();

            return hasValue;
        }

        static public bool TestValue<T>(T value, System.Action onFailed = null)
        {
            bool Test = value != null;

            //test failed log stack 
            if (Test == false)
                Debug.LogWarning("Test Value Was Null" + "\n\n");

            if (Test == false && onFailed != null)
                onFailed();

            return Test;
        }


        static public bool TestArrayValues(object[] value, System.Action onFailed = null)
        {
            bool Test = value != null;

            if (Test == false)
                Debug.LogWarning("Test Value array Was Null" + "\n\n");

            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] == null)
                {
                    Debug.LogWarning(value.ToString() + " Test Value at " + i + " Was Null" + "\n\n");
                    Test = false;
                }
            }

            if (Test == false && onFailed != null)
                onFailed();

            return Test;
        }

        static public bool TestHasKey<K, I>(Dictionary<I, K> type, I key, System.Action onFailed = null)
        {
            if (TestArrayValues(new object[] { type, key }) == false)
                return false;

            bool Test = type.ContainsKey(key);

            //test failed log stack 
            if (Test == false)
                Debug.LogWarning(type.ToString() + " doesnt contain key " + key.ToString() + "\n\n");

            if (Test == false && onFailed != null)
                onFailed();

            return Test;
        }

        static public bool TestIsNewKey<K, I>(Dictionary<I, K> type, I key, System.Action onFailed = null)
        {
            if (TestArrayValues(new object[] { type, key }) == false)
                return false;

            bool Test = type.ContainsKey(key) == false;

            //test failed log stack 
            if (Test == false)
                Debug.LogWarning(type.ToString() + " had key already of value" + key.ToString() + "\n\n");

            if (Test == false && onFailed != null)
                onFailed();

            return Test;
        }

    }
}