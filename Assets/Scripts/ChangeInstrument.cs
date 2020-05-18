using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeInstrument : MonoBehaviour
{
    public Canvas pianoCanvas;
    public Canvas drumCanvas;

    public void ChangePianoCanvas()
    {
        /*
        if(drumCanvas.isActiveAndEnabled == true)
        {
            drumCanvas.gameObject.SetActive(false);
            pianoCanvas.gameObject.SetActive(true);
        }
        else
        {
            pianoCanvas.gameObject.SetActive(true);
        }
        */
        if(pianoCanvas.isActiveAndEnabled == false)
        {
            pianoCanvas.gameObject.SetActive(true);
        }
        else
        {
            pianoCanvas.gameObject.SetActive(false);
        }
        Debug.Log("Enabled Piano Canvas");
    }
    /*
    public void ChangeDrumCanvas()
    {
        if(pianoCanvas.isActiveAndEnabled == true)
        {
            pianoCanvas.gameObject.SetActive(false);
            drumCanvas.gameObject.SetActive(true);
        }
        else
        {
            drumCanvas.gameObject.SetActive(true);
        }
        Debug.Log("Enabled Drum Canvas");
    }
    */
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
