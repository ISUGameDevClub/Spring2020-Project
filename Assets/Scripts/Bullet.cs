using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool isPlayer;
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
        if (other.gameObject.tag == "Player" && !isPlayer)
        {
            // DEAL DAMAGE TO PLAYER
            other.gameObject.GetComponent<Health>().recieveDamage(damage);

        }
        else if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<Health>().recieveDamage(damage);
        }

        if (other.gameObject.tag != "Bullet")
        {
            Destroy(gameObject);
        }
    }

    IEnumerator DespawnBullet()
    {
        yield return new WaitForSeconds(despawnTime);
        Destroy(gameObject);
    }
}
