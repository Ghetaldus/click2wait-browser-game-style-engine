// (c) www.click2wait.net

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using System.Linq;

public static partial class Extensions
{

	// -----------------------------------------------------------------------------------
	public static string TimeFormat(this float seconds)
	{
	
		if (seconds >= 604800)
			return (seconds/604800).ToString("#,0W");
		
		if (seconds >= 86400)
			return (seconds/86400).ToString("0.#") + "D"; 
		
		if (seconds >= 3600)
			return (seconds/3600).ToString("#,0m");
			
		if (seconds >= 60)
			return (seconds/60).ToString("0.#") + "s";
		
		return seconds.ToString("#,0");
	
	}
	
	// -----------------------------------------------------------------------------------
	public static string KiloFormat(this long num)
    {
        if (num >= 100000000)
            return (num/1000000).ToString("#,0M");

        if (num >= 10000000)
            return (num/1000000).ToString("0.#") + "M";

        if (num >= 100000)
            return (num/1000).ToString("#,0K");

        if (num >= 1000)
            return (num/1000).ToString("0.#") + "K";
		
        return num.ToString(); // "#,0"
    } 
	
	// -----------------------------------------------------------------------------------
	
	// string to int (returns errVal if failed)
    public static int ToInt(this string value, int errVal=0)
    {
        Int32.TryParse(value, out errVal);
        return errVal;
    }

    // string to long (returns errVal if failed)
    public static long ToLong(this string value, long errVal=0)
    {
        Int64.TryParse(value, out errVal);
        return errVal;
    }

	
	// UI SetListener extension that removes previous and then adds new listener
    // (this version is for onClick etc.)
    public static void SetListener(this UnityEvent uEvent, UnityAction call)
    {
        uEvent.RemoveAllListeners();
        uEvent.AddListener(call);
    }

    // UI SetListener extension that removes previous and then adds new listener
    // (this version is for onEndEdit, onValueChanged etc.)
    public static void SetListener<T>(this UnityEvent<T> uEvent, UnityAction<T> call)
    {
        uEvent.RemoveAllListeners();
        uEvent.AddListener(call);
    }
    
	// -----------------------------------------------------------------------------------
    public static int GetDeterministicHashCode(this string value)
    {
        unchecked {

            int hash1 = (5381 << 16) + 5381;
            int hash2 = hash1;

            for (int i = 0; i < value.Length; i += 2)
            {
                hash1 = ((hash1 << 5) + hash1) ^ value[i];
                if (i == value.Length - 1)
                    break;
                hash2 = ((hash2 << 5) + hash2) ^ value[i + 1];
            }

            return hash1 + (hash2 * 1566083941);

        }
    }
	
	// check if a list has duplicates
    // new List<int>(){1, 2, 2, 3}.HasDuplicates() => true
    // new List<int>(){1, 2, 3, 4}.HasDuplicates() => false
    // new List<int>().HasDuplicates() => false
    public static bool HasDuplicates<T>(this List<T> list)
    {
        return list.Count != list.Distinct().Count();
    }

    // find all duplicates in a list
    // note: this is only called once on start, so Linq is fine here!
    public static List<U> FindDuplicates<T, U>(this List<T> list, Func<T, U> keySelector)
    {
        return list.GroupBy(keySelector)
                   .Where(group => group.Count() > 1)
                   .Select(group => group.Key).ToList();
    }
	
}
