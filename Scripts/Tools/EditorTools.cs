
#if UNITY_EDITOR

using System;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static partial class EditorTools
{
    
    // -----------------------------------------------------------------------------------
    public static void AddScriptingDefine(string define)
    {
        BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
        string definestring = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
        string[] defines = definestring.Split(';');

        if (Tools.ArrayContains(defines, define))
            return;

        PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, (definestring + ";" + define));
    }

    // -----------------------------------------------------------------------------------
    public static void RemoveScriptingDefine(string define)
    {
        BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
        string definestring = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
        string[] defines = definestring.Split(';');

        defines = Tools.RemoveFromArray(defines, define);

        definestring = string.Join(";", defines);

        PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, (definestring));
    }

}

#endif