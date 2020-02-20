using UnityEngine;
using System.Collections;

public class Respawn : MonoBehaviour {
	//start location (x,y,z)
	public Vector3 startPosition;
	// Use this for initialization
	void Start () {

	}
	void OnTriggerEnter(Collider otherThing)
	{
		//put the player back to the start location
		otherThing.transform.position = startPosition;
	}
	// Update is called once per frame
	void Update () {
	
	}
}
