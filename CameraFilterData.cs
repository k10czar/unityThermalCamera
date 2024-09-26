using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(menuName = "Camera Filter")]
public class CameraFilterData : ScriptableObject
{
    [SerializeReference] Material skyboxMaterial;
	[SerializeReference] Material postProcessingMaterial;
	// public Shader PostProcessing;
	// public Shader SurfaceReplacement;
    [SerializeReference] ScriptableRendererFeature[] features;
	[SerializeReference,ExtendedDrawer,Obsolete] public List<ShaderParameter> parameters;

    // [SerializeReference,ReadOnly] Material skyboxMaterial = null;
	// [SerializeReference,ReadOnly] Material postProcessingMaterial = null;

    public bool UseSurfaceReplacement => skyboxMaterial != null;

    public Material SkyboxMaterialReplacement => skyboxMaterial;
    // {
    //     get
    //     {
    //         // if( !createMaterialInstances ) return SurfaceReplacementOriginalMaterial;
	// 		if( skyboxMaterial == null && SurfaceReplacement != null ) skyboxMaterial = new Material(SurfaceReplacement);
	// 		// if( skyboxMaterial == null && SurfaceReplacementOriginalMaterial != null ) skyboxMaterial = UnityEngine.Object.Instantiate(SurfaceReplacementOriginalMaterial);
    //         return skyboxMaterial;
    //     }
    // }

    public Material PostProcessingMaterial => postProcessingMaterial;
    // {
    //     get
    //     {
    //         // if( !createMaterialInstances ) return PostProcessingOriginalMaterial;
    // 		if( postProcessingMaterial == null && PostProcessing != null ) postProcessingMaterial = new Material(PostProcessing);
    // 		// if( postProcessingMaterial == null && PostProcessingOriginalMaterial != null ) postProcessingMaterial = UnityEngine.Object.Instantiate(PostProcessingOriginalMaterial);
    //         return postProcessingMaterial;
    //     }
    // }

    public void Apply( FullScreenPassRendererFeature feature )
    {
		var mat = PostProcessingMaterial;
		// foreach (var param in parameters) param.ApplyOn( mat );
		foreach (var f in features) f.SetActive( true );
        feature.passMaterial = mat;
    }

    public void Disable()
    {
        foreach (var f in features) f.SetActive( false );
    }
}
