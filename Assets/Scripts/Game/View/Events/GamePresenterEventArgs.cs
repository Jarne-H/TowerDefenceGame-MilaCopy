using System;
using TowerDefense.Presenter;

public class GamePresenterEventArgs : EventArgs
{
    public GamePresenter GamePresenter { get; }

    public GamePresenterEventArgs(GamePresenter presenter)
    {
        GamePresenter = presenter;
    }
}
