#if UNITY_EDITOR
using System;
using UnityEditor;

public class DetekrotChange : AssetPostprocessor
{
    public static event Action OnChange;


    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        OnChange?.Invoke();
    }

}
#endif
