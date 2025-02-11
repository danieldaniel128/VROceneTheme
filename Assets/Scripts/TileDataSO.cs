using UnityEngine;
[CreateAssetMenu(fileName = "TileData", menuName = "ScriptableObjects/TileDatas/TileData", order = 1)]
public class TileDataSO : ScriptableObject
{
    public string TileName;
    public string TileType;
    //public float PrefabOffset;
    //public float PrefabRotation;//0 - 360
    public GameObject tilePrefab;
    public void GeneratePrefab(Vector3 position, Transform parent)
    {
        Instantiate(tilePrefab, position, Quaternion.identity,parent);
    }
}
