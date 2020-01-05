using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{ 
    [Header("Enemies Basic Information")]
    [SerializeField] private float _speed = 4f;
    private Player _player;
    [SerializeField] private int _scoreValue;
    [SerializeField] private bool _isEnemyDead = false;


    [Header("Enemies Shooting References")]
    [SerializeField] private GameObject _enemyLaserPrefab;
    private int _fireMinSeconds = 2;
    private int _fireMaxSeconds = 6;
    [SerializeField] private float _fireRate = 1f;
    [SerializeField] private float _canFire = -1f;
    private float _shotOffset = -0.648f;


    private AudioSource _laserAudioSource;
    private float _laserAudioClipPitch = 5.5f;
    private float _laserPitchShiftdown = 2.5f;


    private Animator _animator;


    private void Start()
    {
        _isEnemyDead = false;

        _animator = gameObject.GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("_animator is null on enemy");
        }

        _player = GameObject.FindObjectOfType<Player>();
        if (_player == null)
        {
            Debug.LogError("the player is NULL on enemy");
        }

        _scoreValue = 10;

        // each enemy will have different shot speeds
        _fireRate = UnityEngine.Random.Range(_fireMinSeconds, _fireMaxSeconds);
        _canFire = -1;



        _laserAudioSource = GetComponent<AudioSource>();
        if (_laserAudioSource == null)
        {
            Debug.LogError("enemies _laserAudioSource is NULL");
        }
        _laserAudioSource.pitch = 1f;

    }

    void Update()
    {
        EnemyMovement();

        EnemyShootMechanics();

    }

    private void EnemyShootMechanics()
    {
        if (Time.time > _canFire && _isEnemyDead == false)
        {
            _canFire = Time.time + _fireRate;

            Instantiate(_enemyLaserPrefab, new Vector3(transform.position.x, transform.position.y + _shotOffset, transform.position.z), Quaternion.identity);

            _laserAudioSource.pitch = 1f;
            _laserAudioSource.Play();
        }

        // audio variance
        if (_laserAudioSource.pitch > 0.5 && _laserAudioSource.isPlaying)
        {
            _laserAudioSource.pitch -= Time.deltaTime * _laserAudioClipPitch / _laserPitchShiftdown;
        }
        else
        {
            _laserAudioSource.pitch = 1.0f;
        }

    }

    private void EnemyMovement()
    {
        if (_isEnemyDead == false)
        {
            // move down at 4 meters per second
            transform.Translate(Vector3.down * _speed * Time.deltaTime);

            // if botton of screen 
            // (respawn)move to top of screen at a random X position
            if (transform.position.y <= -5f)
            {
                float randomX = Random.Range(-8.5f, 8.5f);
                transform.position = new Vector3(randomX, 8f, transform.position.z);
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.name == "Player")
        {
            _player.Damage();

            OnEnemyDeath();
        }

        // could use tags but this can cause issues if the tag is removed, but wont indicate that its an issue
        if (other.gameObject.name == "Laser(Clone)")
        {
            Destroy(other.gameObject);

            // add scoreValue to players score
            if (_player != null)
            {
                _player.UpdatePlayerScore(_scoreValue);
            }

            OnEnemyDeath();

        }

        // triple shot collisions (transform.root.name is used cause this object has a parent and the shots are child objects)
        // as well as i have had issues with tags in the past
        //HitOtherWithoutTags(other);

        HitOtherWithTags(other);
        

    }

    private void HitOtherWithTags(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            _player.Damage();

            OnEnemyDeath();
        }

        // could use tags but this can cause issues if the tag is removed, but wont indicate that its an issue
        if (other.gameObject.tag == "Laser")
        {
            Destroy(other.gameObject);

            // add scoreValue to players score
            if (_player != null)
            {
                _player.UpdatePlayerScore(_scoreValue);
            }

            OnEnemyDeath();

        }

        if (other.gameObject.tag == "Triple_Shot")
        {
            if (other.transform.parent != null)
            {
                Destroy(other.transform.parent.gameObject);
            }
            else
            {
                Destroy(other.gameObject);
            }

            // add scoreValue to players score
            if (_player != null)
            {
                _player.UpdatePlayerScore(_scoreValue);
            }

            OnEnemyDeath();
        }
    }

    private void HitOtherWithoutTags(Collider2D other)
    {
        if (other.transform.name == "Player")
        {
            _player.Damage();

            OnEnemyDeath();
        }

        // could use tags but this can cause issues if the tag is removed, but wont indicate that its an issue
        if (other.gameObject.name == "Laser(Clone)")
        {
            Destroy(other.gameObject);

            // add scoreValue to players score
            if (_player != null)
            {
                _player.UpdatePlayerScore(_scoreValue);
            }

            OnEnemyDeath();

        }

        if (other.transform.root.name == "Triple_Shot(Clone)")
        {
            if (other.transform.parent != null)
            {
                Destroy(other.transform.parent.gameObject);
            }
            else
            {
                Destroy(other.gameObject);
            }

            // add scoreValue to players score
            if (_player != null)
            {
                _player.UpdatePlayerScore(_scoreValue);
            }

            OnEnemyDeath();
        }
    }

    private void OnEnemyDeath()
    {
        _isEnemyDead = true;

        Destroy(this.gameObject.GetComponent<Collider2D>());

        _animator.SetTrigger("OnEnemyDeath");
        _speed = 0.0f;

        Destroy(this.gameObject, 2.8f); // delay death until animation is complete

    }

}
