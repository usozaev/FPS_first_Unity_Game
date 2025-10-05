using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool isActiveWeapon;
    public int weaponDamage;

    [Header("Shooting")]
    //Shooting
    public bool  isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 2f;

    [Header("Burst")]
    // Burst
    public int bulletsPerBurst = 3;
    public int burstBulletsLeft;

    [Header("Spread")]
    //Spread
    public float spreadIntensity;
    public float hipSpreadIntensity;
    public float adsSpreadIntensity;




    [Header("Bullet")]
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 100;
    public float bulletPrefabLifetime = 3f;

    public GameObject muzzleEffect;
    internal Animator animator;

    [Header("Reloading")]

    //Reloading

    public float reloadTime;
    public int magazineSize, bulletsLeft;
    public bool isReloading;

    public Vector3 spawnPosition;
    public Vector3 spawnRotation;

    bool isADS;


    public enum WeaponModel
    {
        Pistol1911,
        M4_8
    }

    public WeaponModel thisWeaponModel;



    public enum ShootingMode
    {
      Single,
      Burst,
      Auto    
    }

    public ShootingMode currentShootingMode;

    private void Awake()
    {
        readyToShoot = true;
        burstBulletsLeft = bulletsPerBurst;
        animator = GetComponent<Animator>();

        bulletsLeft = magazineSize;

        spreadIntensity = hipSpreadIntensity;
    }

    // Update is called once per frame
    void Update()
    {


        if (isActiveWeapon)
        {
            foreach (Transform child in transform)
            {
                gameObject.layer = LayerMask.NameToLayer("WeaponRender");
            }


            if (Input.GetMouseButtonDown(1))
            {
                EnterADS();
            }

            if (Input.GetMouseButtonUp(1))
            {
                ExitADS();
            }



            GetComponent<Outline>().enabled = false;

            if(bulletsLeft == 0 && isShooting)
            {
                SounManager.Instance.emptyMagazineSound1911.Play();
            }
    
            if(currentShootingMode == ShootingMode.Auto)
            {
                isShooting = Input.GetKey(KeyCode.Mouse0);
                Debug.Log(isShooting);
            }
            else if (currentShootingMode == ShootingMode.Single || currentShootingMode == ShootingMode.Burst)
            {
                isShooting = Input.GetKeyDown(KeyCode.Mouse0);
            }
            //Reload
            if (Input.GetKeyDown(KeyCode.R)&& bulletsLeft < magazineSize && isReloading == false && WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel) > 0)
            {
                Reload();
            }
    
            // if autoamtic reload
            if(readyToShoot && isShooting == false && isReloading == false && bulletsLeft <= 0)
            {
                //Reload();
            }
    
    
            if(readyToShoot && isShooting && bulletsLeft > 0)
            {
                burstBulletsLeft = bulletsPerBurst;
                FireWeapon();
            }
    
        }


    }


    private void EnterADS()
    {
        animator.SetTrigger("enterADS");
        isADS = true;
        spreadIntensity = adsSpreadIntensity;
        HUDManager.Instance.middleDot.SetActive(false);
    }

    private void ExitADS()
    {
        animator.SetTrigger("exitADS");
        isADS = false;
        HUDManager.Instance.middleDot.SetActive(true);
        spreadIntensity = hipSpreadIntensity;
    }

    private void FireWeapon()
    {

        bulletsLeft --;

        muzzleEffect.GetComponent<ParticleSystem>().Play();

        if(isADS)
        {
            animator.SetTrigger("RECOIL_ADS");
        }
        else
        {
          animator.SetTrigger("RECOIL");  
        }
        
    

        //SounManager.Instance.shootingSound1911.Play();

        SounManager.Instance.PlayShootingSound(thisWeaponModel);

        readyToShoot = false;
        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        GameObject bullet =  Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        Bullet bul = bullet.GetComponent<Bullet>();
        bul.bulletDamage = weaponDamage;

        bullet.transform.forward = shootingDirection;

        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection*bulletVelocity, ForceMode.Impulse);
        // Destroy the bullet
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifetime));

        // Check if we are shooting
        if(allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }
        // Check Burst
        if(currentShootingMode == ShootingMode.Burst && burstBulletsLeft >1)
        {
            burstBulletsLeft--;
            Invoke("FireWeapon", shootingDelay);
        }

    }


    private void Reload()
    {
        
        SounManager.Instance.PlayReloadSound(thisWeaponModel);

        animator.SetTrigger("RELOAD");

        isReloading = true;
        Invoke("ReloadCompleted", reloadTime);
    }


    private void ReloadCompleted()
    {
      if (WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel) > magazineSize)
      {
        bulletsLeft = magazineSize;
        WeaponManager.Instance.DecreaseTotalAmmo(bulletsLeft, thisWeaponModel);
      }
      else
      {
        bulletsLeft = WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel);
        WeaponManager.Instance.DecreaseTotalAmmo(bulletsLeft, thisWeaponModel);
      }
      isReloading = false;
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    public Vector3 CalculateDirectionAndSpread()
    {
        // Shooting from the middle of the screen
       Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
       RaycastHit hit;

       Vector3 targetPoint;
       if(Physics.Raycast(ray, out hit))
       {
        targetPoint = hit.point;
       }
       else
       {
        targetPoint = ray.GetPoint(100);
       }

       Vector3 direction = targetPoint - bulletSpawn.position;

       float z = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
       float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

       return direction + new Vector3(0,y,z);

    }
    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return  new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
