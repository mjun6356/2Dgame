using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    

    
    [Header("기본 이동 설정")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float turnSpeed = 10.0f;


    [Header("점프 개선 설정")]
    public float fallMultplier = 2.5f;
    public float lowJumMultiplier = 2.0f;

    [Header("지면 감지 설정")]
    public float coyoteTime = 0.15f;
    public float coyoteTimeCounter;
    public bool realGrouned = true;

    [Header("글라이더 설정")]
    public GameObject gliderObject;
    public float gliderFallSpeed =1.0f;
    public float gliderFMoveSpeed = 7.0f;
    public float gliderMaxTime = 5.0f;
    public float gliderTimerLeft;
    public bool isGliding = false;


    public Rigidbody rb;
    
    private bool isGrounded;
    
    public int coinCount = 0;
    
    private Vector3 movement;
    public Quaternion targetRotation;

    // Start is called once befor           e the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coyoteTimeCounter = 0;
        if(gliderObject != null)
        {
           gliderObject.SetActive(false);
        }

        gliderTimerLeft = gliderMaxTime;
    }
    void EnbleGlider()
    {
        isGliding = true;

        if (gliderObject != null)
        {
            gliderObject.SetActive(true);
        }
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, -gliderFallSpeed, rb.linearVelocity.z);
    }
    void DisableGlider()
    {
        isGliding = false;
        
        if(gliderObject != null)
        {
            gliderObject.SetActive(false);
        }
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
    }

    void UpdateGrounededStarte()
    {
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            isGrounded = true;
        }
        else
        {
            if (coyoteTimeCounter > 0)
            {
                coyoteTimeCounter-=Time.deltaTime;
                isGrounded = true;

            }
            else
            {
                isGrounded = false;
            }
        }
    }
    void ApplyGliderMovement(float horizontal, float vertical)
    {
        Vector3 gliderVelocity = new Vector3(horizontal * gliderFMoveSpeed, -gliderFallSpeed , vertical * gliderFMoveSpeed);

        rb.linearVelocity = gliderVelocity;
    }


    // Update is called once per frame
    void Update()
    {
        UpdateGrounededStarte();

      float moveHorizontal = Input.GetAxis("Horizontal");
      float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);

        if (movement.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation , targetRotation, turnSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.G) && !isGrounded && gliderTimerLeft > 0)
        {
            if (!isGliding)
            {
                EnbleGlider();
            }
            gliderTimerLeft -= Time.deltaTime;

            if(gliderTimerLeft <= 0)
            {
                DisableGlider();
            }
        }
        else if (isGliding)
        {
            DisableGlider ();
        }

        if (isGliding)
        {
            ApplyGliderMovement(moveHorizontal, moveVertical);
        }
        else
        {
            rb.linearVelocity = new Vector3(moveHorizontal * moveSpeed, rb.linearVelocity.y, moveVertical * moveSpeed);

            if (rb.linearVelocity.y < 0)
            {
                rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultplier - 1) * Time.deltaTime;
            }
            else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.linearVelocity += Vector3.up * Physics.gravity.y * (lowJumMultiplier - 1) * Time.deltaTime;
            }

        }

       


        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            realGrouned = false;
            coyoteTimeCounter = 0;
        }
        if (isGrounded)
        {
            if (isGliding)
            {
                DisableGlider();
            }
            gliderTimerLeft = gliderMaxTime;
        }

    }

    

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            realGrouned = true;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            realGrouned = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
      if( collision.gameObject.tag == "Ground")
        {
            realGrouned = false;
        }   
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

}
