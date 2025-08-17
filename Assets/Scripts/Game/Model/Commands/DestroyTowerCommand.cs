

using CommandPattern;
using MapSystem.Model;
using TowerDefense.Model;

public class DestroyTowerCommand : ITimedCommand<GameModel>
{
    private readonly CellModel _cell;
    public float Time { get; }

    public DestroyTowerCommand(float time, CellModel cell)
    {
        _cell = cell;
        Time = time;
    }

    public void Execute(GameModel context)
    {
        context.DestroyTower(_cell);
    }

    public void Undo(GameModel context)
    {
        throw new System.NotImplementedException();
    }
}
