using UnityEngine;

public class RifleGunController : MonoBehaviour
{
    private Animator animator;

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
}
