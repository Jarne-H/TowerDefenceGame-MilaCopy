using MapSystem.Model;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Pathfinding;
using CommandPattern;
using System.Linq;

namespace TowerDefense.Model
{
    public class GameModel : ModelBase
    {
        public MapModel Map { get; }

        public CellModel GoalCell { get; private set; }
        
        public GoalModel Goal { get; private set; }

        public SpawnerModel Spawner { get; private set; }

        public List<EnemyModel> Enemies { get; private set; } = new List<EnemyModel>();

        public List<TowerModel> Towers { get; private set; } = new List<TowerModel>();

        public List<CellModel> WatchedCells { get; private set; } = new List<CellModel>();

        private RecursivePathfinder<CellModel> _pathFinder;
        private List<CellModel> _pathToGoal;

        public EventHandler<TowerModelEventArgs> CreatedTower;

        public EventHandler CreatedGoal;

        //goal attack event
        public EventHandler<GoalModelEventArgs> AttackedGoal;

        public CommandHistory<GameModel> CommandHistory { get; private set; }

        public int TowerDamage { get; set; }
        public int EnemyHealth { get; set; }
        public int EnemyDamage { get; set; }

        public int WantedEnemies = 0;
        private float _towerTimer, _totalTime;
        private int _spawnedEnemies;

        public bool IsGameOver { get; private set; }

        private static List<ITimedCommand<GameModel>> _replayCommands = new();
        public bool IsReplaying => _replayCommands.Any();

        public GameModel(MapModel map, int enemyHealth)
        {
            //Assign map
            Map = map;
            LinkCellClickedEvent();

            var goalTile = map.FindTileOfType(CellType.Goal);
            GoalCell = goalTile;

            //find spawner
            EnemyHealth = enemyHealth; //spawner needs this which is why it is set here
            var spawnTile = map.FindTileOfType(CellType.Spawn);
            Spawner = new SpawnerModel(spawnTile, EnemyHealth, EnemyDamage);
            Spawner.CreatedEnemy += Created_Enemy;

            //find path to goal
            FindPath(map.FindTilesOfType(CellType.Road));

            CommandHistory = new CommandHistory<GameModel>();
        }

        public void Update(float deltaTime)
        {
            _totalTime += deltaTime;

            ReplayCommands(deltaTime);
            RemoveInactiveEnemies();
            SpawnEnemies(deltaTime);
            UpdateTowers(deltaTime);
            UpdateEnemies(deltaTime);            
        }

        private void ReplayCommands(float deltaTime)
        {
            while (IsReplaying && _replayCommands[0].Time <= _totalTime)
            {
                CommandHistory.ExecuteCommand(_replayCommands[0], this);
                _replayCommands.RemoveAt(0);
            }
        }

        private void RemoveInactiveEnemies()
        {
            if (Enemies.Count == 0) return;
            for (int i = Enemies.Count; i > 0; i--)
            {
                if (!Enemies[i-1].IsActive)
                {
                    Enemies.RemoveAt(i-1);
                    if (Enemies.Count == 0)
                    {
                        IsGameOver = true;
                    }
                }
            }
        }

        private void UpdateEnemies(float deltaTime)
        {
            foreach (var enemy in Enemies)
            {
                if (enemy.IsActive && enemy.IsAllowedToMove)
                {
                    enemy.StateMachine.Update(deltaTime);
                }
            }
        }

        private void SpawnEnemies(float deltaTime)
        {
            if (WantedEnemies > 0 && _spawnedEnemies != WantedEnemies)
            {
                _towerTimer += deltaTime;
                if (_towerTimer >= 1)
                {
                    Spawner.CreateEnemyModel(_pathToGoal);
                    _spawnedEnemies++;
                    _towerTimer = 0;
                }
            }
        }

        private void UpdateTowers(float deltaTime)
        {
            foreach (var tower in Towers)
            {
                tower.Update(deltaTime);
            }
        }

        private void Tower_Shot(object sender, EnemyModelEventArgs e)
        {
            e.Enemy.TakeDamage(TowerDamage);
        }

        private void LinkCellClickedEvent()
        {
            foreach (var cell in Map.AllCells)
            {
                cell.WasClicked += Clicked_Cell;
            }
        }

        public void CreateTower(CellModel cell)
        {
            var tower = new TowerModel(cell, _pathToGoal, Enemies);
            tower.TowerShot += Tower_Shot;

            cell.PlacePlaceableObjectModel(tower);
            Towers.Add(tower);
            AddWatchedCells(tower.WatchedCells);

            OnCreatedTower(tower);
        }

        private void AddWatchedCells(List<CellModel> watchedCells)
        {
            foreach (var cell in watchedCells)
            {
                if (!WatchedCells.Contains(cell))
                {
                    WatchedCells.Add(cell);
                }
            }
        }

        public void DestroyTower(CellModel cell)
        {
            if (cell.PlacedObject is TowerModel tower)
            {
                cell.RemovePlacedObjectModel();
                Towers.Remove(tower);
                ClearWatchedCells(tower.WatchedCells);
                tower.DestroyPresenter();
            }
        }

        private void ClearWatchedCells(List<CellModel> watchedCells)
        {
            WatchedCells.RemoveAll(item => watchedCells.Contains(item));
        }

        protected virtual void OnCreatedTower(TowerModel tower)
        {
            CreatedTower?.Invoke(this, new TowerModelEventArgs(tower));
        }

        public void FindPath(IEnumerable<CellModel> roadCells)
        {
            _pathToGoal = new List<CellModel>();
            _pathFinder = new RecursivePathfinder<CellModel>(roadCells);
            _pathToGoal = _pathFinder.FindPath(Spawner.Cell, GoalCell);
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {

        }

        private void Clicked_Cell(object sender, EventArgs e)
        {
            if (sender is CellModel cell)
            {
                if (cell.CellType == CellType.Buildable)
                {
                    if (cell.IsEmpty)
                    {
                        CommandHistory.ExecuteCommand(new CreateTowerCommand(_totalTime, cell), this);
                    }
                    else
                    {
                        CommandHistory.ExecuteCommand(new DestroyTowerCommand(_totalTime, cell), this);
                    }
                }
            }
        }

        public void CreateGoal(int health)
        {
            Goal = new GoalModel(health);
            Goal.GameOver += Game_Over;
            OnCreatedGoal();
        }

        private void Game_Over(object sender, EventArgs e)
        {
            IsGameOver = true;
        }

        protected virtual void OnCreatedGoal()
        {
            CreatedGoal?.Invoke(this, EventArgs.Empty);
        }

        private void Attacked_Goal(object sender, EventArgs e)
        {
            if (sender is EnemyModel enemy)
            {
                Goal.TakeDamage(EnemyDamage);
                if (Goal.Health <= 0)
                {
                    enemy.SelfDestruct();
                    IsGameOver = true;
                }
                else
                {
                    OnAttackedGoal(new GoalModelEventArgs(Goal));
                }
            }
        }

        protected virtual void OnAttackedGoal(GoalModelEventArgs e)
        {
            AttackedGoal?.Invoke(this, e);
        }

        private void Created_Enemy(object sender, EnemyModelEventArgs e)
        {
            Enemies.Add(e.Enemy);
            e.Enemy.AttackGoal += Attacked_Goal;
        }

        public void StoreReplayCommands()
        {
            _replayCommands.Clear();
            _replayCommands.AddRange(CommandHistory.UndoStack.OfType<ITimedCommand<GameModel>>().Reverse());
        }
    }
}
