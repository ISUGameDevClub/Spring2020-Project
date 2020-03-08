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
        Destroy(gameObject);
    }

    IEnumerator DespawnBullet()
    {
        yield return new WaitForSeconds(despawnTime);
        Destroy(gameObject);
    }
}
