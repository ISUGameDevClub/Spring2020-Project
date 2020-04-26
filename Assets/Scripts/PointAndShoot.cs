using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAndShoot : MonoBehaviour
{
    //public GameObject crosshairs;
    public GameObject player;
    public GameObject bulletPrefab;
    public GameObject bulletStart;

    public float bulletSpeed = 30.0f;

    bool canShoot = true;

    private Vector3 target;
  
    int currentBullet = 0;
    GameObject[] b = new GameObject[10];
    

    public AudioClip impact;
    AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        for (int n = 0; n < 10; n++)
        {
            GameObject bull = Instantiate(bulletPrefab) as GameObject;
            b[n] = bull;
            b[n].SetActive(false);
        }
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 difference = player.transform.position;
        float rotationZ = player.transform.rotation.y;
        if (currentBullet > 10 - 1)
        {
            currentBullet = 0;
        }
        

        if (Input.GetMouseButtonDown(0))
        {
                float distance = difference.magnitude;
                Vector3 direction = difference / distance;
                direction.Normalize();
                fireBullet(direction, rotationZ);
            
        }
     
    }
    
    void fireBullet(Vector3 direction, float rotationZ)
    {
        if (canShoot == true)
        {

            b[currentBullet].SetActive(true);
            b[currentBullet].transform.position = transform.position + bulletStart.transform.forward * 2;
            b[currentBullet].transform.rotation = Quaternion.Euler(player.transform.rotation.x, player.transform.rotation.y, player.transform.position.z);
            b[currentBullet].GetComponent<Rigidbody>().velocity = direction * bulletSpeed;
            currentBullet++;
            audioSource.PlayOneShot(impact, 1.0F);
            canShoot = false;
        }
        else if(canShoot == false)
        {
            canShoot = true;
        }
    }
   
}

