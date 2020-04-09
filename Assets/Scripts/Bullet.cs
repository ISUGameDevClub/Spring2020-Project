using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float damage;
    public float despawnTime;

    private void Start()
    {
        StartCoroutine(DespawnBullet());
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            // DEAL DAMAGE TO PLAYER
        }
        else if(other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<Health>().recieveDamage(damage);
        }

        Destroy(gameObject);
    }

    IEnumerator DespawnBullet()
    {
        yield return new WaitForSeconds(despawnTime);
        Destroy(gameObject);
    }
}
