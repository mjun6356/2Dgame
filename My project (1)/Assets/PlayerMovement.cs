using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
   public float moveSpeed = 5f;
   public Rigidbody rb;
    public float jumpForce = 5f;
    private bool isGrounded;
    
    public int coinCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      float moveHorizontal = Input.GetAxis("Horizontal");
      float moveVertical = Input.GetAxis("Vertical");

        rb.linearVelocity = new Vector3(moveHorizontal * moveSpeed, rb.linearVelocity.y, moveVertical * moveSpeed);
        if (Input.GetKeyDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void OnTriggerEnter(Collider other)
    { 
        if(other.CompareTag("Coin"))
        {
            coinCount++;
            Destroy(other.gameObject);
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
