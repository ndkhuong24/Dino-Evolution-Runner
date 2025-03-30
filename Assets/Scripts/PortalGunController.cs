using System;
using System.Collections;
using UnityEngine;

public class PortalGunController : MonoBehaviour
{
    public Transform firePoint;
    public GameObject ammoPrefab;

    public void ActivateWeapon()
    {
        gameObject.SetActive(true);  
    }

    public void DeactivateWeapon()
    {
        gameObject.SetActive(false); 
    }

    internal void FireAmmo()
    {
        StartCoroutine(SpawnBulletAfterFrame());
    }

    private IEnumerator SpawnBulletAfterFrame()
    {
        yield return new WaitForEndOfFrame();
        Instantiate(ammoPrefab, firePoint.position, firePoint.rotation);
    }
}
