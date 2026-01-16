using NWH.DWP2.WaterData;
using Crest;
using UnityEngine;
using NWH.DWP2.WaterObjects;
using System.Drawing;

public class CrestWaterDataProvider : WaterDataProvider
{
	private Vector3[] _displacements;
	public override bool SupportsWaterFlowQueries()
	{
		return true;
	}

	public override bool SupportsWaterHeightQueries()
	{
		return true;
	}

	public override bool SupportsWaterNormalQueries()
	{
		return false;
	}
	void EnsureArrays(int count)
	{
		if (_displacements != null && _displacements.Length == count)
			return;

		_displacements = new Vector3[count];
	}

	public override void GetWaterHeights(WaterObject waterObject, ref Vector3[] points, ref float[] waterHeights)
	{
		var ocean = OceanRenderer.Instance;
		if (ocean == null || ocean.CollisionProvider == null)
			return;

		var provider = ocean.CollisionProvider;

		EnsureArrays(points.Length);

		var status = provider.Query(
			GetHashCode(),
			1f,
			points,
			_displacements,
			null,
			null
		);

		if (!provider.RetrieveSucceeded(status))
		{
			for (int i = 0; i < waterHeights.Length; i++)
				waterHeights[i] = ocean.SeaLevel;

			return;
		}

		for (int i = 0; i < points.Length; i++)
		{
			waterHeights[i] = ocean.SeaLevel + _displacements[i].y;
		}
	}
	public override float GetWaterHeightSingle(WaterObject waterObject, Vector3 point)
	{
		var ocean = OceanRenderer.Instance;
		if (ocean == null || ocean.CollisionProvider == null)
			return 0f;

		Vector3[] disp = new Vector3[1];
		Vector3[] pts = new[] { point };

		var status = ocean.CollisionProvider.Query(
			GetHashCode(),
			1f,
			pts,
			disp,
			null,
			null
		);

		if (!ocean.CollisionProvider.RetrieveSucceeded(status))
			return ocean.SeaLevel;

		return ocean.SeaLevel + disp[0].y;
	}
}
