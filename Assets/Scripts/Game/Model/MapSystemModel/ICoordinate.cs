

namespace MapSystem.Model
{
	public interface ICoordinate
	{
		public bool IsAdjacentTo(ICoordinate other);
	}
}