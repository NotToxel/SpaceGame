using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSequence : MonoBehaviour
{
    public GameObject Cam1;
    public GameObject Cam2;
    [SerializeField] private float cutSceneLength;

    void Start() 
    {
        StartCoroutine(TheSequence()); 
    }

    IEnumerator TheSequence()
    {
        yield return new WaitForSeconds(cutSceneLength);
        Cam2.SetActive(true);
        Cam1.SetActive(false);
    }
}
