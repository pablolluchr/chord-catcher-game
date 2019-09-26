using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public bool buttonBeingPressed = false;
    public int numberOfButtonsBeingPressed = 0;

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    public void PressButton()
    {
        numberOfButtonsBeingPressed++;
        buttonBeingPressed = true;

    }
    public void ReleaseButton()
    {
        numberOfButtonsBeingPressed--;
        buttonBeingPressed = numberOfButtonsBeingPressed > 0;
    }

}