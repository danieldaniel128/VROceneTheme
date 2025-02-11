using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private TileDataSO _containedData;
    public TileDataSO ContainedData { get { return _containedData; } }
    public int X { get; private set; }
    public int Y { get; private set; }
    public Vector2Int TileIndex => new Vector2Int(X, Y);
    public void SetIndex(int x, int y) { X = x ; Y = y; }
    public void SetIndex(Vector2Int tileIndex) { X = tileIndex.x; Y = tileIndex.y; }
    public bool IsOccupied;

    public void ChangeTileData(TileDataSO tileDataSO) => _containedData = tileDataSO;

    public void InitTile(TileDataSO tileDataSO, Vector2Int tileIndex)
    {
        SetIndex(tileIndex);
        ChangeTileData(tileDataSO);
    }

}
