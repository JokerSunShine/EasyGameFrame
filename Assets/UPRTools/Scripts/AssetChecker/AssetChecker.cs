using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using UnityEngine;
using UPRProfiler.Common;
using Newtonsoft.Json;


public class AssetChecker
{
    public static void Start()
    {
        var checkId = ParseCheckId();
        Console.WriteLine("Asset Runtime Check: " + checkId);
        var uploadRows = new List<AssetRuntimeCheckResultRow>();
        int ruleIndex = 0;
        foreach (Type type in GetAllRules())
        {
            BaseRule rule = (BaseRule)System.Activator.CreateInstance(type);
            rule.Check(out List<RuleResult> results);
            foreach (var result in results)
            {
                uploadRows.Add(new AssetRuntimeCheckResultRow
                {
                    AssetPath = result.AssetPath,
                    RuleInstanceId = ruleIndex,
                    RuleName = rule.Name,
                    RuleDescription = rule.Description,
                    Properties = result.Properties,
                });
            }
            ruleIndex++;
        }
        var uploadPayload = ScriptableObject.CreateInstance<AssetRuntimeCheckUploadPayload>();
        uploadPayload.AssetRuntimeCheckResults = uploadRows;
        uploadPayload.AssetRuntimeCheckId = checkId;
        uploadPayload.Properties = new Dictionary<string, string>()
        {
            { "PackageVersion", Utils.PackageVersion }
        };


        string uploadJson = JsonConvert.SerializeObject(uploadPayload);
        var uploadUrl = Utils.UploadHost + "/asset-runtime-check";
        var client = new WebClient();
        client.Headers[HttpRequestHeader.ContentType] = "application/json";
        client.UploadString(uploadUrl, uploadJson);
    }
    
    public abstract class BaseRule
    {
        public abstract string Name { get; }
        public abstract string Description { get; }

        public abstract void Check(out List<RuleResult> results);
    
    }

    private static IEnumerable<Type> GetAllRules()
    {
        return Assembly.GetAssembly(typeof(BaseRule)).GetTypes().Where(type =>
            type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(BaseRule)));
    }

    private static string ParseCheckId()
    {
        int idx;
        var args = System.Environment.GetCommandLineArgs();
        for (idx = 0; idx < args.Length; idx++)
        {
            if (args[idx].ToUpper().Contains("RUNTIMECHECKID"))
            {
                break;
            }
        }

        return idx + 1 < args.Length ? args[idx + 1] : "";
    }
    
    public class AssetRuntimeCheckUploadPayload : ScriptableObject
    {
        public string                           AssetRuntimeCheckId { get; set; }
        public List<AssetRuntimeCheckResultRow> AssetRuntimeCheckResults { get; set; }
        public Dictionary<string, string>       Properties { get; set; }
    }

    [Serializable]
    public class AssetRuntimeCheckResultRow
    {
        public string                     AssetPath;
        public int                        RuleInstanceId;
        public string                     RuleName;
        public string                     RuleDescription;
        public Dictionary<string, string> Properties;
    }

    public struct RuleResult
    {
        public string                     AssetPath;
        public Dictionary<string, string> Properties;
    }
}
