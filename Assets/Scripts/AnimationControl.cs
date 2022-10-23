using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    public GameManager GameManager;
    public Transform Player;
    int MoveSpeed = 4;
    float Force = 15f;
    int MaxDist = 20;
    public Rigidbody rigidbody;
    Animator anim;
    public Vector3 playerDirection;
    // Start is called before the first frame update

    void Start()
    {
        anim = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {


        if (Vector3.Distance(transform.position, Player.position) < MaxDist)
        {
            transform.LookAt(Player);
            //

            /* Move at Player*/
            //playerDirection = Player.transform.position - gameObject.transform.position;
            //transform.position = transform.position + new Vector3(playerDirection.x * MoveSpeed * Time.deltaTime, playerDirection.y * MoveSpeed * Time.deltaTime, playerDirection.z * MoveSpeed * Time.deltaTime);
            //transform.position = Vector3.MoveTowards(transform.position, Player.position, MoveSpeed * Time.deltaTime);
            //transform.position += transform.forward * MoveSpeed * Time.deltaTime;
            //rigidbody.MovePosition(transform.position + transform.forward * MoveSpeed);
            rigidbody.AddForce(transform.forward * Force);
            anim.SetFloat("speed", MoveSpeed);

        }
        else
        {
            anim.SetFloat("speed", 0);
        }


        anim.SetFloat("distance", Vector3.Distance(transform.position, Player.position));
    }
}
