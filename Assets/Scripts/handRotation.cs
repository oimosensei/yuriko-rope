using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handRotation : MonoBehaviour
{
    private linerender line;
    // Start is called before the first frame update
    void Start()
    {
        line = GameObject.Find("line").GetComponent<linerender>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
