using System.Collections.Generic;
using UnityEngine;

public class CameraFilter : MonoBehaviour
{
	[SerializeField,ReadOnly] int selectedFilter = -1;
	[SerializeField,InlineProperties] List<CameraFilterData> cameraFilters = new List<CameraFilterData>();
	CameraFilterApplication filterApplication = new CameraFilterApplication();

	void Awake() 
	{
		filterApplication.Init( GetComponent<Camera>() );
	}

	void OnEnable()
	{
		UpdateFilter();
	}

	void OnDisable()
	{
		filterApplication.Apply( null );
	}

	void UpdateFilter()
	{
		CameraFilterData filter = null;
		if( selectedFilter >= 0 && selectedFilter < cameraFilters.Count ) filter = cameraFilters[selectedFilter];
		filterApplication.Apply( filter );
	}

	void OnRenderImage(RenderTexture src, RenderTexture dst) 
	{
		filterApplication.OnRenderImage( src,  dst );
	}

	void Update()
	{
		if( Input.GetKeyDown( KeyCode.Space ) )
		{
			selectedFilter = ( (selectedFilter + 2) % ( cameraFilters.Count + 1 ) ) - 1;
			UpdateFilter();
		}
	}
}
