using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    walk,
    attack,
    interact,
}

public class PlayerMovement : MonoBehaviour
{
    public PlayerState currentState;
    //floats
    public float speed;

    //physics componenets
    private Rigidbody2D myRigidBody;

    //vectors
    private Vector3 change;

    //animation components
    private Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        currentState = PlayerState.walk;
        animator = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveX", -1);
    }

    // Update is called once per frame
    void Update()
    {
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");

        if (Input.GetButton("attack") && currentState != PlayerState.attack)
        {
            Debug.Log("ATTAAACK");
            StartCoroutine(AttackCo());
        }

        else if(currentState == PlayerState.walk)
        {
            updateAnimationAndMove();
        }
        
        
    }

    private IEnumerator AttackCo()
    {
        animator.SetBool("attacking", true);
        currentState = PlayerState.attack;
        yield return null;
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(.3f);
        currentState = PlayerState.walk;
    }

    void updateAnimationAndMove()
    {
        //if character is moving change animation float values according to the axis value change
        if (change != Vector3.zero)
        {
            moveCharacter();
            animator.SetFloat("moveX", change.x);
            animator.SetFloat("moveY", change.y);
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
        }
    }

    void moveCharacter()
    {
        change.Normalize();
        myRigidBody.MovePosition(transform.position + change * speed * Time.fixedDeltaTime);
    }
}
