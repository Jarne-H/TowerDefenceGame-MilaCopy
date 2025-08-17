using MapSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(HexMapDefinition))]
public class HexMapEditor : Editor
{
    private CellType _customCellType = CellType.Grass;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override VisualElement CreateInspectorGUI()
    {
        VisualElement myInspector = new VisualElement();

        VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Editor/HexMapInspector.uxml");
        visualTree.CloneTree(myInspector);

        Button generateButton = myInspector.Q<Button>("GenerateButton");
        generateButton.clicked += GenerateMap;



        // Query the EnumField from the UXML
        EnumField customTileTypeField = myInspector.Q<EnumField>("CustomCellType");
        if (customTileTypeField != null)
        {
            customTileTypeField.Init(_customCellType);
            customTileTypeField.value = _customCellType;
            customTileTypeField.RegisterValueChangedCallback((ChangeEvent<Enum> change) =>
            {
                _customCellType = (CellType)change.newValue;
            });
        }

        Button setCustomTilesButton = myInspector.Q<Button>("SetCustomCellTypeButton");
        setCustomTilesButton.clicked += SetCustomTilesButton_clicked;
        Button clearCustomTilesButton = myInspector.Q<Button>("ClearCustomCellsButton");
        clearCustomTilesButton.clicked += ClearCustomTilesButton_clicked;

        return myInspector;
    }

    private void ClearCustomTilesButton_clicked()
    {
        HexMapDefinition definition = target as HexMapDefinition;
        if (definition == null) return;

        serializedObject.FindProperty("_customCells").ClearArray();
        serializedObject.ApplyModifiedProperties();
        GenerateMap();
    }

    private void SetCustomTilesButton_clicked()
    {
        HexMapDefinition definition = target as HexMapDefinition;
        if (definition == null) return;

        List<HexPosition> _selectedPositions = new List<HexPosition>();
        SerializedProperty arrayProperty = serializedObject.FindProperty("_customCells");

        //store selected presenter positions
        foreach (CellPresenter presenter in Selection.gameObjects.Select(g => g.GetComponent<CellPresenter>()))
        {
            if (presenter == null) continue;
            _selectedPositions.Add((HexPosition)definition.GetCoordinateConverter().ConvertVector3ToCoordinate(presenter.transform.localPosition));
        }

        //clear 
        foreach (HexPosition pos in _selectedPositions)
        {
            int matchingIndex = FindCustomCellIndexAtPosition(arrayProperty, pos);
            if (matchingIndex == -1) continue;

            arrayProperty.DeleteArrayElementAtIndex(matchingIndex);
        }

        // Debug log to verify _customCellType before use
        Debug.Log($"Setting custom tiles with CellType: {_customCellType}");

        foreach (HexPosition pos in _selectedPositions)
        {
            int newItemIndex = arrayProperty.arraySize;
            arrayProperty.InsertArrayElementAtIndex(newItemIndex);
            SerializedProperty newItemProperty = arrayProperty.GetArrayElementAtIndex(newItemIndex);

            newItemProperty.boxedValue = new HexCellDefinition() { CellType = _customCellType, HexPosition = pos };
        }
        serializedObject.ApplyModifiedProperties();

        GenerateMap();
    }

    private static int FindCustomCellIndexAtPosition(SerializedProperty arrayProperty, HexPosition position)
    {
        for (int idx = 0; idx < arrayProperty.arraySize; ++idx)
        {
            var customCellDefinition = arrayProperty.GetArrayElementAtIndex(idx).boxedValue as HexCellDefinition;
            if (customCellDefinition == null) continue;

            if (customCellDefinition.HexPosition.Equals(position)) return idx;
        }
        return -1;
    }

    private void GenerateMap()
    {
        HexMapDefinition definition = target as HexMapDefinition;
        if (definition == null) return;
        var prefabsDict = BuildCellPrefabsDictionary(definition);

        //Despawn previous tiles
        for (int idx = definition.transform.childCount - 1; idx >= 0; --idx)
        {
            DestroyImmediate(definition.transform.GetChild(idx).gameObject);
        }

        //Spawn tiles
        Dictionary<ICoordinate, CellType> customCellTypes = definition.GetCustomCellTypes();
        foreach (HexPosition hexCoordinate in definition.GetAllCoordinates())
        {
            //If customCellTypes doesn't contain coordinate, use default celltype
            CellType cellTypeAtCoordinate = customCellTypes.GetValueOrDefault(hexCoordinate, definition.DefaultCellType);
            GameObject prefab = prefabsDict.GetValueOrDefault(cellTypeAtCoordinate);

            if (prefab == null)
            {
                Debug.LogWarning($"No prefab found for cellType {cellTypeAtCoordinate}");
                continue;
            }

            //Spawn the prefab. Sets the parent, position and name of the GameObject
            GameObject spawnedTile = (GameObject)PrefabUtility.InstantiatePrefab(prefab, definition.transform);
            spawnedTile.transform.localPosition = definition.GetCoordinateConverter().ConvertCoordinateToVector3(hexCoordinate);
            spawnedTile.name = $"{prefab.name}-{hexCoordinate}";
        }


    }

    /// <summary>
    /// Creates a dictionary with all the prefab GameObjects that are stored in the MapPresenter component on the same GameObject as the HexMapDefinition
    /// </summary>
    /// <param name="definition"></param>
    /// <returns></returns>
    private static Dictionary<CellType, GameObject> BuildCellPrefabsDictionary(HexMapDefinition definition)
    {
        MapPresenter presenter = definition.GetComponent<MapPresenter>();
        Dictionary<CellType, GameObject> prefabsDict = new Dictionary<CellType, GameObject>();

        SerializedObject presenterObject = new SerializedObject(presenter);
        SerializedProperty prefabsArr = presenterObject.FindProperty("_cellPrefabs");

        //iteration over serialized array in Unity
        for (int idx = 0; idx < prefabsArr.arraySize; ++idx)
        {
            CellPresenter cellPresenterComponent = prefabsArr.GetArrayElementAtIndex(idx).objectReferenceValue as CellPresenter;
            if (cellPresenterComponent == null) continue;
            prefabsDict.Add(cellPresenterComponent.CellType, cellPresenterComponent.gameObject);
        }
        return prefabsDict;
    }
}
