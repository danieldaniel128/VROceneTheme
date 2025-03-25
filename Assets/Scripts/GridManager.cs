using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.VolumeComponent;

public class GridManager : MonoBehaviour
{
    Dictionary<Vector2Int, Tile> _tiles = new Dictionary<Vector2Int, Tile>();

    [Header("Init Grid Params")]
    [SerializeField] int _width;
    [SerializeField] int _height;

    [Header("Tile Settings")]
    [SerializeField] Tile _tilePrefab;
    [SerializeField] Transform _tileParent;
    [SerializeField] Transform _initPosition;
    [SerializeField] float _tileOffset;
    [SerializeField] List<TileDataSO> _tileDataSOs;
    [Header("Environment Props References")]
    [SerializeField] List<GameObject> _EnvironmentPropsPrefabs;
    public List<Vector2Int> MonsterPath { get; private set; }

    public Action OnMonsterPathGeneratedComplete;

    void Start()
    {
        InitGrid();
    }

    private void InitGrid()
    {
        //generate start tile.
        Vector3 newTileWorldPosition = GetTileIndexWorldPosition(0, 0);
        Tile newTile = Instantiate(_tilePrefab, newTileWorldPosition, Quaternion.identity, _tileParent);
        TileDataSO generatedTileData = _tileDataSOs.FirstOrDefault(c => c.TileType.Equals(TileTypeDictionary.Start_Path_Tile));
        generatedTileData.GeneratePrefab(newTileWorldPosition, newTile.transform);
        newTile.IsOccupied = true;
        //generate end tile
        newTileWorldPosition = GetTileIndexWorldPosition(_width - 1, _height - 1);
        newTile = Instantiate(_tilePrefab, newTileWorldPosition, Quaternion.identity, _tileParent);
        generatedTileData = _tileDataSOs.FirstOrDefault(c => c.TileType.Equals(TileTypeDictionary.End_Path_Tile));
        generatedTileData.GeneratePrefab(newTileWorldPosition, newTile.transform);
        newTile.IsOccupied = true;
        //generate monster path
        MonsterPath = GenerateMonsterPath(new Vector2Int(0, 0), new Vector2Int(_width - 1, _height - 1));


        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                newTileWorldPosition = GetTileIndexWorldPosition(x, y);
                newTile = Instantiate(_tilePrefab, newTileWorldPosition, Quaternion.identity, _tileParent);
                generatedTileData = null;
                Vector2Int currentIndex = new Vector2Int(x, y);

                if (currentIndex == new Vector2Int(0, 0))
                {
                    continue;
                }
                else if (currentIndex == new Vector2Int(_width - 1, _height - 1))
                {
                    continue;
                }
                else if (MonsterPath.Contains(currentIndex))
                {
                    generatedTileData = _tileDataSOs.FirstOrDefault(c => c.TileType.Equals(TileTypeDictionary.Monster_Path_Tile));
                }
                else
                {
                    // Randomly assign Tower or Environment tile
                    generatedTileData = UnityEngine.Random.value < 0.7f
                        ? _tileDataSOs.FirstOrDefault(c => c.TileType.Equals(TileTypeDictionary.Tower_Tile))
                        : _tileDataSOs.FirstOrDefault(c => c.TileType.Equals(TileTypeDictionary.Environment_Tile));
                }

                newTile.InitTile(generatedTileData, currentIndex);
                _tiles.Add(currentIndex, newTile);

                Transform generatedTileTransform = newTile.ContainedData.GeneratePrefab(newTileWorldPosition, newTile.transform);
                if (generatedTileData.TileType.Equals(TileTypeDictionary.Environment_Tile))
                {
                    //generatedTileData.GeneratePrefab(newTileWorldPosition, newTile.transform);
                    GameObject environmentProp = Instantiate(_EnvironmentPropsPrefabs[UnityEngine.Random.Range(0, _EnvironmentPropsPrefabs.Count)], newTileWorldPosition, Quaternion.identity);
                    environmentProp.transform.SetParent(generatedTileTransform);
                    newTile.IsOccupied = true;
                }
            }
        }

        //after grid finished building call the on finish generating monster path event.
        OnMonsterPathGeneratedComplete?.Invoke();
    }

    private List<Vector2Int> GenerateMonsterPath(Vector2Int start, Vector2Int end)
    {
        List<Vector2Int> path = new List<Vector2Int> { start };
        HashSet<Vector2Int> visited = new HashSet<Vector2Int> { start };
        Vector2Int current = start;

        while (current != end)
        {
            // Define possible moves
            List<Vector2Int> possibleMoves = new List<Vector2Int>
                {
                    new Vector2Int(current.x + 1, current.y), // Right
                    new Vector2Int(current.x, current.y + 1), // Up
                    new Vector2Int(current.x - 1, current.y), // Left
                    new Vector2Int(current.x, current.y - 1)  // Down
                };  

            // Filter valid moves (within bounds, not revisited, and ensuring max 2 connections)
            possibleMoves = possibleMoves
                .Where(pos => pos.x >= 0 && pos.x < _width && pos.y >= 0 && pos.y < _height) // Within bounds
                .Where(pos => !visited.Contains(pos)) // Not already visited
                .Where(pos => GetConnectedNeighborsCount(pos, path) < 2) // Max 2 connections
                .ToList();

            if (possibleMoves.Count > 0)
            {
                // Select next move with randomness to ensure path uniqueness
                Vector2Int nextMove = possibleMoves[UnityEngine.Random.Range(0, possibleMoves.Count)];

                path.Add(nextMove);
                visited.Add(nextMove);
                current = nextMove;
            }
            else
            {
                // Dead-end detected; backtrack
                path.RemoveAt(path.Count - 1);

                if (path.Count > 0)
                {
                    current = path.Last();
                }
                else
                {
                    // If no path exists, restart
                    return GenerateMonsterPath(start, end);
                }
            }
        }

        return path;
    }
    private int GetLineLength(List<Vector2Int> path)
    {
        if (path.Count < 2) return 0;

        Vector2Int currentDirection = path[path.Count - 1] - path[path.Count - 2];
        int length = 1;

        // Traverse backward in the path to calculate line length
        for (int i = path.Count - 2; i > 0; i--)
        {
            Vector2Int previousDirection = path[i] - path[i - 1];
            if (previousDirection == currentDirection)
            {
                length++;
            }
            else
            {
                break; // Line breaks when the direction changes
            }
        }

        return length;
    }
    private bool IsCorner(List<Vector2Int> path, Vector2Int nextMove)
    {
        if (path.Count < 2) return false; // A corner requires at least two previous moves

        Vector2Int previous = path[path.Count - 1];
        Vector2Int direction1 = previous - path[path.Count - 2]; // Last move direction
        Vector2Int direction2 = nextMove - previous; // Next move direction

        return direction1 != direction2; // A corner occurs if the direction changes
    }
    // Helper to count how many neighbors of a tile are in the path
    private int GetConnectedNeighborsCount(Vector2Int tile, List<Vector2Int> path)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>
    {
        new Vector2Int(tile.x + 1, tile.y), // Right
        new Vector2Int(tile.x, tile.y + 1), // Up
        new Vector2Int(tile.x - 1, tile.y), // Left
        new Vector2Int(tile.x, tile.y - 1)  // Down
    };

        return neighbors.Count(n => path.Contains(n));
    }




    public Tile GetTileAtIndex(int x, int y)
    {
        if (_tiles.TryGetValue(new Vector2Int(x, y), out Tile tile))
            return tile;
        return null;
    }

    public Tile GetTileAtIndex(Vector2Int index)
    {
        if (_tiles.TryGetValue(index, out Tile tile))
            return tile;
        return null;
    }

    public Vector3 GetTileIndexWorldPosition(int x, int y) =>
        new Vector3(_initPosition.position.x + x * _tileOffset, _initPosition.position.y, _initPosition.position.z + y * _tileOffset);
    public Vector3 GetTileIndexWorldPosition(Vector2Int Index) =>
        new Vector3(_initPosition.position.x + Index.x * _tileOffset, _initPosition.position.y, _initPosition.position.z + Index.y * _tileOffset);

    public Vector3 GetTileWorldPosition(Tile tile) =>
        new Vector3(_initPosition.position.x + tile.X * _tileOffset, _initPosition.position.y, _initPosition.position.z + tile.Y * _tileOffset);
}
