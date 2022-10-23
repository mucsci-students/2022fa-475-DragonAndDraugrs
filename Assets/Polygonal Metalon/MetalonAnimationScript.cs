using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalonAnimationScript : MonoBehaviour
{
    public Transform Player;
    public int Health = 100;
    int MaxDist = 20;
    int AttackDist = 4;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
         if (Vector3.Distance(transform.position, Player.position) < MaxDist)
        {
            if (Vector3.Distance(transform.position, Player.position) < AttackDist)
            {
                anim.SetBool("Run Forward", false);
                Attack();
            }
            else
            {
                anim.SetBool("Run Forward", true);
            }
            var targetPos = Player.position;
            if (targetPos.x < transform.position.x && changed(transform.position.y, targetPos.y))
            {
                anim.ResetTrigger("Turn Right");
                anim.SetTrigger("Turn Right");
            }
            else if (changed(transform.position.y, targetPos.y))
            {
                anim.ResetTrigger("Turn Left");
                anim.SetTrigger("Turn Left");
            }
            else
            {
                anim.SetBool("Run Forward", true);
            }

        }
        else
        {
            anim.SetBool("Run Forward", false);
        }
        
        anim.SetBool("Run Forward", true);
    }


    void Attack()
    {
        anim.ResetTrigger("Smash Attack");
        anim.SetTrigger("Smash Attack");
    }

    bool changed(float a, float b)
    {
        if ((int)a == (int)b)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
