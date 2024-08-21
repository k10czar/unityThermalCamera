using UnityEngine;

public class CameraFilterApplication
{
	Material SkyboxMaterialCached = null;
	CameraFilterData currentFilter;

	public void Apply( Camera camera, CameraFilterData cameraFilterData )
	{
		if( currentFilter == cameraFilterData ) return;
		
		var wasNullFilter = currentFilter == null;
		if( wasNullFilter ) SkyboxMaterialCached = RenderSettings.skybox;

		var willBeNullFilter = cameraFilterData == null;
		if( willBeNullFilter )
		{
			RenderSettings.skybox = SkyboxMaterialCached;
			camera.ResetReplacementShader();
		}
		else
		{
			// PostProcessingMaterial = new Material( cameraFilterData.PostProcessingMaterial );
			if( cameraFilterData.SurfaceReplacement != null )
			{
				RenderSettings.skybox = cameraFilterData.SkyboxMaterialReplacement;
				camera.SetReplacementShader(cameraFilterData.SurfaceReplacement, "RenderType");
			}
		}

		currentFilter = cameraFilterData;
	}

	public void OnRenderImage( RenderTexture src, RenderTexture dst ) 
	{
		if( currentFilter == null ) 
		{
			Graphics.Blit(src, dst);
			return;
		}

		currentFilter.Apply();
		Graphics.Blit(src, dst, currentFilter.PostProcessingMaterial);
	}
}
