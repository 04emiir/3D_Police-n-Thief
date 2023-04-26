using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameController : MonoBehaviour {
    bool isSelectedThief = false;
    bool isSelectedPolice = false;

    private GameObject selectedPolice;
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit raycastHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out raycastHit, 100f)) {
                if (raycastHit.transform != null) {
                    ClickedPiece(raycastHit.transform.gameObject);
                }
            }
        }
    }

    public void ClickedPiece(GameObject gameObject) {
        if (gameObject.tag == "Thief") {
            isSelectedThief = !isSelectedThief;
            isSelectedPolice = false;

        }

        if (gameObject.tag == "Police") {
            isSelectedThief = false;
            isSelectedPolice = !isSelectedThief;
            selectedPolice = gameObject;
            if (isSelectedPolice) {
                selectedPolice.GetComponent<Outline>().enabled = true;
            } else {
                selectedPolice.GetComponent<Outline>().enabled = false;
            }

        }

        if (gameObject.tag == "Tile" && (isSelectedPolice || isSelectedThief)) {
            if(isSelectedPolice)
                MovePiece(selectedPolice, gameObject.transform);
            if(isSelectedThief)
                MovePiece(GameObject.FindGameObjectWithTag("Thief"), gameObject.transform);
        }
    }

    public void MovePiece (GameObject piece, Transform newPosition) {
            piece.transform.position = newPosition.position;

    }

}
