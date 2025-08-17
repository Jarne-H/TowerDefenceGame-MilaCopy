using System;

namespace TowerDefense.Model
{
    public class GoalModelEventArgs : EventArgs
    {
        public GoalModel GoalModel { get; }

        public GoalModelEventArgs(GoalModel goalModel)
        {
            GoalModel = goalModel;
        }
    }
}
