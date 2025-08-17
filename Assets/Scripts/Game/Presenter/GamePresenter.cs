using UnityEngine;
using TowerDefense.Model;
using System;
using UnityEngine.SceneManagement;

namespace TowerDefense.Presenter
{
    public class GamePresenter : MonoBehaviour
    {
        private HexPositionConverter _hexConverter = new HexPositionConverter();

        [SerializeField]
        private MapPresenter _map;

        public GameModel Model { get; private set; }

        private SpawnerPresenter _spawner;

        [SerializeField]
        private GameObject _enemyPrefab, _towerPrefab, _goalPrefab;

        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private Material _hoveredMaterial;

        [SerializeField]
        private int _enemyHealth, _towerDamage, _wantedEnemyAmount, _goalHealth, _enemyDamage;

        public bool IsGameOver => Model.IsGameOver;

        void Start()
        {
            Model = new GameModel(_map.Model, _enemyHealth) 
            {
                //set values
                TowerDamage = _towerDamage,
                EnemyDamage = _enemyDamage,
                WantedEnemies = _wantedEnemyAmount
            };

            //subscribe to events
            Model.CreatedTower += Created_Tower;
            Model.CreatedGoal += Created_Goal;

            //create spawner
            _spawner = gameObject.AddComponent<SpawnerPresenter>();
            _spawner.Model = Model.Spawner;
            _spawner.EnemyPrefab = _enemyPrefab;

            //create entities
            Model.CreateGoal(_goalHealth);
        }        

        void Update()
        {
            if (!Model.IsReplaying) GetInput();

            Model.Update(Time.deltaTime);
        }

        private void GetInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hitInfo, _camera.farClipPlane))
                {
                    var clicked = hitInfo.collider?.gameObject;

                    var clickedTransform = clicked.transform;

                    var targetObject = CheckForParent(clickedTransform);

                    if (targetObject.TryGetComponent<IClickHandler>(out IClickHandler click))
                    {
                        click.HandleClick();
                    }
                }
            }
        }

        private GameObject CheckForParent(Transform clickedTransform)
        {
            //Making it so clicking on an object placed on top of a tile returns the tile instead
            //Checking if there is a parent and the parent isn't Map (cells are parented to map)
            //This is a pretty bad system I think but we'll find out soon enough

            Transform parentTransform = clickedTransform.parent;
            Transform grandParentTransform = parentTransform.parent; //gotta go 2 levels up because of my prefab :(

            GameObject targetObject =
                (grandParentTransform != null && grandParentTransform.name != "Map")
                ? grandParentTransform.gameObject
                : clickedTransform.gameObject;

            return targetObject;
        }

        private void Created_Tower(object sender, TowerModelEventArgs e)
        {
            MakeTowerPresenter(e.TowerModel);
        }

        private void MakeTowerPresenter(TowerModel tower)
        {
            //convert position
            var cellBelowPos = tower.CellBelow.Position;
            Vector3 newTowerPos = _hexConverter.ConvertCoordinateToVector3(cellBelowPos);

            //create tower
            var newTower = Instantiate(_towerPrefab, newTowerPos, Quaternion.identity);

            //link to model
            var newTowerPresenter = newTower.GetComponent<TowerPresenter>();
            newTowerPresenter.Model = tower;

            ParentToCellBelow(newTower);
        }

        private void ParentToCellBelow(GameObject tower)
        {
            var towerPresenter = tower.GetComponent<TowerPresenter>();
            var cellBelow = towerPresenter.Model.CellBelow;
            var towerPos = cellBelow.Position;

            //icky but there's no other way to get the tile gameObject
            var name = $"{cellBelow.CellType}Tile-{cellBelow.Position}";
            var cellTile = GameObject.Find(name);

            tower.transform.SetParent(cellTile.transform);
        }

        private void Created_Goal(object sender, EventArgs e)
        {
            //convert position
            var goalPos = _hexConverter.ConvertCoordinateToVector3(Model.GoalCell.Position);

            //create goal
            var goalObj = Instantiate(_goalPrefab, goalPos, Quaternion.identity);

            //link to model
            goalObj.GetComponent<GoalPresenter>().Model = Model.Goal;
        }

        public void StartReplay()
        {
            Model.StoreReplayCommands();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}


