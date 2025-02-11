using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _monsterPrefab; // Prefab for the monster
    [SerializeField] private float _spawnInterval = 4.0f; // Time between spawns
    [SerializeField] private float _monsterSpeed = 2.0f; // Speed of the monster
    [SerializeField] private int _monstersAmount;
    [SerializeField] private int _monsterCounter = 0;//for read only
    private List<Vector2Int> _path;
    System.Func<Vector2Int, Vector3> _gridToWorldPosition;

    public void Init(List<Vector2Int> path, System.Func<Vector2Int, Vector3> gridToWorldPosition)
    {
        _path = path;
        _gridToWorldPosition = gridToWorldPosition;
        // Start spawning monsters
        StartCoroutine(SpawnMonsters());
    }

    private IEnumerator SpawnMonsters()//REPLACE later
    {
        if(_monstersAmount!=0)
            while (true) // Infinite spawning loop
            {
                SpawnMonster();
                _monsterCounter++;
                if(_monsterCounter >= _monstersAmount)
                    break;
                yield return new WaitForSeconds(_spawnInterval);
            }
    }

    private void SpawnMonster()
    {
        if (_path.Count == 0) return;

        // Instantiate monster at the start position
        Vector3 spawnPosition = _gridToWorldPosition(_path[0]);
        GameObject monster = Instantiate(_monsterPrefab, spawnPosition, Quaternion.identity);

        // Initialize the monster with the path and speed
        monster.GetComponent<MonsterController>().Init(_path, _monsterSpeed, _gridToWorldPosition);
    }
}
