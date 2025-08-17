using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;

namespace MapSystem.Model
{
    public enum HexDirection
    {
        NorthEast,
        NorthWest,
        East,
        West,
        SouthEast,
        SouthWest
    }
    [Serializable]
    public struct HexPosition: ICoordinate
    {
        public int Q;
        public int R;
        public int S;

        public HexPosition(int q, int r, int s)
        {
            Q = q;
            R = r;
            S = s;
        }

        public static HexPosition operator +(HexPosition pos, HexDirection dir)
        {
            switch (dir)
            {
                default:
                case HexDirection.West:
                    pos.Q += 1;
                    pos.R += 0;
                    pos.S += -1;
                    break;
                case HexDirection.East:
                    pos.Q += -1;
                    pos.R += 0;
                    pos.S += 1;
                    break;
                case HexDirection.NorthEast:
                    pos.Q += 0;
                    pos.R += -1;
                    pos.S += 1;
                    break;
                case HexDirection.SouthWest:
                    pos.Q += 0;
                    pos.R += 1;
                    pos.S += -1;
                    break;
                case HexDirection.NorthWest:
                    pos.Q += 1;
                    pos.R += -1;
                    pos.S += 0;
                    break;
                case HexDirection.SouthEast:
                    pos.Q += -1;
                    pos.R += 1;
                    pos.S += 0;
                    break;
            }
            return pos;
        }

		public bool IsAdjacentTo(ICoordinate other)
		{
            if(!(other is HexPosition otherPos)) { return false; }
			if (this.S == otherPos.S)
			{
				return Math.Abs(this.R - otherPos.R) == 1;
			}
			if (this.R == otherPos.R)
			{
				return Math.Abs(this.Q - otherPos.Q) == 1;
			}
			if (this.Q == otherPos.Q)
			{
				return Math.Abs(this.S - otherPos.S) == 1;
			}
			return false;
		}

		public static HexPosition operator -(HexPosition pos, HexDirection dir)
        {
            return pos + FlipDirection(dir);
        }

        public override string ToString()
        {
            return $"(Q: {Q} ,R: {R} ,S: {S} )";
        }

        public static bool operator ==(HexPosition a, HexPosition b)
        {
            return a.Q == b.Q && a.S == b.S && a.R == b.R;
        }

        public static bool operator !=(HexPosition a, HexPosition b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is HexPosition b)) return false;
            return (this == b);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Q, R, S);
        }

		public static HexDirection FlipDirection(HexDirection dir)
		{
			switch (dir)
			{
				default:
				case HexDirection.West:
					return HexDirection.East;
				case HexDirection.East:
					return HexDirection.West;
				case HexDirection.NorthEast:
					return HexDirection.SouthWest;
				case HexDirection.SouthWest:
					return HexDirection.NorthEast;
				case HexDirection.NorthWest:
					return HexDirection.SouthEast;
				case HexDirection.SouthEast:
					return HexDirection.NorthWest;
			}
		}
	}
}
