using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    private float _speed = 10f;


    void Update()
    {
        if (transform.position.y < -5)
        {
            Destroy(this.gameObject);
        }
        
        transform.Translate(Vector3.down * Time.deltaTime * _speed);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //HitPlayerWithoutTags(other);
        HitPlayerWithTags(other);
    }

    private void HitPlayerWithTags(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Player>().Damage();

            Destroy(this.gameObject);
        }

    }

    private void HitPlayerWithoutTags(Collider2D other)
    {
        if (other.transform.name == "Player")
        {
            other.gameObject.GetComponent<Player>().Damage();

            Destroy(this.gameObject);

        }
    }

}
