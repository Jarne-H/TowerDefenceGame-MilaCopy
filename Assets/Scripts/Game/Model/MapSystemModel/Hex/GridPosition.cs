using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapSystem.Model 
{
	public enum GridDirection
	{
		North,
		West,
		South,
		East
	}



	[Serializable]
	public struct GridPosition: ICoordinate
	{

		public int Row;
		public int Column;

		public GridPosition( int column, int row)
		{
			Row = row;
			Column = column;
		}

		public static GridPosition operator +(GridPosition pos, GridDirection dir)
		{
			switch (dir)
			{
				default:
				case GridDirection.West:
					pos.Column -= 1;
					break;
				case GridDirection.East:
					pos.Column += 1;
					break;
				case GridDirection.South:
					pos.Row += 1;
					break;
				case GridDirection.North:
					pos.Column -= 1;
					break;
				
			}
			return pos;
		}
		public bool IsAdjacentTo(ICoordinate other)
		{
			if (!(other is GridPosition otherPos)) { return false; }
			if (this.Column == otherPos.Column)
			{
				return Math.Abs(this.Row - otherPos.Row) == 1;
			}
			if (this.Row == otherPos.Row)
			{
				return Math.Abs(this.Column - otherPos.Column) == 1;
			}
		
			return false;
		}

		public static GridPosition operator -(GridPosition pos, GridDirection dir)
		{
			return pos + FlipDirection(dir);
		}

		public override string ToString()
		{
			return $"(Column: {Column} ,Row: {Row})";
		}

		public static bool operator ==(GridPosition a, GridPosition b)
		{
			return a.Row == b.Row && a.Column == b.Column;
		}
		public static bool operator !=(GridPosition a, GridPosition b)
		{
			return !(a == b);
		}

		public override bool Equals(object obj)
		{
			if (!(obj is GridPosition b)) return false;
			return (this == b);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Column, Row);
		}

		public static GridDirection FlipDirection(GridDirection dir)
		{
			switch (dir)
			{
				default:
				case GridDirection.West:
					return GridDirection.East;
				case GridDirection.East:
					return GridDirection.West;
				case GridDirection.North:
					return GridDirection.South;
				case GridDirection.South:
					return GridDirection.North;
			}
		}
	}
}

