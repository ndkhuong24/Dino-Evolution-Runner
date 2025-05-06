using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleGunAmmoMovement : MonoBehaviour
{
    public float speed = 15f;

    void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;

        if (transform.position.x > 10f)
        {
            ObjectPool.Instance.ReturnObject(gameObject);
            //Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Transform parent = other.transform.parent;

            if (parent != null && parent.name == "ObstacleGroup")
            {
                List<Transform> children = new List<Transform>();

                foreach (Transform child in parent)
                {
                    children.Add(child);
                }

                foreach (Transform child in children)
                {
                    child.SetParent(null);
                    ObjectPool.Instance.ReturnObject(child.gameObject);
                }

                if (parent.childCount == 0)
                {
                    Destroy(parent.gameObject);
                }
            }
            else
            {
                ObjectPool.Instance.ReturnObject(other.gameObject);
            }

            ObjectPool.Instance.ReturnObject(gameObject);
            //Destroy(gameObject);
        }
    }

    //private IEnumerator DestroyAfterFrame(GameObject obj)
    //{
    //    yield return new WaitForEndOfFrame();
    //    Destroy(obj);
    //}
}