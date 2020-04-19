using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, -1, 0) * Time.deltaTime * 3f);

        if (transform.position.y < -7f)
        {
            transform.position = new Vector3(Random.Range(-10f, 10f), 7f, 0);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Bullet(Clone)")
        {
            Destroy(other.gameObject);
            //Player earns a score
            Destroy(this.gameObject);
        }
        else if (other.name == "Player")
        {
            //Player (Game Object) has a script component which is called PlayerBehavior and that script has a method GetDamaged()
            GameObject.Find("Player").GetComponent<PlayerBehavior>().GetDamaged();
            Destroy(this.gameObject);
        }
    }
}
