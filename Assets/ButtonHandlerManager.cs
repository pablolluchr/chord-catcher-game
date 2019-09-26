using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandlerManager : MonoBehaviour
{
    // Start is called before the first frame update
    public void ButtonDown()
    {
        GameManager.instance.PressButton();
    }
    public void ButtonUp()
    {
        GameManager.instance.ReleaseButton();

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
