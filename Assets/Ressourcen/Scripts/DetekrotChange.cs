using System;
using UnityEditor;
using UnityEngine;

public class DetekrotChange : AssetPostprocessor
{
    public static event Action OnChange;

#if UNITY_EDITOR
    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        OnChange?.Invoke();
    }
#endif
}
