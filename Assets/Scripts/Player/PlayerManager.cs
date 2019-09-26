using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public bool buttonBeingPressed = false;
    #region Singleton
    public static PlayerManager instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    public GameObject player;
}
