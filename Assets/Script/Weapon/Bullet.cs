using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody rb;
    float damage;
    Vector3 playerCam_centerPos;
    Quaternion shotDir;
    float bulletSpeed;
    public void Shot(float atk, float speed)
    {
        damage =atk;
        bulletSpeed =speed*10;
        rb.velocity = transform.forward * bulletSpeed;

    }
    private void FixedUpdate()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log($"ê⁄êGÇµÇΩìGÇÃñºëO{other.gameObject.name}");
            other.GetComponent<EnemyController>().OnDamage(damage);
        }
        Destroy(gameObject);
    }
}
