using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public Material paletteMaterial;


    public void activateMenuTransition(GameObject transitionManagerHolder)
    {
        //if (Menu1) { Menu1.GetComponent<MenuTransitionHandler>().setState(MenuTransitionHandler.menuStatesArray[Menu1NewState]); }
        MenuTransitionManager tm = transitionManagerHolder.GetComponent<MenuTransitionManager>();
        MenuTransitionHandler menu1Handler = tm.Menu1.GetComponent<MenuTransitionHandler>();
        MenuTransitionHandler menu2Handler = tm.Menu2.GetComponent<MenuTransitionHandler>();
        MenuTransitionHandler.menuStates menu1State = tm.Menu1NewState;
        MenuTransitionHandler.menuStates menu2State = tm.Menu2NewState;

        menu1Handler.setState(menu1State);
        print(menu1State);
        menu2Handler.setState(menu2State);
        print(menu2State);

        print("ooooooh thank you so so much");

        if (tm.boombox) { tm.boombox.GetComponent<AudioLowPassFilter>().cutoffFrequency = tm.lowPassFilterValue; }
    }


    public void mainMenuStartButton(GameObject title)
    {
        title.GetComponent<TitleTransitionHandler>().setState(TitleTransitionHandler.titleStates.centre);
        print("we did it, we saved the world");
    }


    public void quitGame()
    {
        if (Application.isPlaying) { Application.Quit(0); }
        else { UnityEditor.EditorApplication.isPlaying = false; }
        UnityEditor.EditorApplication.isPlaying = false;
    }


    public void swapPalette(bool choice)
    {
        if (choice == true) { 
            paletteMaterial.SetFloat("_PaletteChoice", paletteMaterial.GetFloat("_PaletteChoice") + 0.1f);
            print("palette up");
        }
        else {
            paletteMaterial.SetFloat("_PaletteChoice", paletteMaterial.GetFloat("_PaletteChoice") - 0.1f);
            print("palette down");
        }
        if(paletteMaterial.GetFloat("_PaletteChoice") > 1) { paletteMaterial.SetFloat("_PaletteChoice", 0.05f); }
        if (paletteMaterial.GetFloat("_PaletteChoice") < 0) { paletteMaterial.SetFloat("_PaletteChoice", 0.95f); }
    }
}
