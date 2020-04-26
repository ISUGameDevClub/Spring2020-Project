using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletGameObject;
    public GameObject bulletSpawn;

    public int maxAmmo;
    public int reloadTime;
    bool isPlayer;
    bool canFire;
    public int ammo;
    public float secondaryFireSpread;
    public float rateOfFire = .5f;
    private Coroutine enemyFire;

    Coroutine currentReloadCoroutine;

    private void Start()
    {
        ammo = maxAmmo;
        canFire = true;
        isPlayer = GetComponent<PlayerController>() != null;
    }


    void Update()
    {
        if (isPlayer && !FindObjectOfType<PauseMenu>().gamePaused)
        {
            if (Input.GetMouseButtonDown(0) && canFire && ammo > 0)
            {
                Fire();
            }

            if (Input.GetKey(KeyCode.R))
            {
                currentReloadCoroutine = StartCoroutine(reload());
            }

            //shotgun 
            if (Input.GetMouseButtonDown(1) && canFire && ammo > 0)
            {
                SecondaryFire();
            }
        }
    }

    public void allowFire()
    {
        canFire = true;
    }

    public void disAllowFire()
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

    public void Fire()
    {
        ammo--;
        Bullet bull = Instantiate(bulletGameObject, bulletSpawn.transform).GetComponent<Bullet>();
        bull.gameObject.transform.parent = null;
        bull.gameObject.transform.localScale = new Vector3(.05f, .05f, .5f);
    }

    //shot
    void SecondaryFire()
    {
        while (ammo > 0)
        {

            ammo--;
            Bullet bull = Instantiate(bulletGameObject, bulletSpawn.transform).GetComponent<Bullet>();
            bull.gameObject.transform.parent = null;
            bull.gameObject.transform.localScale = new Vector3(.05f, .05f, .5f);
            bull.gameObject.transform.localEulerAngles = new Vector3(bull.gameObject.transform.localEulerAngles.x + Random.Range(-secondaryFireSpread, secondaryFireSpread), bull.gameObject.transform.localEulerAngles.y + Random.Range(-secondaryFireSpread, secondaryFireSpread), bull.gameObject.transform.localEulerAngles.z);
        }

    }

    public IEnumerator delay()
    {
        yield return new WaitForSeconds(.5f);
        Fire();
        enemyFire = null;
    }

    public void EnemyFire()
    {
        if (enemyFire == null)
        {
            enemyFire = StartCoroutine(delay());
        }
    }
}
