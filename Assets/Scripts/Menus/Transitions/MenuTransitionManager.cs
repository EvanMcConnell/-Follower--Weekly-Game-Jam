using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTransitionManager : MonoBehaviour
{
    public GameObject Menu1, Menu2;
    public MenuTransitionHandler.menuStates Menu1NewState, Menu2NewState;
    public GameObject boombox;
    public int lowPassFilterValue = 22000;
}
