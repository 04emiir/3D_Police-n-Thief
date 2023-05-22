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
        tiles.Add(1, "A");
        tiles.Add(2, "B");
        tiles.Add(3, "C");
        tiles.Add(4, "D");
        tiles.Add(5, "E");
        tiles.Add(6, "F");
        tiles.Add(7, "G");
        tiles.Add(8, "H");

        PopulateBoard();
        PlacePolices();
        PlaceThief();
    }  

    private void PopulateBoard() {
        for (var rowNumber = 8; rowNumber > 0; rowNumber--) {
            for (var columnLetter = 8; columnLetter > 0; columnLetter--) {
                if (columnLetter % 2 == 0) {
                    if (rowNumber % 2 == 0)
                        tile = Instantiate(blackFloor, new Vector3(columnLetter, 0f, rowNumber), Quaternion.identity);
                    else
                        tile = Instantiate(whiteFloor, new Vector3(columnLetter, 0f, rowNumber), Quaternion.identity);
                } else {
                    if (rowNumber % 2 == 0)
                        tile = Instantiate(whiteFloor, new Vector3(columnLetter, 0f, rowNumber), Quaternion.identity);
                    else
                        tile = Instantiate(blackFloor, new Vector3(columnLetter, 0f, rowNumber), Quaternion.identity);
                }
                tile.transform.parent = GameObject.Find("Board").transform;
                tile.name = "Tile" + tiles[columnLetter] + "-" + rowNumber;
            }
        }
    }

    private void PlaceThief() {
        Transform thiefSpawn = GameObject.Find("TileD-1").transform;
        Instantiate(thief, new Vector3(thiefSpawn.transform.position.x, thiefSpawn.transform.position.y+thief.transform.localScale.y/2 + 0.5f, thiefSpawn.transform.position.z), Quaternion.identity);
    }
    private void PlacePolices() {
        Transform policeSpawnA = GameObject.Find("TileA-8").transform;
        Transform policeSpawnB = GameObject.Find("TileC-8").transform;
        Transform policeSpawnC = GameObject.Find("TileE-8").transform;
        Transform policeSpawnD = GameObject.Find("TileG-8").transform;
        Instantiate(police, new Vector3(policeSpawnA.transform.position.x, policeSpawnA.transform.position.y + police.transform.localScale.y / 2 + 0.5f, policeSpawnA.transform.position.z), Quaternion.identity);
        Instantiate(police, new Vector3(policeSpawnB.transform.position.x, policeSpawnB.transform.position.y + police.transform.localScale.y / 2 + 0.5f, policeSpawnB.transform.position.z), Quaternion.identity);
        Instantiate(police, new Vector3(policeSpawnC.transform.position.x, policeSpawnC.transform.position.y + police.transform.localScale.y / 2 + 0.5f, policeSpawnC.transform.position.z), Quaternion.identity);
        Instantiate(police, new Vector3(policeSpawnD.transform.position.x, policeSpawnD.transform.position.y + police.transform.localScale.y / 2 + 0.5f, policeSpawnD.transform.position.z), Quaternion.identity);

    }
}
