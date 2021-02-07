using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestListMaterials : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] Camera myCamera;
    void Start()
    {
        //myCamera.opaqueSortMode = UnityEngine.Rendering.OpaqueSortMode.NoDistanceSort;

        Debug.Log("Printed " + Texture.streamingTextureCount);

        foreach (Transform obj in Object.FindObjectsOfType<Transform>())
        {
            if(obj.GetComponent<Renderer>() != null)
            {
                //Debug.Log(obj.GetComponent<Renderer>().material.name + "\n");
            }
        }
    }

}
