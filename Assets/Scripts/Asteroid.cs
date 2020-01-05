using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _rotateSpeedmultiplier = 4.0f;
    [SerializeField] private float _rotateSpeed = 5.0f;

    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private SpawnManager _spawnManager;

    void Start()
    {
        //_spawnManager = GetComponent<SpawnManager>();
        

        // possibly of a reversed rotation
        if (Random.Range(1, 5) > 2)
        {
            _rotateSpeedmultiplier *= -1;
        }

    }

    void Update()
    {
        transform.Rotate(0, 0, (_rotateSpeed * _rotateSpeedmultiplier) * Time.deltaTime, Space.World);

    }

    // I avoided using tags, i have had issues where a tag was missing and this error didnt tell me that a tag 
    // was missing, alot of hours went into this error. I can update it and use tags if required.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Laser(Clone)" || other.transform.root.name == "Triple_Shot(Clone)")
        {
            if (other.transform.parent != null)
            {
                Destroy(other.transform.parent.gameObject);
            }
            else
            {
                Destroy(other.gameObject);
            }

            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);

            // Game starts after the asteroid has exploded
            _spawnManager.StartSpawning();

            Destroy(this.gameObject, 0.08f);
        }

    }

}
