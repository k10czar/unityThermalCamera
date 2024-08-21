using System.Collections.Generic;
using UnityEngine;

public class CameraFilter : MonoBehaviour
{
	[SerializeField,ReadOnly] int selectedFilter = -1;
	[SerializeField,InlineProperties] List<CameraFilterData> cameraFilters = new List<CameraFilterData>();
	CameraFilterApplication filterApplication = new CameraFilterApplication();

    public int Count => cameraFilters.Count;

    [SerializeField] Camera filterCamera;

	void Awake() 
	{
	}

	void OnEnable()
	{
		UpdateFilter();
	}

	void OnDisable()
	{
		filterApplication.Apply( filterCamera, null );
	}

	public void Remove()
	{
		filterApplication.Apply( filterCamera, null );
	}

	public void Apply( int id )
	{
		if( id < 0 || id >= cameraFilters.Count )
		{
			Remove();
			return;
		}
		var filter = cameraFilters[id];
		filterApplication.Apply( filterCamera, filter );
		Debug.Log( $"Applied filter {filter.TypeNameOrNullColored(Colors.Console.TypeName)} on {filterCamera.TypeNameOrNullColored(Colors.Console.Fields)}" );
	}

	void UpdateFilter()
	{
		CameraFilterData filter = null;
		if( selectedFilter >= 0 && selectedFilter < cameraFilters.Count ) filter = cameraFilters[selectedFilter];
		filterApplication.Apply( filterCamera, filter );
	}

	void OnRenderImage(RenderTexture src, RenderTexture dst) 
	{
		filterApplication.OnRenderImage( src,  dst );
	}

	public int CycleFilter()
	{
		selectedFilter = ( (selectedFilter + 2) % ( cameraFilters.Count + 1 ) ) - 1;
		UpdateFilter();
		return selectedFilter;
	}
}
