using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuTransitionHandler : MonoBehaviour
{
    public enum menuStates { idle, topEnter, topExit, bottomEnter, bottomExit, leftEnter, leftExit, rightEnter, rightExit };

    //public static menuStates[] menuStatesArray = new menuStates[5];
    [SerializeField]
    menuStates state;
    [SerializeField]
    Animator menuAnim;

    void Start()
    {
        menuAnim = gameObject.GetComponent<Animator>();
        /*
        menuStatesArray[0] = menuStates.idle;
        menuStatesArray[1] = menuStates.topEnter;
        menuStatesArray[2] = menuStates.topExit;
        menuStatesArray[3] = menuStates.bottomEnter;
        menuStatesArray[4] = menuStates.bottomExit;
        */
    }

    public void setState(menuStates newState)
    {
        state = newState;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state) {
            case menuStates.idle:
                menuAnim.SetBool("idle", true);
                menuAnim.SetBool("topExit", false);
                menuAnim.SetBool("topEnter", false);
                menuAnim.SetBool("bottomExit", false);
                menuAnim.SetBool("bottomEnter", false);
                menuAnim.SetBool("leftExit", false);
                menuAnim.SetBool("leftEnter", false);
                menuAnim.SetBool("rightExit", false);
                menuAnim.SetBool("rightEnter", false);
                break;

            case menuStates.topExit:
                menuAnim.SetBool("idle", false);
                menuAnim.SetBool("topExit", true);
                menuAnim.SetBool("topEnter", false);
                menuAnim.SetBool("bottomExit", false);
                menuAnim.SetBool("bottomEnter", false);
                menuAnim.SetBool("leftExit", false);
                menuAnim.SetBool("leftEnter", false);
                menuAnim.SetBool("rightExit", false);
                menuAnim.SetBool("rightEnter", false);
                break;

            case menuStates.topEnter:
                menuAnim.SetBool("idle", false);
                menuAnim.SetBool("topExit", false);
                menuAnim.SetBool("topEnter", true);
                menuAnim.SetBool("bottomExit", false);
                menuAnim.SetBool("bottomEnter", false);
                menuAnim.SetBool("leftExit", false);
                menuAnim.SetBool("leftEnter", false);
                menuAnim.SetBool("rightExit", false);
                menuAnim.SetBool("rightEnter", false);
                break;

            case menuStates.bottomEnter:
                menuAnim.SetBool("idle", false);
                menuAnim.SetBool("topExit", false);
                menuAnim.SetBool("topEnter", false);
                menuAnim.SetBool("bottomExit", false);
                menuAnim.SetBool("bottomEnter", true);
                menuAnim.SetBool("leftExit", false);
                menuAnim.SetBool("leftEnter", false);
                menuAnim.SetBool("rightExit", false);
                menuAnim.SetBool("rightEnter", false);
                break;

            case menuStates.bottomExit:
                menuAnim.SetBool("idle", false);
                menuAnim.SetBool("topExit", false);
                menuAnim.SetBool("topEnter", false);
                menuAnim.SetBool("bottomExit", true);
                menuAnim.SetBool("bottomEnter", false);
                menuAnim.SetBool("leftExit", false);
                menuAnim.SetBool("leftEnter", false);
                menuAnim.SetBool("rightExit", false);
                menuAnim.SetBool("rightEnter", false);
                break;

            case menuStates.leftEnter:
                menuAnim.SetBool("idle", false);
                menuAnim.SetBool("topExit", false);
                menuAnim.SetBool("topEnter", false);
                menuAnim.SetBool("bottomExit", false);
                menuAnim.SetBool("bottomEnter", false);
                menuAnim.SetBool("leftExit", false);
                menuAnim.SetBool("leftEnter", true);
                menuAnim.SetBool("rightExit", false);
                menuAnim.SetBool("rightEnter", false);
                break;

            case menuStates.leftExit:
                menuAnim.SetBool("idle", false);
                menuAnim.SetBool("topExit", false);
                menuAnim.SetBool("topEnter", false);
                menuAnim.SetBool("bottomExit", false);
                menuAnim.SetBool("bottomEnter", false);
                menuAnim.SetBool("leftExit", true);
                menuAnim.SetBool("leftEnter", false);
                menuAnim.SetBool("rightExit", false);
                menuAnim.SetBool("rightEnter", false);
                break;

            case menuStates.rightEnter:
                menuAnim.SetBool("idle", false);
                menuAnim.SetBool("topExit", false);
                menuAnim.SetBool("topEnter", false);
                menuAnim.SetBool("bottomExit", false);
                menuAnim.SetBool("bottomEnter", false);
                menuAnim.SetBool("leftExit", false);
                menuAnim.SetBool("leftEnter", false);
                menuAnim.SetBool("rightExit", false);
                menuAnim.SetBool("rightEnter", true);
                break;

            case menuStates.rightExit:
                menuAnim.SetBool("idle", false);
                menuAnim.SetBool("topExit", false);
                menuAnim.SetBool("topEnter", false);
                menuAnim.SetBool("bottomExit", false);
                menuAnim.SetBool("bottomEnter", false);
                menuAnim.SetBool("leftExit", false);
                menuAnim.SetBool("leftEnter", false);
                menuAnim.SetBool("rightExit", true);
                menuAnim.SetBool("rightEnter", false);
                break;
        }
    }
}
