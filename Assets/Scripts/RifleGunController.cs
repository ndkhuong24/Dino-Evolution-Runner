﻿using System.Collections;
using UnityEngine;

public class RifleGunController : MonoBehaviour
{
    private Animator animator;
    public Transform firePoint;
    public GameObject ammoPrefab;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ActivateWeapon()
    {
        gameObject.SetActive(true);
    }

    public void DeactivateWeapon()
    {
        gameObject.SetActive(false);
    }

    public void ShootAnimation()
    {
        animator.SetBool("isShooting", true);
    }

    public void ResetShootAnimation()
    {
        animator.SetBool("isShooting", false);
    }

    public void FireAmmo()
    {
        StartCoroutine(SpawnBulletAfterFrame());
    }

    private IEnumerator SpawnBulletAfterFrame()
    {
        yield return new WaitForEndOfFrame();

        GameObject ammo = ObjectPool.Instance.GetObject(ammoPrefab);
        ammo.transform.position = firePoint.position;
        ammo.transform.rotation = firePoint.rotation;
    }

    //private IEnumerator SpawnBulletAfterFrame()
    //{
    //    yield return new WaitForEndOfFrame();
    //    Instantiate(ammoPrefab, firePoint.position, firePoint.rotation);
    //}
}