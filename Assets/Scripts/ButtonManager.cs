using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public Material paletteMaterial;
    public GameObject title;

    void Start()
    {
        title = GameObject.Find("Home Title");
    }

    public void startLevel(string levelName)
    {
        title.GetComponent<Animator>().enabled = true;
        title.GetComponent<TitleTransitionHandler>().setState(TitleTransitionHandler.titleStates.centre);
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
