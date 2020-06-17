using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSpawner : MonoBehaviour{

    public Tilemap tilemap;
    public TileReference[] objectList;
    List<List<GameObject>> boxes; 

    void Awake (){
        boxes = new List<List<GameObject>>();
        foreach (TileReference reference in objectList){
            List<GameObject> nexBox = new List<GameObject>();
            boxes.Add(nexBox);
        }
    }

    // Start is called before the first frame update
    void Start(){
        Spawn();
    }

    public List<GameObject> GetObjects(string objType){
        for (int i=0; i<objectList.Length; i++){
            if(objType == objectList[i].title)
                return boxes[i];
        }
        return null;
    }

    public void Spawn(){
        foreach (var pos in tilemap.cellBounds.allPositionsWithin){
            //get tile pivot pos
            Vector3Int tilePos = new Vector3Int(pos.x, pos.y, pos.z);
            TileBase tile = tilemap.GetTile(tilePos);
            if(tile != null){
                for (int i=0; i<objectList.Length; i++){
                    for (int j = 0; j < objectList[i].refTiles.Length; j++)
                        if (tile == objectList[i].refTiles[j])
                        {
                            //spawn object on tile center and register it
                            GameObject newObj = Instantiate(objectList[i].objPrefab, tilemap.gameObject.transform);
                            newObj.transform.position = tilePos + tilemap.layoutGrid.cellSize / 2;

                            SpriteRenderer newObjSpriteRenderer = newObj.GetComponent<SpriteRenderer>();
                            if (newObjSpriteRenderer && tile.GetType() == typeof(Tile))
                                newObjSpriteRenderer.sprite = (tile as Tile).sprite;

                            boxes[i].Add(newObj);

                            tilemap.SetTile(tilePos, null); //erase tile pivot
                            break;
                        }
                }
            }
        }
    }

    // Update is called once per frame
    void Update(){
        
    }
}
