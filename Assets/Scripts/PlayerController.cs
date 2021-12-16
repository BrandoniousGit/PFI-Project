using UnityEngine;
using System.Collections;
using System;

public class PlayerController : MonoBehaviour 
{

    float x, y;

    public float moveSpeed, maxSpeed, counterMovement = 0.175f;
    private float threshold = 0.01f;

    public Rigidbody orientation;
    public Transform spawnPoint;
	Rigidbody rb;
	
	void Start()
	{
		rb = gameObject.GetComponent<Rigidbody> ();
	}

    void Update()
	{
        MyInput();
	}
	
	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.tag == "PickUp") {
			other.gameObject.SetActive (false);
		}
        else if (other.gameObject.tag == "DontPickUp") {
			other.gameObject.SetActive (false);
		}
        if (other.gameObject.tag == "Deathplane")
        {
            rb.transform.position = spawnPoint.position;
        }
	}

    void MyInput()
    {
        orientation.transform.position = rb.position;

        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;

        Vector3 movement = new Vector3(x, 0.0f, y);
        rb.AddForce(movement * moveSpeed * Time.deltaTime);
        CounterMovement(x, y, mag);
    }

    void CounterMovement(float x, float y, Vector2 mag)
    {
        if (Math.Abs(mag.x) > threshold && Math.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0))
        {
            rb.AddForce(moveSpeed * orientation.transform.right * Time.deltaTime * -mag.x * counterMovement);
        }
        if (Math.Abs(mag.y) > threshold && Math.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0))
        {
            rb.AddForce(moveSpeed * orientation.transform.forward * Time.deltaTime * -mag.y * counterMovement);
        }

        if (Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) > maxSpeed)
        {
            float fallspeed = rb.velocity.y;
            Vector3 n = rb.velocity.normalized * maxSpeed;
            rb.velocity = new Vector3(n.x, fallspeed, n.z);
        }
    }

    public Vector2 FindVelRelativeToLook()
    {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitue = rb.velocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }
}
