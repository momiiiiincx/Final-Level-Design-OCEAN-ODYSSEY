using UnityEngine;

public class Character : MonoBehaviour
{
    public float baseMovementSpeed = 5f;
    
    private float currentMovementSpeed;
    private SpriteRenderer playerSprite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentMovementSpeed = baseMovementSpeed;
        playerSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    
    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        
        if (playerSprite != null)
        {
            if (moveX > 0) 
            {
                playerSprite.flipX = false; 
            }
            else if (moveX < 0) 
            {
                playerSprite.flipX = true;  
            }
        }

        Vector2 movement = new Vector2(moveX, moveY).normalized;
        transform.Translate(movement * currentMovementSpeed * Time.deltaTime);
    }
}
