using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterShip : MonoBehaviour
{
    // Start is called before the first frame update
    private float max_forward_speed = 12;
    private float forward_force_coef = 2;
    private float max_rotational_speed = 15; //threeeeee
    private float rotational_force_coef = .8f;
    private float max_strafe_speed = 2;
    private float strafe_force_coef = .7f;

    private Rigidbody2D rb;
    private CapsuleCollider2D cc;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapsuleCollider2D>();
    }
    void Update()
    {
        //BUG TODO WHATEVER strafe and rotation inverted currently
        float strafe_in = Input.GetAxisRaw("Rotational") * strafe_force_coef;
        float forward_in = Input.GetAxisRaw("Vertical") * forward_force_coef;
        //Actually don't want a whole axis for forward stuff, it's gotta be different
        //for the up and down key because big engines on one side.
        float rotation_in = Input.GetAxisRaw("Horizontal") * rotational_force_coef; //need to add a new axis
        Vector2 direction_vec = facingVector();
        if (forward_in > 0)
        {
            rb.AddForce(direction_vec * forward_in);   
        }
        else if (forward_in < 0)
        {
            Vector2 retrograde = rotate180(rb.velocity);

        }
        rb.AddForce(clockwise90(direction_vec) * strafe_in);

        if (rotation_in == 0)
        {
            rb.angularVelocity = 0;
        }
        else if (forward_in >= 0)
        {
            rb.AddTorque(rotation_in);
        }


    }
    private Vector2 facingVector() //returns a normalized vector
    {   //this is like, negative sin posotive cos or some nonsense, but adding 90 works, if less well
        Vector2 direction_vec = new Vector2(Mathf.Cos((rb.rotation + 90f) * Mathf.Deg2Rad), Mathf.Sin((rb.rotation + 90f) * Mathf.Deg2Rad));
        return direction_vec;
    }

    private Vector2 rotate180(Vector2 vector)
    {
        return new Vector2(-vector.y, -vector.x);
    }
    private Vector2 clockwise90(Vector2 vector)
    {
        return new Vector2(vector.y, -vector.x);
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        // Vector2 forward = facingVector();
        // Vector2 starbord = clockwise90(forward);
        Vector2 direction_vector = facingVector();
        float forward_velocity = Vector2.Dot(rb.velocity, direction_vector);
        float strafe_velocity =  Vector2.Dot(rb.velocity, clockwise90(direction_vector));
        Vector2 forward_vector = direction_vector * forward_velocity;
        Vector2 strafe_vector = clockwise90(direction_vector) * strafe_velocity;

        if (Mathf.Abs(forward_velocity) > max_forward_speed)
        {
            //FIND COMPONENT IN ORTHOGANAL DIRECTION
            //NEW VECTOR IS FORWARD VECTOR + ORTHOGANAL VECTOR
            forward_velocity = Mathf.Clamp(forward_velocity, -max_forward_speed, max_forward_speed);

            forward_vector =  direction_vector * forward_velocity;
        }
        if (Mathf.Abs(strafe_velocity) > max_strafe_speed)
        {
            strafe_velocity = Mathf.Clamp(strafe_velocity, -max_strafe_speed, max_strafe_speed);

            strafe_vector = clockwise90(direction_vector) * strafe_velocity;
        }
        Vector2 velocity_vector = strafe_vector + forward_vector;
        rb.velocity = velocity_vector;

        if (Mathf.Abs(rb.angularVelocity) > max_rotational_speed)
        {
            rb.angularVelocity = Mathf.Clamp(rb.angularVelocity, -max_rotational_speed, max_rotational_speed);
        }

    }
}
