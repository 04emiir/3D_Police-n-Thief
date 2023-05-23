using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    bool isTurnThief = true;
    bool isTurnPolice = false;

    public TextMeshProUGUI currentTurn;

    int thiefPositionLetter;
    int thiefPositionNumber;

    int selectedPolicePositionLetter;
    int selectedPolicePositionNumber;

    private GameObject thief;
    private GameObject selectedPolice;

    int[] policePositionsLetter = new int[4];
    int[] policePositionsNumber = new int[4];

    private Dictionary<int, string> tiles = new Dictionary<int, string>();

    public void StartGame() {
        tiles.Add(1, "A");
        tiles.Add(2, "B");
        tiles.Add(3, "C");
        tiles.Add(4, "D");
        tiles.Add(5, "E");
        tiles.Add(6, "F");
        tiles.Add(7, "G");
        tiles.Add(8, "H");
        CurrentTurnDisplay();
        thief = GameObject.FindGameObjectWithTag("Thief");
        UpdateAllPolicePositions();
        UpdateThiefPosition();
        CheckThiefMovements();
    }

    void Update() {
        ThiefWin();
        PoliceWin();
        if (isTurnThief) {
            if (Input.GetMouseButtonDown(0)) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100)) {
                    if (hit.transform.gameObject.tag == "Tile") {
                        int posX = (int)hit.transform.position.x;
                        int posZ = (int)hit.transform.position.z;
                        var availabletTile = GameObject.Find("Tile" + tiles[posX] + "-" + posZ);
                        if (availabletTile.GetComponent<Outline>().enabled == true) {
                            MovePiece(thief, new Vector3(posX, thief.transform.position.y, posZ));
                            DisableCheckThiefMovement();
                            ChangeTurn();
                            CurrentTurnDisplay();
                        }
                    }


                }
            }
        } else if (isTurnPolice) {
            if (Input.GetMouseButtonDown(0)) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100)) {
                    if (hit.transform.gameObject.tag == "Police") {
                        if(selectedPolice != null)  
                            DisableCheckPoliceMovements();
                        selectedPolice = hit.transform.gameObject;
                        UpdateSelectedPolicePosition();
                        CheckPoliceMovements();
                    } else if (hit.transform.gameObject.tag == "Tile" && selectedPolice != null) {
                        int posX = (int)hit.transform.position.x;
                        int posZ = (int)hit.transform.position.z;
                        var availabletTile = GameObject.Find("Tile" + tiles[posX] + "-" + posZ);
                        if (availabletTile.GetComponent<Outline>().enabled == true) {
                            MovePiece(selectedPolice, new Vector3(posX, selectedPolice.transform.position.y, posZ));
                            DisableCheckPoliceMovements();
                            selectedPolice = null;
                            ChangeTurn();
                            CurrentTurnDisplay();
                        }
                    }


                }
            }
        }


    }

    public void CurrentTurnDisplay() {
        if (isTurnThief) {
            currentTurn.text = "Turno Ladron";
            currentTurn.color = Color.red;
        } else {
            currentTurn.text = "Turno Policias";
            currentTurn.color = Color.blue;
        }

    }

    public void ChangeTurn() {
        isTurnThief = !isTurnThief;
        isTurnPolice = !isTurnPolice;
        UpdateAllPolicePositions();
        UpdateThiefPosition();
        if(isTurnThief)
            CheckThiefMovements();
    }

    public void UpdateAllPolicePositions() {
        int cont = 0;
        foreach (GameObject police in GameObject.FindGameObjectsWithTag("Police")) {
            policePositionsNumber[cont] = (int)police.transform.position.z;
            policePositionsLetter[cont] = (int)police.transform.position.x;
            cont++;
        }
    }

    public void UpdateSelectedPolicePosition() {
        selectedPolicePositionNumber = (int)selectedPolice.transform.position.z;
        selectedPolicePositionLetter = (int)selectedPolice.transform.position.x;
    }

    public void UpdateThiefPosition() {
        thiefPositionNumber = (int)thief.transform.position.z;
        thiefPositionLetter = (int)thief.transform.position.x;
    }

    public bool CheckDiagonalA(int number, int letter) {
        // Can move z++ and x--
        return (number + 1 <= 8 && letter - 1 >= 1);
    }

    public bool CheckPoliceInDiagonal(int number, int letter) {
        for (int i = 0; i < 4; i++) {
            int standingPolicePositionLetter = policePositionsLetter[i];
            int standingPolicePositionNumber = policePositionsNumber[i];
            if(standingPolicePositionLetter == letter && standingPolicePositionNumber == number)
                return true;
        }
        return false;
    }

    public bool CheckThiefInDiagonal(int number, int letter) {
        if (thiefPositionLetter == letter && thiefPositionNumber == number)
            return true;
        else
            return false;
    }



    public bool CheckDiagonalB(int number, int letter) {
        // Can move z++ and x++
        return (number + 1 <= 8 && letter + 1 <= 8);
    }

    public bool CheckDiagonalC(int number, int letter) {
        // Can move z-- and x++
        return (number - 1 >= 1 && letter + 1 <= 8);
    }

    public bool CheckDiagonalD(int number, int letter) {
        // Can move z-- and x--
        return (number - 1 >= 1 && letter - 1 >= 1);

    }

    public void TileEnable(Color color, int number, int letter) {
        var availabletTile = GameObject.Find("Tile" + tiles[letter] + "-" + number);
        if (!availabletTile.GetComponent<Outline>().enabled) { 
            availabletTile.GetComponent<Outline>().enabled = true;
            availabletTile.GetComponent<Outline>().OutlineColor = color;
        }
    }

    public void TileDisable(int number, int letter) {
        var availabletTile = GameObject.Find("Tile" + tiles[letter] + "-" + number);
        availabletTile.GetComponent<Outline>().enabled = false;
    }


    public void CheckThiefMovements() {
        // z=0 inicio de blancas (abajo)
        // z=8 inicio de negras (arriba)

        // x=0 inicio lateral blancas (izquierda)
        // x=8 incio lateral negras (deracha)

        if (CheckDiagonalA(thiefPositionNumber, thiefPositionLetter) && !CheckPoliceInDiagonal(thiefPositionNumber + 1, thiefPositionLetter - 1)) {
            TileEnable(Color.red, thiefPositionNumber+1, thiefPositionLetter-1);
        }
        if (CheckDiagonalB(thiefPositionNumber, thiefPositionLetter) && !CheckPoliceInDiagonal(thiefPositionNumber + 1, thiefPositionLetter + 1)) {
            TileEnable(Color.red, thiefPositionNumber+1, thiefPositionLetter+1);
        }
        if (CheckDiagonalC(thiefPositionNumber, thiefPositionLetter) && !CheckPoliceInDiagonal(thiefPositionNumber - 1, thiefPositionLetter + 1)) {
            TileEnable(Color.red, thiefPositionNumber-1, thiefPositionLetter+1);
        }
        if (CheckDiagonalD(thiefPositionNumber, thiefPositionLetter) && !CheckPoliceInDiagonal(thiefPositionNumber - 1, thiefPositionLetter - 1)) {
            TileEnable(Color.red, thiefPositionNumber-1, thiefPositionLetter-1);
        }
    }

    public void CheckPoliceMovements() {
        // z=0 inicio de blancas (abajo)
        // z=8 inicio de negras (arriba)

        // x=0 inicio lateral blancas (izquierda)
        // x=8 incio lateral negras (deracha)

        if (CheckDiagonalC(selectedPolicePositionNumber, selectedPolicePositionLetter) && !CheckThiefInDiagonal(selectedPolicePositionNumber - 1, selectedPolicePositionLetter + 1)) {
            TileEnable(Color.blue, selectedPolicePositionNumber - 1, selectedPolicePositionLetter + 1);
        }
        if (CheckDiagonalD(selectedPolicePositionNumber, selectedPolicePositionLetter) && !CheckThiefInDiagonal(selectedPolicePositionNumber - 1, selectedPolicePositionLetter - 1)) {
            TileEnable(Color.blue, selectedPolicePositionNumber - 1, selectedPolicePositionLetter - 1);
        }

    }

    public void DisableCheckPoliceMovements() {
        if (CheckDiagonalC(selectedPolicePositionNumber, selectedPolicePositionLetter)) {
            TileDisable(selectedPolicePositionNumber - 1, selectedPolicePositionLetter + 1);
        }
        if (CheckDiagonalD(selectedPolicePositionNumber, selectedPolicePositionLetter)) {
            TileDisable(selectedPolicePositionNumber - 1, selectedPolicePositionLetter - 1);
        }

    }

    public void DisableCheckThiefMovement() {
        if (CheckDiagonalA(thiefPositionNumber, thiefPositionLetter)) {
            TileDisable(thiefPositionNumber + 1, thiefPositionLetter - 1);
        }
        if (CheckDiagonalB(thiefPositionNumber, thiefPositionLetter)) {
            TileDisable(thiefPositionNumber + 1, thiefPositionLetter + 1);
        }
        if (CheckDiagonalC(thiefPositionNumber, thiefPositionLetter)) {
            TileDisable(thiefPositionNumber - 1, thiefPositionLetter + 1);
        }
        if (CheckDiagonalD(thiefPositionNumber, thiefPositionLetter)) {
            TileDisable(thiefPositionNumber - 1, thiefPositionLetter - 1);
        }
    }

    public void MovePiece (GameObject piece, Vector3 newPosition) {
            piece.transform.position = newPosition;

    }

    public void ThiefWin() {
        if(thief.transform.position.z == 8) {
            Debug.Log("el ladron ");
        }

    }

    public void PoliceWin() {
        if ((!CheckDiagonalA(thiefPositionNumber, thiefPositionLetter) || CheckPoliceInDiagonal(thiefPositionNumber + 1, thiefPositionLetter - 1)) &&
            (!CheckDiagonalB(thiefPositionNumber, thiefPositionLetter) || CheckPoliceInDiagonal(thiefPositionNumber + 1, thiefPositionLetter + 1)) &&
            (!CheckDiagonalC(thiefPositionNumber, thiefPositionLetter) || CheckPoliceInDiagonal(thiefPositionNumber - 1, thiefPositionLetter + 1)) &&
            (!CheckDiagonalD(thiefPositionNumber, thiefPositionLetter) || CheckPoliceInDiagonal(thiefPositionNumber - 1, thiefPositionLetter - 1)) ) {
            Debug.Log("el ladron pierde");

        }

    }

}
