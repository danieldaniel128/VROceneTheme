using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private MonsterSpawner _monsterSpawner;
    [SerializeField] private GridManager _gridManager;
    [SerializeField] private TowerSelectionHandler _towerSelectionHandler;
    [SerializeField] private TowerBuilder _towerBuilder;
    private void Awake()
    {
        // Generate the monster path
        _gridManager.OnMonsterPathGeneratedComplete += InitMonsterSpawenr;
        _towerSelectionHandler.OnTowerSelectionSucceed += _towerBuilder.SetSelectTower;
        // Initialize the spawner with the path
    }
    private void OnDestroy()
    {
        _gridManager.OnMonsterPathGeneratedComplete -= InitMonsterSpawenr;
    }
    private void OnApplicationQuit()
    {
        _gridManager.OnMonsterPathGeneratedComplete -= InitMonsterSpawenr;
    }
    void InitMonsterSpawenr()
    {
        _monsterSpawner.Init(_gridManager.MonsterPath, _gridManager.GetTileIndexWorldPosition);
    }
}