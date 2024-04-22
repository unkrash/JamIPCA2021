using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rb;
    private Animator _anim;
    private float xInput;
    private float jumpInput;
    private bool canJump;
    private bool isGrounded;
    private bool inBoat;

    public Rigidbody boatRB;

    public float moveSpeed;
    public float jumpSpeed;

    public LayerMask groundCheckLayer;
    public float groundCheckDistance;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    public int coins;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        coins = 0;
    }

    private void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        jumpInput = Input.GetAxisRaw("Jump");
        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance, groundCheckLayer);

        if (isGrounded)
        {
            if (hit.collider.CompareTag("Boat"))
            {
                inBoat = true;
                Debug.Log("IN BOAT");

                if(coins != 0)
                {
                    //correr animacao entrega
                    GameManager.instance.OnPlayerReturn(coins);
                    coins = 0;
                }
            }
            else
            {
                inBoat = false;
                Debug.Log("OUT OF BOAT");
            }
        }

        Debug.Log("GROUNDED: " + isGrounded);

        //Better jump
        if (_rb.velocity.y < 0)
        {
            _anim.SetBool("isJumping", false);
            _rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            if (isGrounded)
            {
                _anim.SetBool("isGrounded", true);
            }
        }
        else if (_rb.velocity.y > 0 && jumpInput.Equals(0))
        {
            _rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (xInput == 0)
        {
            _anim.SetBool("isRunning", false);
        }
        else
        {
            _anim.SetBool("isRunning", true);
        }

        if(xInput == 1)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 0.7f);
        } else if (xInput == -1)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, -0.7f);
        }


        _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y, xInput * moveSpeed);

        if (inBoat && xInput == 1)
        {
            _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y, _rb.velocity.z + boatRB.velocity.z);
        }

        if (jumpInput.Equals(1) && isGrounded && canJump)
        {
            _rb.velocity = new Vector3(_rb.velocity.x, jumpSpeed, _rb.velocity.z);
            canJump = false;
            _anim.SetBool("isJumping", true);
            _anim.SetBool("isGrounded", false);
        }

        if (isGrounded && jumpInput.Equals(0))
        {
            canJump = true;
        }

        if (inBoat && xInput.Equals(0))
        {
            _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y, boatRB.velocity.z);
        }
    }
}
