using UnityEngine;

public class PortalGunController : MonoBehaviour
{
    //private Animator animator;

    //private void Awake()
    //{
    //    animator = GetComponent<Animator>();
    //}

    public void ActivateWeapon()
    {
        gameObject.SetActive(true);  // ✅ Bật PortalGun
    }

    public void DeactivateWeapon()
    {
        gameObject.SetActive(false); // ✅ Tắt PortalGun
    }

    //public void StartPortalAnimation()
    //{
    //    animator.SetTrigger("openPortal"); // ✅ Kích hoạt animation mở cổng
    //}
}
