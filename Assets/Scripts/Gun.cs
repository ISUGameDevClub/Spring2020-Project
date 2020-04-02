using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletGameObject;
    public GameObject bulletSpawn;

    public int maxAmmo;
    public int reloadTime;
    bool canFire;
    int ammo;

    Coroutine currentReloadCoroutine;

    private void Start()
    {
        ammo = maxAmmo;
        canFire = true;
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canFire && ammo > 0)
        {
            Fire();
        }

        if (Input.GetKey(KeyCode.R))
        {
            currentReloadCoroutine = StartCoroutine(reload());
        }
    }

    void allowFire()
    {
        canFire = true;
    }

    void disAllowFire()
    {
        canFire = false;
    }

    IEnumerator reload()
    {
        disAllowFire();
        yield return new WaitForSeconds(reloadTime);
        ammo = maxAmmo;
        allowFire();
    }

    void stopReload()
    {
        if(currentReloadCoroutine != null)
        {
            StopCoroutine(currentReloadCoroutine);
        }
    }

    void Fire()
    {
        ammo--;
        Bullet bull = Instantiate(bulletGameObject, bulletSpawn.transform).GetComponent<Bullet>();
        bull.gameObject.transform.parent = null;
        bull.gameObject.transform.localScale = new Vector3(.05f, .05f, .5f);
    }
}
