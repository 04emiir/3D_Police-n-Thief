using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class WinLoseScene : MonoBehaviour
{
    public Animator thiefAnim;
    public Animator policeAnim;
    public Animator policeAnim1;
    public Animator policeAnim2;
    public Animator policeAnim3;

    public Camera main;
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "PoliceWon") {
            thiefAnim.Play("Terrified");
            policeAnim.Play("Victory");
            policeAnim1.Play("Cheering");
            policeAnim2.Play("Victory");
            policeAnim3.Play("Cheering");
        } else if (scene.name == "ThiefWon") {
            thiefAnim.Play("Chicken Dance");
            policeAnim.Play("Angry");
            policeAnim1.Play("Sad Idle");
            policeAnim2.Play("Angry");
            policeAnim3.Play("Sad Idle");
        }
        StartCoroutine(Restart());
    }

    // Update is called once per frame
    void Update()
    {
        main.transform.LookAt(target.transform);
        main.transform.Translate(Vector3.right * Time.deltaTime);
    }

    IEnumerator Restart() {
        yield return new WaitForSeconds(10f);
        SceneManager.LoadScene("Game");
    }
}
