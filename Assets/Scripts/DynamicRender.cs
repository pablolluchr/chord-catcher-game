using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicRender : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnBecameVisible()
    {
        gameObject.SetActive(true);

    }

    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);

    }
}
