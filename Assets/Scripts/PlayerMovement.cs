using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//good
public class PlayerMovement : MonoBehaviour {

    private bool good;

    public string hAxis;
    public string vAxis;

    private float delayedRotation, rotation, rs;
    private float delayedMovement, movement, ms;
    private float delay;

    public float rotationSpeed;
    public float movementSpeed;

    private Rigidbody rb;

    // Use this for initialization
    void Start () {
        delay = 0;
        rb = GetComponent<Rigidbody> ();
        rs = rotationSpeed;
        ms = movementSpeed;
        good = true;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        rotation = Input.GetAxis(hAxis);
        movement = Input.GetAxis(vAxis);

        if (delay > 0)
        {
            StartCoroutine(getInput(rotation, movement));
        }
        else
        {
            delayedRotation = rotation;
            delayedMovement = movement;
        }

        if (rotation != 0)
        {
            transform.Rotate(0, delayedRotation * rs, 0);
        }

        float yAngle = Mathf.Deg2Rad * (transform.eulerAngles.y + 90);

        Vector3 shift = new Vector3(Mathf.Sin(yAngle), 0, Mathf.Cos(yAngle));

        transform.position += shift * ms * delayedMovement;

	}

    public IEnumerator getInput(float r, float m)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log(delay);
        delayedRotation = r;
        delayedMovement = m;
    }

    public void resetDefaults()
    {
        rs = rotationSpeed;
        ms = movementSpeed;
        delay = 0;
    }

    public void setRotateInput(float rInput)
    {
        rotation = rInput;
    }

    public void setMovementInput(float mInput)
    {
        movement = mInput;
    }

    public void setRotationSpeed(float r)
    {
        rs = r;
    }

    public void setMovementSpeed(float m)
    {
        ms = m;
    }

    public void setDelay(float d)
    {

        delay = d;
    }

    public float getRotateInput()
    {
        return rotation;
    }

    public float getMovementInput()
    {
        return movement;
    }

    public float getRotationSpeed()
    {
        return rs;
    }

    public float getMovementSpeed()
    {
        return ms;
    }

    public float getDelay()
    {
        return delay;
    }
}
