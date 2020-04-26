using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthFade : MonoBehaviour
{
    public Health health;

    // Start is called before the first frame update
    void Start()
    {
        health = FindObjectOfType<PlayerController>().gameObject.GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        //CHANGE THIS IF ANNOYING
        GetComponent<RawImage>().color = new Vector4(0, 0, 0, 1 - health.currentHealth * .01f);
    }
}
