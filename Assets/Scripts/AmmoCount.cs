using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoCount : MonoBehaviour
{

    public Gun gun;


    // Start is called before the first frame update
    void Start()
    {
        gun = FindObjectOfType<PlayerController>().gameObject.GetComponent<Gun>();
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Text>().text = gun.ammo.ToString();
    }
}
