using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleTransitionHandler : MonoBehaviour
{
    public enum titleStates { centre, exit, enter , load};
    [SerializeField]
    titleStates state;
    [SerializeField]
    Animator titleAnim;
    [SerializeField]
    string level;

    void start()
    {
        titleAnim = gameObject.GetComponent<Animator>();
    }

    public void setState(titleStates newState)
    {
        state = newState;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state) {
            case titleStates.centre:
                titleAnim.SetBool("centre", true);
                titleAnim.SetBool("exit", false);
                titleAnim.SetBool("enter", false);
                break;

            case titleStates.exit:
                titleAnim.SetBool("centre", false);
                titleAnim.SetBool("exit", true);
                titleAnim.SetBool("enter", false);
                break;

            case titleStates.enter:
                titleAnim.SetBool("centre", false);
                titleAnim.SetBool("exit", false);
                titleAnim.SetBool("enter", true);
                break;

            case titleStates.load:
                if (level == "exit") { Application.Quit(0); }
                else { SceneManager.LoadScene(level); }
                break;
        }
    }
}
