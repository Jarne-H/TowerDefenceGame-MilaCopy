using TowerDefense.Presenter;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChangeColor : MonoBehaviour
{
    private Renderer _renderer;
    private Material _startMaterial;
    [SerializeField] private Material _hoverMaterial;
    private GamePresenter _gamePresenter;

    void Start()
    {
        _renderer = GetComponentInChildren<Renderer>();
        _startMaterial = _renderer.material;
        _gamePresenter = GameObject.FindAnyObjectByType<GamePresenter>();
    }

    private void OnMouseEnter()
    {
        if (_gamePresenter.Model.IsReplaying) return;
        _renderer.material = _hoverMaterial;
    }

    private void OnMouseExit()
    {
        if (_gamePresenter.Model.IsReplaying) return;
        _renderer.material = _startMaterial;
    }
}
