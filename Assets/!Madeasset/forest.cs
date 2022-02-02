using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class forest : MonoBehaviour
{
    public string sceneName;

    void OnTriggerEnter2D(Collider2D c)
    {
        Debug.Log("Hello world!");
        if (c.tag == "Player")
            UnityEngine.SceneManagement.SceneManager.LoadScene("Forest");
    }
}