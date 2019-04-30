using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//good
public class PointControl : MonoBehaviour {

    private int score;

    private bool hasPoint;

    public GameObject point1, point2;
    public GameObject drop1, drop2;

    public int playerNum;

    private GameObject point, dropoff;

    private Rigidbody rb;

	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody>();

        if (playerNum==1)
        {
            point = point1;
            dropoff = drop1;
        }
        else
        {
            point = point2;
            dropoff = drop2;
        }

        score = 0;
        hasPoint = false;
        setActives();
    }

    //update is called once per frame
    void Update()
    {
        setActives();
    }

    void OnTriggerEnter(Collider other)
    {
        
        //award a point if you have a point and are in the rigth base
        if (other.Equals(dropoff.GetComponent<Collider>()) && hasPoint == true)
        {
            score++;
            hasPoint = false;
        }

        //if you dont have a point and are in the right goal pick one up
        else if (other.Equals(point.GetComponent<Collider>()) && hasPoint == false)
        {
            hasPoint = true;
        }
    }

    public int getScore()
    {
        return score;
    }

    public void setHasPoint(bool p)
    {
        hasPoint = p;
    }

    public bool getHasPoint()
    {
        return hasPoint;
    }

    private void setActives()
    {
        point.SetActive(!hasPoint);
        dropoff.SetActive(hasPoint);
    }

}
