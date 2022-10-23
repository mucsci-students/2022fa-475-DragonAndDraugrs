using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class reset : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject collidedWith = GetComponent<Collider>().gameObject;
        if (other.tag.Equals("Player"))
        {
            SceneManager.LoadScene(sceneName: "VolcanoLevel");
        }
    }
}
