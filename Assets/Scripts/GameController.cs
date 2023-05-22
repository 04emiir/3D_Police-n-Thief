using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class GameController : MonoBehaviour {
    bool isTurnThief = true;
    bool isTurnPolice = false;

    public TextMeshProUGUI currentTurn;

    private GameObject selectedPolice;
    private GameObject thiefGO;
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        //if (Input.GetMouseButtonDown(0)) {
        //    RaycastHit raycastHit;
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    if (Physics.Raycast(ray, out raycastHit, 100f)) {
        //        if (raycastHit.transform != null) {
        //            CheckPossibleMovement(raycastHit.transform.gameObject);
        //        }
        //    }
        //}
    }

    public void CheckCurrentTurn() {
        if (isTurnThief) {
            currentTurn.text = "Turno Ladrón";
            currentTurn.color = Color.red;
        } else {
            currentTurn.text = "Turno Policías";
            currentTurn.color = Color.blue;
        }

    }

    //public void CheckPossibleMovement(GameObject gameObject) {
    //    if (gameObject.tag == "Thief") {
    //        isSelectedThief = !isSelectedThief;
    //        isSelectedPolice = false;

    //    }

    //    if (gameObject.tag == "Police") {
    //        isSelectedThief = false;
    //        isSelectedPolice = !isSelectedThief;
    //        selectedPolice = gameObject;
    //        if (isSelectedPolice) {
    //            selectedPolice.GetComponent<Outline>().enabled = true;
    //        } else {
    //            selectedPolice.GetComponent<Outline>().enabled = false;
    //        }

    //    }

    //    if (gameObject.tag == "Tile" && (isSelectedPolice || isSelectedThief)) {
    //        if(isSelectedPolice)
    //            MovePiece(selectedPolice, gameObject.transform);
    //        if(isSelectedThief)
    //            MovePiece(GameObject.FindGameObjectWithTag("Thief"), gameObject.transform);
    //    }
    //}

    public void CheckMovementThief() {
        // Betweewn (1,0,1) and (8,0,8)
        /* (x+1, y, z+1) right+up
         * (x+1, y, z-1) right+down
         * (x-1, y, z+1) left+up
         * (x-1, y, z-1) left+down
         */
        float thiefPositionRow = thiefGO.transform.position.z;
        float thiefPositionColumn = thiefGO.transform.position.x;

        if ((thiefPositionColumn + 1) <= 8 && (thiefPositionColumn + 1) <= 8) {
            //can move (x+1, y, z+1) right+up
        }
        if ((thiefPositionColumn + 1) <= 8 && (thiefPositionColumn - 1) >= 1) {
            //can move  (x+1, y, z-1) right+down
        }
        if ((thiefPositionColumn - 1) >= 1 && (thiefPositionColumn - 1) >= 1) {
            //can move (x-1, y, z+1) left+up
        }
        if ((thiefPositionColumn - 1) >= 8 && (thiefPositionColumn + 1) <= 8) {
            //can move (x-1, y, z-1) left+down
        }



    }

    public void MovePiece (GameObject piece, Transform newPosition) {
            piece.transform.position = newPosition.position;

    }

}
