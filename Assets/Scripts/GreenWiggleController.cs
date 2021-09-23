using UnityEngine;
using System.Collections;

public class GreenWiggleController : MonoBehaviour {

	public Transform leftPoint;
	public Transform rightPoint;

	public float moveSpeed;

	private Rigidbody2D myRigidbody;
	public bool movingRight;

	// Use this for initialization
	void Start () {
		myRigidbody = GetComponent<Rigidbody2D> ();

	
	}
	
	// Update is called once per frame
	void Update () {
		if (movingRight && transform.position.x > rightPoint.position.x)     //x-axis positive line moving
		{
			movingRight = false;
		}

		if (!movingRight && transform.position.x < leftPoint.position.x) {    //x-axis negative side moving

			movingRight = true;
		}


		//movement conditions

		if (movingRight) {
			myRigidbody.velocity = new Vector3 (moveSpeed, myRigidbody.velocity.y, 0f);

		} else {
			myRigidbody.velocity = new Vector3 (-moveSpeed, myRigidbody.velocity.y, 0f);
		}
	}
}
