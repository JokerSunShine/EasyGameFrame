using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental;
using UnityEngine;

public class AssetExport : AssetsModifiedProcessor
{
    protected override void OnAssetsModified(string[] changedAssets, string[] addedAssets, string[] deletedAssets,
        AssetMoveInfo[] movedAssets)
    {
        Debug.LogError("资源设置变动");
    }
}
