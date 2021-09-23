using UnityEngine;
using System.Collections;

public class DestroyOverTime : MonoBehaviour {

	public float lifeTime;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		lifeTime = lifeTime - Time.deltaTime;    //lifeTime gets zero by time

		if(lifeTime <= 0f)
		{
			Destroy (gameObject);
		}
	}
}
