using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Weapon;

public class SounManager : MonoBehaviour
{
    public static SounManager Instance{get; set;}

    public AudioSource ShootingChannel;

    public AudioSource emptyMagazineSound1911;

    public AudioClip P1911_Shot;

    public AudioClip M4_8Shot;

    public AudioSource reloadingSound1911;
    public AudioSource reloadingSoundM4_8;
    public AudioSource throwableChannel;
    public AudioClip grenadeSound;

    public AudioClip zombieWalking;
    public AudioClip zombieChase;
    public AudioClip zombieAttack;
    public AudioClip zombieHurt;
    public AudioClip zombieDeath;

    public AudioSource zombieChannel;
    public AudioSource zombieChannel2;
    public AudioSource playerChannel;
    public AudioClip playerHurt;
    public AudioClip playerDie;

    private void Awake(){
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlayShootingSound(WeaponModel weapon)
    {
        switch(weapon)
        {
            case WeaponModel.Pistol1911:
                ShootingChannel.PlayOneShot(P1911_Shot);
                break;
            case WeaponModel.M4_8:
                ShootingChannel.PlayOneShot(M4_8Shot);
                break;
        }

    }

    public void PlayReloadSound(WeaponModel weapon)
    {
                switch(weapon)
        {
            case WeaponModel.Pistol1911:
                reloadingSound1911.Play();
                break;
            case WeaponModel.M4_8:
                reloadingSoundM4_8.Play();
                break;
        }

    }
}
