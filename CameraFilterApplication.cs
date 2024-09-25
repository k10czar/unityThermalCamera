using UnityEngine;

public class CameraFilterApplication
{
	Material SkyboxMaterialCached = null;
	CameraFilterData currentFilter;

    public void Apply( CameraFilterData cameraFilterData, FullScreenPassRendererFeature feature )
	{
		if( currentFilter == cameraFilterData ) return;
		
		var hasFilterBefore = currentFilter != null;
		if( hasFilterBefore ) currentFilter.Disable();
		else SkyboxMaterialCached = RenderSettings.skybox;

		currentFilter = cameraFilterData;
		var willBeNullFilter = currentFilter == null;
		if( willBeNullFilter )
		{
			RenderSettings.skybox = SkyboxMaterialCached;
		}
		else
		{
			if( currentFilter.SurfaceReplacement != null )
			{
				RenderSettings.skybox = currentFilter.SkyboxMaterialReplacement;
			}
			currentFilter.Apply( feature );
		}

	}
}
