using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFaceCamera : MonoBehaviour
{
    private Camera m_MainCamera;

    private void Start()
    {
        m_MainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + m_MainCamera.transform.rotation * Vector3.back,
            m_MainCamera.transform.rotation * Vector3.up);
    }
}
