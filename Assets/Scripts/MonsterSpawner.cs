using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> _monsterPrefab; // Prefab for the monster
    [SerializeField] private float _spawnInterval = 4.0f; // Time between spawns
    [SerializeField] private float _monsterSpeed = 2.0f; // Speed of the monster
    [SerializeField] private int _monstersAmount;
    [SerializeField] private int _monsterCounter = 0;//for read only
    [SerializeField] private int _waves = 2;
    [SerializeField] private int _currentWave;
    private List<Vector2Int> _path;
    System.Func<Vector2Int, Vector3> _gridToWorldPosition;
    [SerializeField] TMPro.TMP_Text _waveNumberText;
    [SerializeField] TMPro.TMP_Text _waveRestTimeText;
    [SerializeField] int secondsRestTime;
    [SerializeField] int currentRestTime;
    float timer = 1;
    private void Update()
    {
        if (isInBreak)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)//passed a second.
            {
                currentRestTime++;
                _waveRestTimeText.text = $"Rest time: {secondsRestTime - currentRestTime}";
                timer = 1;
                if (currentRestTime >= secondsRestTime)
                {
                    currentRestTime = 0;
                    isInBreak = false;
                    _waveRestTimeText.gameObject.SetActive(false);
                    StartCoroutine(SpawnMonsters());
                    
                }
            }
        }


    }
    public void Init(List<Vector2Int> path, System.Func<Vector2Int, Vector3> gridToWorldPosition)
    {
        //starts at 0, wave 1 is 1.
        _currentWave++;
        _path = path;
        _gridToWorldPosition = gridToWorldPosition;
        isInBreak = true;
        // Start spawning monsters
    }
    bool isInBreak;
    private IEnumerator SpawnMonsters()//REPLACE later
    {
        if(_monstersAmount!=0)
            while (true) // Infinite spawning loop
            {
                if (isInBreak)
                    break;
                SpawnMonster();
                _monsterCounter++;
                if (_monsterCounter >= _monstersAmount)
                {
                    if (_currentWave != _waves)
                    {
                        isInBreak = true;
                        _waveRestTimeText.gameObject.SetActive(true);
                        _currentWave++;
                        _monsterCounter = 0;
                        _waveNumberText.text = "Wave: " + _currentWave;
                    }
                    break;
                }
                yield return new WaitForSeconds(_spawnInterval);
            }
    }

    private void SpawnMonster()
    {
        if (_path.Count == 0) return;

        // Instantiate monster at the start position
        Vector3 spawnPosition = _gridToWorldPosition(_path[0]);
        GameObject monster = Instantiate(_monsterPrefab[_currentWave-1], spawnPosition, Quaternion.identity);
        // Initialize the monster with the path and speed
        monster.GetComponent<MonsterController>().Init(_path, _monsterSpeed, _gridToWorldPosition);
    }
}
