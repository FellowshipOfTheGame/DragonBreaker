using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Tile Reference", menuName = "Tile Reference")]
public class TileReference : ScriptableObject{
    public string title;
    public GameObject objPrefab;
    public TileBase refTile;
}
