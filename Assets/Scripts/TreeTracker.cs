using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeTracker : MonoBehaviour
{
    public static int treesCut = 0; // Shared counter for all trees

    void OnDestroy()
    {
        treesCut++; // Increment when this tree is destroyed
    }

}
