﻿using UnityEngine;
[CreateAssetMenu(fileName = "TileData", menuName = "ScriptableObjects/TileDatas/TileData", order = 1)]
public class TileDataSO : ScriptableObject
{
    public string TileName;
    public string TileType;
    //public float PrefabOffset;
    //public float PrefabRotation;//0 - 360
    public GameObject tilePrefab;
    public Transform GeneratePrefab(Vector3 position, Transform parent)
    {
        return Instantiate(tilePrefab, position, Quaternion.identity,parent).transform;
    }
}
