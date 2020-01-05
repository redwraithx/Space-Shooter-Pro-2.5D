using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float _speed = 8.0f;


    void Update()
    {
        CalculateMovement();

        if (transform.position.y >= 8.0f)
        {
            //Destroy(this.transform.root.gameObject); this works for all objects but less used
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void CalculateMovement()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
    }
}
