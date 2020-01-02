
using System;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AI;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

public partial class Tools
{
    
    private const char CONST_DELIMITER = ';';
	private const int MAX_LENGTH_NAME = 16;
	
	
	// ----------------------------------------------------------------------------------- 
	public static string GetMethodName(Func<IEnumerator> method)
    {
        return method.Method.Name;
    }
	
	// ----------------------------------------------------------------------------------- 
	public static bool IsAllowedName(string _text)
    {
        return _text.Length <= MAX_LENGTH_NAME && Regex.IsMatch(_text, @"^[a-zA-Z0-9_]+$");
    }
    
	// ----------------------------------------------------------------------------------- 
	public static bool IsAllowedPassword(string _text)
    {
        return !String.IsNullOrWhiteSpace(_text);
    }
	
	// ----------------------------------------------------------------------------------- 
    public static string PBKDF2Hash(string text, string salt)
    {
        byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
        Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(text, saltBytes, 10000);
        byte[] hash = pbkdf2.GetBytes(20);
        return BitConverter.ToString(hash).Replace("-", string.Empty);
    }
    
    // -----------------------------------------------------------------------------------
    public static string IntArrayToString(int[] array)
    {
        if (array == null || array.Length == 0) return null;
        string arrayString = "";
        for (int i = 0; i < array.Length; i++)
        {
            arrayString += array[i].ToString();
            if (i < array.Length - 1)
                arrayString += CONST_DELIMITER;
        }
        return arrayString;
    }

    // -----------------------------------------------------------------------------------
    // IntStringToArray
    // -----------------------------------------------------------------------------------
    public static int[] IntStringToArray(string array)
    {
        if (string.IsNullOrWhiteSpace(array)) return null;
        string[] tokens = array.Split(CONST_DELIMITER);
        int[] arrayInt = Array.ConvertAll<string, int>(tokens, int.Parse);
        return arrayInt;
    }

    // -------------------------------------------------------------------------------
    // ArrayContains
    // -------------------------------------------------------------------------------
    public static bool ArrayContains(int[] defines, int define)
    {
        foreach (int def in defines)
        {
            if (def == define)
                return true;
        }
        return false;
    }

    // -------------------------------------------------------------------------------
    // ArrayContains
    // -------------------------------------------------------------------------------
    public static bool ArrayContains(string[] defines, string define)
    {
        foreach (string def in defines)
        {
            if (def == define)
                return true;
        }
        return false;
    }

    // -------------------------------------------------------------------------------
    // RemoveFromArray
    // -------------------------------------------------------------------------------
    public static string[] RemoveFromArray(string[] defines, string define)
    {
        return defines.Where(x => x != define).ToArray();
    }

    // -----------------------------------------------------------------------------------
}
