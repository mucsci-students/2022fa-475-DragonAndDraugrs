using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MoveTowards : MonoBehaviour
{

    public GameManager GameManager;
    public Transform Player;
    int MoveSpeed = 4;
    int MaxDist = 20;




    void Start()
    {

    }

    void Update()
    {

        if (Vector3.Distance(transform.position, Player.position) < MaxDist)
        {
            transform.LookAt(Player);
            transform.position += transform.forward * MoveSpeed * Time.deltaTime;

        }

    }

    void OnTriggerEnter (Collider other)
    {
        
    }
}
