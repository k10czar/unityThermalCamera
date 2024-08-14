﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

public class ThermalCameraScript : MonoBehaviour {

	[Header("Resource References")]

	[Tooltip("ThermalVisionPostProcessing.shader")]
	public Shader TVPostProcessing;
	[Tooltip("ThermalVisionSurfaceReplacement.shader")]
	public Shader TVSurfaceReplacement; // cool replacement material
	[Tooltip("ThermalVisionPalettes.png")]
	public Texture2D paletteTex;

	[Header("Palette")]

	[Tooltip("0 - don't use texture (use 3 colors instead, as specified below)\n1 - use texture palette 1\n2 - use texture palette 2")]
	public int useTexture = 1;
	public Color coolColor;
	public Color midColor;
	public Color warmColor;

	[Header("Parameters")]
	public float environmentTemperature = 0.2f;

	//--------------------------------
	
	Camera cam;

	Material TVPostProcessingMaterial = null;
	Material SkyboxMaterialCached = null;
	Material SkyboxMaterialReplacement = null;

	void Awake() {
		SkyboxMaterialCached = RenderSettings.skybox;
		TVPostProcessingMaterial = new Material(TVPostProcessing);
		SkyboxMaterialReplacement = new Material(TVSurfaceReplacement);

		cam = GetComponent<Camera>();
	}

	void OnEnable()
	{
		List<TemperatureController> TCs = GetAllTemperatureControllers();
		// replace skybox material (since replacement shade doesn't seem to affect it)
		RenderSettings.skybox = SkyboxMaterialReplacement;

		// replace material tags and color for objects with explicit temperature control
		foreach (TemperatureController TC in TCs) {
			Renderer R = TC.gameObject.GetComponent<Renderer>();
			if (R==null) continue;
			TC.cachedMaterialTag = R.material.GetTag("RenderType", false);
			TC.cachedColor = R.material.color;
			TC.gameObject.GetComponent<Renderer>().material.SetOverrideTag("RenderType", "Temperature");
		}

		// everything else
		cam.SetReplacementShader(TVSurfaceReplacement, "RenderType");
	}

	void OnDisable()
	{
		List<TemperatureController> TCs = GetAllTemperatureControllers();
		// restore skybox material
		RenderSettings.skybox = SkyboxMaterialCached;

		// restore temperature-controlled object tags and color
		foreach (TemperatureController TC in TCs) {
			Renderer R = TC.gameObject.GetComponent<Renderer>();
			if (R==null) continue;
			TC.gameObject.GetComponent<Renderer>().material.SetOverrideTag("RenderType", TC.cachedMaterialTag);
			TC.gameObject.GetComponent<Renderer>().material.color = TC.cachedColor;
		}

		// everything else
		cam.ResetReplacementShader();
	}

	void Update() {
		List<TemperatureController> TCs = GetAllTemperatureControllers();
		foreach (TemperatureController TC in TCs) {
			TC.gameObject.GetComponent<Renderer>().material.color = new Color(TC.temperature, 0, 0, 0);
		}
	}

	void OnRenderImage(RenderTexture src, RenderTexture dst) {

		Shader.SetGlobalFloat("_EnvironmentTemperature", environmentTemperature);

		if (enabled) {
			if (useTexture==1 || useTexture==2) {
				TVPostProcessingMaterial.SetInt("useTexture", useTexture);
				TVPostProcessingMaterial.SetTexture("_PaletteTex", paletteTex);

			} else {
				TVPostProcessingMaterial.SetInt("useTexture", 0);
				TVPostProcessingMaterial.SetColor("coolColor", coolColor);
				TVPostProcessingMaterial.SetColor("midColor", midColor);
				TVPostProcessingMaterial.SetColor("warmColor", warmColor);
			}
			Graphics.Blit(src, dst, TVPostProcessingMaterial);

		} else {
			Graphics.Blit(src, dst);
		}
	}
	
	List<TemperatureController> GetAllTemperatureControllers() {
		List<TemperatureController> TCs = new List<TemperatureController>();
		foreach(TemperatureController TC in Resources.FindObjectsOfTypeAll(typeof(TemperatureController)) as TemperatureController[]) {
			if (!EditorUtility.IsPersistent(TC.transform.root.gameObject) && 
					!(TC.hideFlags == HideFlags.NotEditable || TC.hideFlags == HideFlags.HideAndDontSave)) {
				TCs.Add(TC);
			}
		}
		return TCs;
	}

}
