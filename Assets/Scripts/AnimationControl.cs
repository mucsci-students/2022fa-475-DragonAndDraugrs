using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    public GameManager GameManager;
    public Transform Player;
    int MoveSpeed = 4;
    int MaxDist = 20;
    public Rigidbody rigidbody;
    Animator anim;
    public Vector3 playerDirection;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {



        rigidbody = GetComponent<Rigidbody>();
        //waterBody.velocity = new Vector3(0, 0.5f, 0);
        if (Vector3.Distance(transform.position, Player.position) < MaxDist)
        {
            playerDirection = Player.transform.position - gameObject.transform.position;
            transform.LookAt(Player);
            //rigidbody.velocity = new Vector3(MoveSpeed, 0, 0);
            //transform.rotation = Quaternion.Slerp(transform.rotation
                                       //, Quaternion.LookRotation(Player.position - transform.position)
                                       //, 3f * Time.deltaTime);

            /* Move at Player*/
            transform.position = transform.position + new Vector3(playerDirection.x * MoveSpeed * Time.deltaTime, playerDirection.y * MoveSpeed * Time.deltaTime, playerDirection.z * MoveSpeed * Time.deltaTime);
            //transform.position = Vector3.MoveTowards(transform.position, Player.position, MoveSpeed * Time.deltaTime);
            //transform.position += transform.forward * MoveSpeed * Time.deltaTime;
            anim.SetFloat("speed", MoveSpeed);
            //rigidbody.MovePosition(transform.position + transform.forward * MoveSpeed);

        }
        else
        {
            anim.SetFloat("speed", 0);
        }


        anim.SetFloat("distance", Vector3.Distance(transform.position, Player.position));
    }
}
