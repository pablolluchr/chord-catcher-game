using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFaceCameraReverse : MonoBehaviour
{
    private Camera m_MainCamera;

    private void Start()
    {
        m_MainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(m_MainCamera.transform);
        transform.LookAt(transform.position + m_MainCamera.transform.rotation * Vector3.forward,
            m_MainCamera.transform.rotation * Vector3.up);
    }
}
