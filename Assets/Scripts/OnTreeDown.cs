using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTreeDown : MonoBehaviour
{
    void OnDestroy()
    {
        //add event invoker
        Debug.Log("Tree Down");
    }
}
