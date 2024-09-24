using UnityEngine;

public class CameraFilterApplication
{
	Material SkyboxMaterialCached = null;
	CameraFilterData currentFilter;
    private FullScreenPassRendererFeature feature;

    public void Apply( Camera camera, CameraFilterData cameraFilterData )
	{
		if( currentFilter == cameraFilterData ) return;
		
		var hasFilterBefore = currentFilter != null;
		if( hasFilterBefore ) currentFilter.Disable();
		else SkyboxMaterialCached = RenderSettings.skybox;

		var willBeNullFilter = cameraFilterData == null;
		if( willBeNullFilter )
		{
			RenderSettings.skybox = SkyboxMaterialCached;
		}
		else
		{
			if( cameraFilterData.SurfaceReplacement != null )
			{
				RenderSettings.skybox = cameraFilterData.SkyboxMaterialReplacement;
			}
		}

		currentFilter = cameraFilterData;
	}
}
