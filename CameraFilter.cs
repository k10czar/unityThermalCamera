using System.Collections.Generic;
using UnityEngine;

public class CameraFilter : MonoBehaviour
{
    [SerializeField] FullScreenPassRendererFeature feature;
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
		filterApplication.Apply( null, feature );
		feature.passMaterial = null;
		feature.SetActive( false );
	}

	public void Remove()
	{
		filterApplication.Apply( null, feature );
		feature.passMaterial = null;
		feature.SetActive( false );
	}

	public void Apply( int id )
	{
		if( id < 0 || id >= cameraFilters.Count )
		{
			Remove();
			return;
		}
		var filter = cameraFilters[id];
		feature.passMaterial = filter.PostProcessingMaterial;
		filterApplication.Apply( filter, feature );
		feature.SetActive( true );
		Debug.Log( $"Applied filter {filter.NameAndTypeColored(Colors.Console.TypeName)} on {filterCamera.TypeNameOrNullColored(Colors.Console.Fields)}" );
	}

	void UpdateFilter()
	{
		CameraFilterData filter = null;
		if( selectedFilter >= 0 && selectedFilter < cameraFilters.Count ) filter = cameraFilters[selectedFilter];
		filterApplication.Apply( filter, feature );
		if( feature != null ) feature.SetActive( true );
	}

	// void OnRenderImage(RenderTexture src, RenderTexture dst) 
	// {
	// 	filterApplication.OnRenderImage( src,  dst );
	// }

	public int CycleFilter()
	{
		selectedFilter = ( (selectedFilter + 2) % ( cameraFilters.Count + 1 ) ) - 1;
		UpdateFilter();
		return selectedFilter;
	}
}
