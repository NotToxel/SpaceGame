using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options : MonoBehaviour
{

    // toggles fullscreen 
    public void Fullscreen(){

        Screen.fullScreen = !Screen.fullScreen;
        print("Screen changed");
    }
}
