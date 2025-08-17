using MapSystem.Model;
using UnityEngine;
using TowerDefense.Presenter;

public class CellPresenter : MonoBehaviour, IClickHandler
{
    [SerializeField]
    private CellType _cellType;

    public CellType CellType => _cellType;

    public CellModel Model { get; private set; }

    public void HandleClick()
    {
        Debug.Log($"{gameObject} Clicked!");
        Model.InformGameOfClick();
    }

    public virtual void SetModel(CellModel model)
    {
        Model = model;        
    }
}
