using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBoard : MonoBehaviour {
    public GameObject blackFloor;
    public GameObject whiteFloor;
    public GameObject thief;
    public GameObject police;

    private GameObject tile;
    private Dictionary<int, string> tiles = new Dictionary<int, string>();
    void Start() {
        tiles.Add(1, "a");
        tiles.Add(2, "b");
        tiles.Add(3, "c");
        tiles.Add(4, "d");
        tiles.Add(5, "e");
        tiles.Add(6, "f");
        tiles.Add(7, "g");
        tiles.Add(8, "h");

        PopulateBoard();
        PlacePolices();
        PlaceThief();
    }  

    private void PopulateBoard() {
        for (var row = 1; row < 9; row++) {
            for (var column = 1; column < 9; column++) {
                if (column % 2 == 0) {
                    if (row % 2 == 0)
                        tile = Instantiate(blackFloor, new Vector3(row, 0f, column), Quaternion.identity);
                    else
                        tile = Instantiate(whiteFloor, new Vector3(row, 0f, column), Quaternion.identity);
                } else {
                    if (row % 2 == 0)
                        tile = Instantiate(whiteFloor, new Vector3(row, 0f, column), Quaternion.identity);
                    else
                        tile = Instantiate(blackFloor, new Vector3(row, 0f, column), Quaternion.identity);
                }
                tile.transform.parent = GameObject.Find("Board").transform;
                tile.name = "Tile" + row + "-" + tiles[column];
            }
        }
    }

    private void PlaceThief() {
        Transform thiefSpawn = GameObject.Find("Tile1-d").transform;
        Instantiate(thief, new Vector3(thiefSpawn.transform.position.x, thiefSpawn.transform.position.y+thief.transform.localScale.y/2 + 0.5f, thiefSpawn.transform.position.z), Quaternion.identity);
    }
    private void PlacePolices() {
        Transform policeSpawnA = GameObject.Find("Tile8-a").transform;
        Transform policeSpawnB = GameObject.Find("Tile8-c").transform;
        Transform policeSpawnC = GameObject.Find("Tile8-e").transform;
        Transform policeSpawnD = GameObject.Find("Tile8-g").transform;
        Instantiate(police, new Vector3(policeSpawnA.transform.position.x, policeSpawnA.transform.position.y + police.transform.localScale.y / 2 + 0.5f, policeSpawnA.transform.position.z), Quaternion.identity);
        Instantiate(police, new Vector3(policeSpawnB.transform.position.x, policeSpawnB.transform.position.y + police.transform.localScale.y / 2 + 0.5f, policeSpawnB.transform.position.z), Quaternion.identity);
        Instantiate(police, new Vector3(policeSpawnC.transform.position.x, policeSpawnC.transform.position.y + police.transform.localScale.y / 2 + 0.5f, policeSpawnC.transform.position.z), Quaternion.identity);
        Instantiate(police, new Vector3(policeSpawnD.transform.position.x, policeSpawnD.transform.position.y + police.transform.localScale.y / 2 + 0.5f, policeSpawnD.transform.position.z), Quaternion.identity);

    }
}
