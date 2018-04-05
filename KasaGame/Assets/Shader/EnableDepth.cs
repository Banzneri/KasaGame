using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
[ExecuteInEditMode]
public class EnableDepth : MonoBehaviour
{
 
    private Camera cam;
 
    // Use this for initialization
    void Awake()
    {
        cam = GetComponent<Camera>();
        cam.depthTextureMode = DepthTextureMode.Depth;
    }
 
    // Update is called once per frame
    void Update()
    {
 
    }
}