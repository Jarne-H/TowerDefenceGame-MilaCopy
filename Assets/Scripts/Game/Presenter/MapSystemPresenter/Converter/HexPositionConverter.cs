
using MapSystem.Model;
using UnityEngine;
public class HexPositionConverter : ICoordinateConverter
{
	public const float TileSize = 1;

	static readonly float _sqr3over3 = Mathf.Sqrt(3f) / 3f;

	public Vector3 ConvertCoordinateToVector3(ICoordinate coordinate)
	{
		//braces = assumes coordinate should be of HexPosition type
		HexPosition hexCoord = (HexPosition)coordinate;

		//multiplied to adjust hex grid
		float z = 3f / 2f * hexCoord.R;
		//dividing to adjust scale
		float x = (hexCoord.Q + z / 3f) / _sqr3over3;
		return new Vector3(x, 0f, z) * TileSize;
	}

	public ICoordinate ConvertVector3ToCoordinate(Vector3 position)
	{
		//position divided by size
		position /= TileSize;

		float q = (_sqr3over3 * position.x - position.z / 3f);
		float r = (2f / 3f * position.z);
		float s = -q - r;

		return Round(q, r, s);
	}

	private static HexPosition Round(float q, float r, float s)
	{
		HexPosition p = new HexPosition(Mathf.RoundToInt(q), Mathf.RoundToInt(r), Mathf.RoundToInt(s));
		float q_diff = Mathf.Abs(p.Q - q);
		float r_diff = Mathf.Abs(p.R - r);
		float s_diff = Mathf.Abs(p.S - s);


		if (q_diff > r_diff && q_diff > s_diff)
		{
			p.Q = -p.R - p.S;

		}
		else if (r_diff > s_diff)
		{
			p.R = -p.Q - p.S;
		}
		else
		{
			p.S = -p.Q - p.R;
		}

		return p;
	}

}