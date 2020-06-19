using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleTransitionHandler : MonoBehaviour
{
    public enum titleStates { idle, centre, exit, enter, home, load};
    [SerializeField]
    titleStates state;
    [SerializeField]
    Animator titleAnim;
    [SerializeField]
    string level;

    void Start()
    {
        titleAnim = gameObject.GetComponent<Animator>();
    }

    public void setState(titleStates newState)
    {
        state = newState;
        print("it's ya boii");
    }

    // Update is called once per frame
    void Update()
    {
        switch (state) {
            case titleStates.idle:
                titleAnim.SetBool("idle", true);
                titleAnim.SetBool("centre", false);
                titleAnim.SetBool("exit", false);
                titleAnim.SetBool("enter", false);
                titleAnim.SetBool("home", false);
                break;

            case titleStates.centre:
                titleAnim.SetBool("idle", false);
                titleAnim.SetBool("centre", true);
                titleAnim.SetBool("exit", false);
                titleAnim.SetBool("enter", false);
                titleAnim.SetBool("home", false);
                break;

            case titleStates.exit:
                titleAnim.SetBool("idle", false);
                titleAnim.SetBool("centre", false);
                titleAnim.SetBool("exit", true);
                titleAnim.SetBool("enter", false);
                titleAnim.SetBool("home", false);
                break;

            case titleStates.enter:
                titleAnim.SetBool("idle", false);
                titleAnim.SetBool("centre", false);
                titleAnim.SetBool("exit", false);
                titleAnim.SetBool("enter", true);
                titleAnim.SetBool("home", false);
                break;

            case titleStates.home:
                titleAnim.SetBool("idle", false);
                titleAnim.SetBool("centre", false);
                titleAnim.SetBool("exit", false);
                titleAnim.SetBool("enter", false);
                titleAnim.SetBool("home", true);
                break;

            case titleStates.load:
                if (level == "exit") { Application.Quit(0); }
                else { SceneManager.LoadScene(level); }
                break;
        }
    }
}
