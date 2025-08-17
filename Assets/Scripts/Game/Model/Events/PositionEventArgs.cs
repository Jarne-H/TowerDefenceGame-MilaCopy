using MapSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PositionEventArgs : EventArgs
{
    public ICoordinate Position { get; set; }

    public PositionEventArgs(ICoordinate position)
    {
        Position = position;
    }
}
