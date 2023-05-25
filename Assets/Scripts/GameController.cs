using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class GameController : MonoBehaviour {
    bool isTurnThief = true;
    bool isTurnPolice = false;

    public TextMeshProUGUI currentTurn;

    int thiefPositionLetter;
    int thiefPositionNumber;

    int selectedPolicePositionLetter;
    int selectedPolicePositionNumber;

    int raycastPosX;
    int raycastPosZ;

    private GameObject thief;
    private GameObject selectedPolice;

    private SceneLoader scene;


    int[] policePositionsLetter = new int[4];
    int[] policePositionsNumber = new int[4];

    bool objectMoving;

    private Dictionary<int, string> tiles = new Dictionary<int, string>();

    public void StartGame() {
        objectMoving = false;
        scene = this.gameObject.GetComponent<SceneLoader>();
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

        if (isTurnThief && !objectMoving) {
            if (Input.GetMouseButtonDown(0)) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                LayerMask mask = LayerMask.GetMask("GetRaycast");
                if (Physics.Raycast(ray, out hit, 100, mask)) {
                    if (hit.transform.gameObject.tag == "Tile") {
                        raycastPosX = (int)hit.transform.position.x;
                        raycastPosZ = (int)hit.transform.position.z;
                        var availabletTile = GameObject.Find("Tile" + tiles[raycastPosX] + "-" + raycastPosZ);
                        if (availabletTile.GetComponent<Outline>().enabled == true) {
                            objectMoving = true;
                        }
                    }


                }
            }
        } else if (isTurnPolice && !objectMoving) {
            if (Input.GetMouseButtonDown(0)) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                LayerMask mask = LayerMask.GetMask("GetRaycast");
                if (Physics.Raycast(ray, out hit, 100, mask)) {
                    if (hit.transform.gameObject.tag == "Police") {
                        if(selectedPolice != null)  
                            DisableCheckPoliceMovements();
                        selectedPolice = hit.transform.gameObject;
                        UpdateSelectedPolicePosition();
                        CheckPoliceMovements();
                    } else if (hit.transform.gameObject.tag == "Tile" && selectedPolice != null) {
                        raycastPosX = (int)hit.transform.position.x;
                        raycastPosZ = (int)hit.transform.position.z;
                        var availabletTile = GameObject.Find("Tile" + tiles[raycastPosX] + "-" + raycastPosZ);
                        if (availabletTile.GetComponent<Outline>().enabled == true) {
                            objectMoving = true;
                        }
                    }


                }
            }
        }

        if (objectMoving && isTurnThief) {
            PlayWalking(thief);
            thief.transform.position = Vector3.MoveTowards(thief.transform.position, new Vector3(raycastPosX, thief.transform.position.y, raycastPosZ), 3f * Time.deltaTime);
            if (thief.transform.position == new Vector3(raycastPosX, thief.transform.position.y, raycastPosZ)) {
                objectMoving = false;
                DisableCheckThiefMovement();
                ChangeTurn();
                CurrentTurnDisplay();
                PlayIdle(thief);
            }
        } else if (objectMoving && isTurnPolice) {
            PlayWalking(selectedPolice);
            selectedPolice.transform.position = Vector3.MoveTowards(selectedPolice.transform.position, new Vector3(raycastPosX, selectedPolice.transform.position.y, raycastPosZ), 3f * Time.deltaTime);
            if (selectedPolice.transform.position == new Vector3(raycastPosX, selectedPolice.transform.position.y, raycastPosZ)) {
                objectMoving = false;
                PlayIdle(selectedPolice);
                selectedPolice = null;
                DisableCheckPoliceMovements();
                ChangeTurn();
                CurrentTurnDisplay();
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

        if (CheckDiagonalC(selectedPolicePositionNumber, selectedPolicePositionLetter) && !CheckThiefInDiagonal(selectedPolicePositionNumber - 1, selectedPolicePositionLetter + 1) && !CheckPoliceInDiagonal(selectedPolicePositionNumber - 1, selectedPolicePositionLetter + 1)) {
            TileEnable(Color.blue, selectedPolicePositionNumber - 1, selectedPolicePositionLetter + 1);
        }
        if (CheckDiagonalD(selectedPolicePositionNumber, selectedPolicePositionLetter) && !CheckThiefInDiagonal(selectedPolicePositionNumber - 1, selectedPolicePositionLetter - 1) && !CheckPoliceInDiagonal(selectedPolicePositionNumber - 1, selectedPolicePositionLetter - 1)) {
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

    public void PlayWalking(GameObject objecto) { 
        Animator animator = objecto.transform.GetChild(0).GetComponent<Animator>();
        animator.Play("Walking");
    }
    public void PlayIdle(GameObject objecto) {
        Animator animator = objecto.transform.GetChild(0).GetComponent<Animator>();
        animator.Play("Idle");
    }

    public void ThiefWin() {
        if(thief.transform.position.z == 8) {
            scene.LoadTheScene("ThiefWon");
        }

    }

    public void PoliceWin() {
        if ((!CheckDiagonalA(thiefPositionNumber, thiefPositionLetter) || CheckPoliceInDiagonal(thiefPositionNumber + 1, thiefPositionLetter - 1)) &&
            (!CheckDiagonalB(thiefPositionNumber, thiefPositionLetter) || CheckPoliceInDiagonal(thiefPositionNumber + 1, thiefPositionLetter + 1)) &&
            (!CheckDiagonalC(thiefPositionNumber, thiefPositionLetter) || CheckPoliceInDiagonal(thiefPositionNumber - 1, thiefPositionLetter + 1)) &&
            (!CheckDiagonalD(thiefPositionNumber, thiefPositionLetter) || CheckPoliceInDiagonal(thiefPositionNumber - 1, thiefPositionLetter - 1)) ) {
            scene.LoadTheScene("PoliceWon");

        }

    }



}
