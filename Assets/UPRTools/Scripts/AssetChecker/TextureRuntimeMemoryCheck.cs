using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class TextureRuntimeMemoryCheckRule : AssetChecker.BaseRule
{
    public override string Name => "Texture Runtime Memory";
    public override string Description => "Check Runtime Memory of Texture with .tga format";

    public override void Check(out List<AssetChecker.RuleResult> results)
    {
        results = new List<AssetChecker.RuleResult>();
        var files = Directory.GetFiles("Assets", "*.tga", SearchOption.AllDirectories);
        for (int index = 0; index < files.Length; index++)
        {
            var fileName = files[index];
            Texture tex = AssetDatabase.LoadAssetAtPath<Texture>(fileName);
            long runtimeSize = UnityEngine.Profiling.Profiler.GetRuntimeMemorySizeLong(tex);
            results.Add(new AssetChecker.RuleResult{
                AssetPath = fileName, Properties = new Dictionary<string, string>
                {
                    {"RuntimeMemory", EditorUtility.FormatBytes(runtimeSize)}
                }
            });
        }
    }
}
#endif