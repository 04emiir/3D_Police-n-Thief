using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class GameController : MonoBehaviour {
    bool isTurnThief = true;
    bool isTurnPolice = false;

    public TextMeshProUGUI currentTurn;

    int thiefPositionLetter;
    int thiefPositionNumber;
    private GameObject thief;

    int[] policePositionsLetter = new int[4];
    int[] policePositionsNumber = new int[4];

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
        CurrentTurn();
    }

    public void StartGame() {
        thief = GameObject.FindGameObjectWithTag("Thief");
        UpdatePolicePositions();
        UpdateThiefPosition();
    }

    void Update() {
        CheckAllDiagonal();

  
    }

    public void CurrentTurn() {
        if (isTurnThief) {
            currentTurn.text = "Turno Ladron";
            currentTurn.color = Color.red;
        } else {
            currentTurn.text = "Turno Policias";
            currentTurn.color = Color.blue;
        }

    }

    public void UpdatePolicePositions() {
        int cont = 0;
        foreach (GameObject police in GameObject.FindGameObjectsWithTag("Police")) {
            policePositionsNumber[cont] = (int)police.transform.position.z;
            policePositionsLetter[cont] = (int)police.transform.position.x;
            cont++;
        }
    }

    public void UpdateThiefPosition() {
        thiefPositionNumber = (int)thief.transform.position.z;
        thiefPositionLetter = (int)thief.transform.position.x;
    }

    public bool CheckBlackLetterBlackNumber(int letter, int number) {
        // Can move towards the black numbers zone[z++] and black letters zone [x++])
        return (column + 1 <= 8 && row + 1 <= 8);
    }

    public bool CheckBlackLetterWhiteNumber(int letter, int number) {
        // Can move towards the black number zone [z++] and white letters zone [x--]
        return (column - 1 >= 1 && row - 1 >= 1);
    }

    public bool CheckWhiteLetterWhiteNumber(int letter, int number) {
        // Can move diagonally to the right and backwards?
        return (column + 1 <= 8 && row - 1 >= 1);
    }

    public bool CheckWhiteLetterBlackNumber(int letter, int number) {
        // Can move diagonally to the left and backwards?
        return (column - 1 >= 8 && row + 1 <= 8);
    }

    public void TileEnable(Color color, int letter, int number) {
        var availabletTile = GameObject.Find("Tile" + tiles[row] + column);
        Debug.Log(row);
        Debug.Log(column);
        availabletTile.GetComponent<Outline>().enabled = true;
        availabletTile.GetComponent<Outline>().OutlineColor = Color.red;
    }

    public void CheckAllDiagonal() {
        // z=0 inicio de blancas (abajo)
        // z=8 inicio de negras (arriba)
        int thiefPositionNumber = (int)thief.transform.position.z;

        // x=0 inicio lateral blancas (izquierda)
        // x=8 incio lateral negras (deracha)
        int thiefPositionLetter = (int)thief.transform.position.x;
        if (CanMoveFDiagonalR(thiefPositionRow, thiefPositionColumn)) {
            TileEnable(Color.red, thiefPositionRow, thiefPositionColumn);
        }
        if (CanMoveFDiagonalR(thiefPositionRow, thiefPositionColumn)) {
            TileEnable(Color.red, thiefPositionRow, thiefPositionColumn);
        }
        if (CanMoveFDiagonalR(thiefPositionRow, thiefPositionColumn)) {
            TileEnable(Color.red, thiefPositionRow, thiefPositionColumn);
        }
        if (CanMoveFDiagonalR(thiefPositionRow, thiefPositionColumn)) {
            TileEnable(Color.red, thiefPositionRow, thiefPositionColumn);
        }
    }

    public void MovePiece (GameObject piece, Transform newPosition) {
            piece.transform.position = newPosition.position;

    }

}
