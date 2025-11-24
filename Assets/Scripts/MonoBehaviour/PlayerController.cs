using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector2 direction;
    private Vector2 lastDirection;
    //private Animator anim;
    [SerializeField] private float speed = 0.2f;
    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction * speed);
    }

    // Update is called once per frame
    void Update()
    {
        direction = Vector2.zero;
        if (Keyboard.current.wKey.isPressed)
        {
            direction.y += 1;
            rb.MoveRotation(0f);
        }
        if (Keyboard.current.aKey.isPressed)
        {
            direction.x -= 1;
            rb.MoveRotation(90f);
        }
        if (Keyboard.current.sKey.isPressed)
        {
            direction.y -= 1;
            rb.MoveRotation(180f);
        }
        if (Keyboard.current.dKey.isPressed)
        {
            direction.x += 1;
            rb.MoveRotation(270f);
        }
        if (direction != Vector2.zero)
        {
            direction = direction.normalized;
            lastDirection = direction;
        }
        /*
        anim.SetFloat("moveX", lastDirection.x);
        anim.SetFloat("moveY", lastDirection.y);
        anim.SetFloat("speed", direction.magnitude);
        */
    }
}
