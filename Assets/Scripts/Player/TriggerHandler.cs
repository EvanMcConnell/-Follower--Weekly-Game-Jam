using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerHandler : MonoBehaviour
{
    private GameObject wintext, gametext;
    private LaunchController lc;
    private Vector2 spawnPoint;
    private bool quitAfterLevel;
    private string nextScene;
    private int loadWaitTime;

    public static TriggerHandler CreateComponent(GameObject where, int loadWaitTime, string nextScene, bool quitAfterLevel)
    {
        TriggerHandler th = where.AddComponent<TriggerHandler>();
        th.loadWaitTime = loadWaitTime;
        th.nextScene = nextScene;
        th.quitAfterLevel = quitAfterLevel;
        return th;
    }

    void Start()
    {
        lc = gameObject.GetComponent<LaunchController>();
        spawnPoint = transform.position;
        if (!gametext) { gametext = GameObject.Find("Gametext"); }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Ground")
        {
            print("collided with water");
            if (lc.launched == false)
            {
                lc.launchReady = true;
            }
        }

        if (coll.gameObject.name == "Grid")
        {
            print("collided with griddy water");
        }

        print("collided");
        if (lc && lc.launched == true)
        {
            //launchReady = true;
            if (lc.bouncesRemaining == 0)
            {
                lc.launched = false;
                lc.toggleBounciness();
            }
            else { lc.bouncesRemaining--; }
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "endzone")
        {
            gametext.SetActive(false);
            wintext.SetActive(true);
            StartCoroutine(nextLevel());
        }

        if (coll.tag == "killzone")
        {
            lc.toggleBounciness();
            transform.position = spawnPoint;
            print("dead");
        }

        IEnumerator nextLevel()
        {
            yield return new WaitForSecondsRealtime(loadWaitTime);

            if (!quitAfterLevel)
            {
                int count = SceneManager.GetActiveScene().buildIndex + 1;
                SceneManager.LoadScene(nextScene);
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(nextScene));
            }
            else { Application.Quit(0); }
        }
    }

}
