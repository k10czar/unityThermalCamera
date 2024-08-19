using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Camera Filter")]
public class CameraFilterData : ScriptableObject
{
	public Shader PostProcessing;
	public Shader SurfaceReplacement;
	[SerializeReference,ExtendedDrawer] public List<ShaderParameter> parameters;

    private Material skyboxMaterial = null;
	private Material postProcessingMaterial = null;
    public Material SkyboxMaterialReplacement
    {
        get
        {
			if( skyboxMaterial == null ) skyboxMaterial = new Material(SurfaceReplacement);
            return skyboxMaterial;
        }
    }

    public Material PostProcessingMaterial
    {
        get
        {
			if( postProcessingMaterial == null ) postProcessingMaterial = new Material(PostProcessing);
            return postProcessingMaterial;
        }
    }

    public void Apply()
    {
		var mat = PostProcessingMaterial;
		foreach (var param in parameters) param.ApplyOn( mat );
    }
}
