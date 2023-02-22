using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneGenerator : MonoBehaviour
{

    // private GameObject plane;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.localScale = new Vector3(constant.planeLength, constant.planeYScale, constant.planeLength);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
