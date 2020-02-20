using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	Vector3 respawnPoint; //where to respawn
	// Use this for initialization
	void Start () {
		respawnPoint = transform.position; //save player’s starting location
	}
	public void Respawn() {
		this.gameObject.transform.position = respawnPoint; //go back to start
	}
	public void SetRespawnPoint( Vector3 newSpawnPoint)
	{
		respawnPoint = newSpawnPoint; //set the new spawn location
	}
	// Update is called once per frame
	void Update () {
	
	}
}
