using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    walk,
    attack,
    interact,
    stagger,
    idle
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

        //input for the attack animation , cant attack while staggered
        if (Input.GetButton("attack") && currentState != PlayerState.attack
            && currentState != PlayerState.stagger)
        {
            StartCoroutine(AttackCo());
        }

        else if (currentState == PlayerState.walk || currentState == PlayerState.idle)
        {
            updateAnimationAndMove();
        }
        
        
    }

    public void Knock(float knockTime)
    {
        StartCoroutine(KnockCo(knockTime));
    }

    private IEnumerator KnockCo(float knockTime)
    {
        //make it so that staggers are not stacked and forces dont multiply
        if (myRigidBody != null)
        {
            yield return new WaitForSeconds(knockTime);
            myRigidBody.velocity = Vector2.zero;
            currentState = PlayerState.idle;
            myRigidBody.velocity = Vector2.zero;

        }
    }

    private IEnumerator AttackCo()
    {
        //set player state to attack and freeze movement
        animator.SetBool("attacking", true);
        currentState = PlayerState.attack;
        yield return null;

        //if the attack has finished, return the ability to move
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
