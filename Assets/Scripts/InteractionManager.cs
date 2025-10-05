using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance{get; set;}

    public Weapon hoveredWeapon = null;
    public AmmoBox hoveredAmmoBox = null;

    public Throwable hoveredThrowable = null;

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
    private void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f,0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            GameObject objectHitByRaycast = hit.transform.gameObject;
            if(objectHitByRaycast.GetComponent<Weapon>() && objectHitByRaycast.GetComponent<Weapon>().isActiveWeapon == false)
            {
                // Disable outline of previously hovered item //
                if (hoveredWeapon)
                {
                    hoveredWeapon.GetComponent<Outline>().enabled = false;
                }

                hoveredWeapon = objectHitByRaycast.gameObject.GetComponent<Weapon>();
                hoveredWeapon.GetComponent<Outline>().enabled = true;
                if(Input.GetKeyDown(KeyCode.F))
                {
                    WeaponManager.Instance.PickupWeapon(objectHitByRaycast.gameObject);
                }
            }
            else
            {
                if(hoveredWeapon)
                {
                    hoveredWeapon.GetComponent<Outline>().enabled = false;
                }
            }

            
            //AmmoBox
            if(objectHitByRaycast.GetComponent<AmmoBox>())
            {
                // Disable outline of previously hovered item //
                if (hoveredAmmoBox)
                {
                    hoveredAmmoBox.GetComponent<Outline>().enabled = false;
                }


                hoveredAmmoBox = objectHitByRaycast.gameObject.GetComponent<AmmoBox>();

                hoveredAmmoBox.GetComponent<Outline>().enabled = true;
                if(hoveredAmmoBox.GetComponent<Outline>() != null)
                {
                   //hoveredAmmoBox.GetComponent<Outline>().enabled = true;
                    if(Input.GetKeyDown(KeyCode.F))
                    {
                        WeaponManager.Instance.PickupAmmo(hoveredAmmoBox);
                        //Destroy(objectHitByRaycast.gameObject);
                    }
                }
            }
            else
            {
                if(hoveredAmmoBox.GetComponent<Outline>() != null)
                {
                    hoveredAmmoBox.GetComponent<Outline>().enabled = false;
                }
            }


            //Throwable
            if(objectHitByRaycast.GetComponent<Throwable>())
            {
                print("HitGreande");
                // Disable outline of previously hovered item //


                hoveredThrowable = objectHitByRaycast.gameObject.GetComponent<Throwable>();
                hoveredThrowable.GetComponent<Outline>().enabled = true;
                if(Input.GetKeyDown(KeyCode.F))
                {
                    WeaponManager.Instance.PickupThrowable(hoveredThrowable);
                }
            }
            else
            {
                if(hoveredThrowable.GetComponent<Outline>() != null)
                {
                    hoveredThrowable.GetComponent<Outline>().enabled = false;
                }
            }

            
            }
        }
    }

