
using MapSystem.Model;
using UnityEngine;

public interface ICoordinateConverter
{
	Vector3 ConvertCoordinateToVector3(ICoordinate coordinate);
	ICoordinate ConvertVector3ToCoordinate(Vector3 position);
}
