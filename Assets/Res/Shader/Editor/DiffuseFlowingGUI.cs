//using System;
//using UnityEditor;
//using UnityEditor.Rendering.Universal;
//using UnityEngine;
//using UnityEngine.Rendering;

//public class DiffuseFlowingGUI : ShaderGUI {
//	MaterialEditor editor;
//	MaterialProperty[] properties;
//	Material material;
//	public override void OnGUI (MaterialEditor materialEditor, MaterialProperty[] properties) {
//		this.editor = materialEditor;
//		this.properties = properties;
//		this.material = editor.target as Material;
//		// base.OnGUI (materialEditor, properties);
//		//foreach (var p in properties) {
//		//	Debug.Log(p.name);

//		//}
//		var mainTex = FindProperty ("_MainTex", this.properties);
//		editor.ShaderProperty (mainTex, "MainTex");

//		var flowSpeed = FindProperty ("_FlowSpeed", this.properties);
//		editor.ShaderProperty (flowSpeed, flowSpeed.name);
//		var lightIntensity = FindProperty ("_LightIntensity", this.properties);
//		editor.ShaderProperty (lightIntensity, lightIntensity.name);
//		var lightIntensity2 = FindProperty ("_LightIntensity2", this.properties);
//		editor.ShaderProperty (lightIntensity2, lightIntensity2.name);
//		var lightIntensityDuration = FindProperty ("_LightIntensityDuration", this.properties);
//		editor.ShaderProperty (lightIntensityDuration, lightIntensityDuration.name);
//		var lightThreshold = FindProperty ("_LightThreshold", this.properties);
//		editor.ShaderProperty (lightThreshold, lightThreshold.name);
//		var specular = FindProperty ("_Specular", this.properties);
//		editor.ShaderProperty (specular, specular.name);
//		var specularPower = FindProperty ("_SpecularPower", this.properties);
//		editor.ShaderProperty (specularPower, specularPower.name);
//		var color1 = FindProperty ("_Color1", this.properties);

//		bool idFlowing1 = material.IsKeywordEnabled ("_FLOWING1");
//		bool idFlowing2 = material.IsKeywordEnabled ("_FLOWING2");
//		bool idFlowing3 = material.IsKeywordEnabled ("_FLOWING3");
//		// bool adjustUV = material.IsKeywordEnabled ("AdjustedUV");

//		idFlowing1 = EditorGUILayout.Toggle ("_FLOWING1", idFlowing1);
//		if (idFlowing1) {
//			material.EnableKeyword ("_FLOWING1");
//			var color2 = FindProperty ("_Color2", this.properties);
//			var duration = FindProperty ("_Duration", this.properties);
//			editor.ShaderProperty (color1, color1.name);
//			editor.ShaderProperty (color2, color2.name);
//			editor.ShaderProperty (duration, duration.name);
//		} else {
//			editor.ShaderProperty (color1, "TintColor");
//			material.DisableKeyword ("_FLOWING1");
//		}

//		idFlowing2 = EditorGUILayout.Toggle ("_FLOWING2", idFlowing2);
//		if (idFlowing2) {
//			material.EnableKeyword ("_FLOWING2");
//			var flowTex = FindProperty ("_FlowTex", this.properties);
//			var flowColor1 = FindProperty ("_FlowColor1", this.properties);
//			var flowColor2 = FindProperty ("_FlowColor2", this.properties);
//			var flowIntensity = FindProperty ("_FlowIntensity", this.properties);
//			var flowDuration = FindProperty ("_FlowDuration", this.properties);
//			var flowPower = FindProperty ("_FlowPower", this.properties);
//			editor.ShaderProperty (flowTex, flowTex.name);
//			editor.ShaderProperty (flowColor1, flowColor1.name);
//			editor.ShaderProperty (flowColor2, flowColor2.name);
//			editor.ShaderProperty (flowIntensity, flowIntensity.name);
//			editor.ShaderProperty (flowDuration, flowDuration.name);
//			editor.ShaderProperty (flowPower, flowPower.name);
//		} else {
//			material.DisableKeyword ("_FLOWING2");
//		}

//		idFlowing3 = EditorGUILayout.Toggle ("_FLOWING3", idFlowing3);
//		if (idFlowing3) {
//			material.EnableKeyword ("_FLOWING3");
//			// adjustUV = EditorGUILayout.Toggle ("使用世界uv坐标", adjustUV);
//			// if (adjustUV) material.EnableKeyword ("AdjustedUV");
//			// else material.DisableKeyword ("AdjustedUV");
//			var flowTex2 = FindProperty ("_FlowTex2", this.properties);
//			var flowTex2Color = FindProperty ("_FlowTex2Color", this.properties);
//			var flowIntensity2 = FindProperty ("_FlowIntensity2", this.properties);
//			var _FlowPower2 = FindProperty ("_FlowPower2", this.properties);
//			editor.ShaderProperty (flowTex2, flowTex2.name);
//			editor.ShaderProperty (flowTex2Color, flowTex2Color.name);
//			editor.ShaderProperty (flowIntensity2, flowIntensity2.name);
//			editor.ShaderProperty (_FlowPower2, _FlowPower2.name);
//		} else {
//			material.DisableKeyword ("_FLOWING3");
//		}

//		for (int i = properties.Length - 3; i < properties.Length; i++) {
//			editor.ShaderProperty (properties[i], properties[i].name);
//		}
//	}
//}