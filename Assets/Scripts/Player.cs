using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player's Speed Attributes")]
    [SerializeField] private float _currentSpeed;
    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private float _speedBoost = 2.5f;
    

    [Header("Player's GameObject references")]
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleLaserPrefab;
    [SerializeField] private GameObject _shieldVisualizer;
    [SerializeField] private GameObject _uiScoreTextManager;
    private SpawnManager _spawnManager;
    [SerializeField] private GameObject _leftEngineDamageObject;
    [SerializeField] private GameObject _rightEngineDamageObject;
    [SerializeField] private GameObject _explosionPrefab;


    [Header("Player's Audio references")]
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _engineHitAudioClip;
    [SerializeField] private AudioClip _laserAudioClip;
    [SerializeField] private float _laserAudioPitch = 4f;
    private Transform _cameraPosition;


    [Header("Player's Attack Attributes")]
    [SerializeField] private float _fireRate = 0.15f;
    private float _offset = 1.05f;
    private float _canFire = -1f;


    [Header("Player's PowerUp's")]
    [SerializeField] private bool _isTripleShotActive = false;
    [SerializeField] private bool _isSpeedBoostActive = false;
    [SerializeField] private bool _isShieldActive = false;


    [Header("Player's Lives")]
    [SerializeField] private int _lives = 3;
    [SerializeField] private int _enginesDamaged = 0; // 0 = no engines are damaged, 1 = 1 engine is damaged, 2 = both engines are damaged
    [SerializeField] private bool _leftEngineDamaged = false;
    [SerializeField] private bool _rightEngineDamaged = false;
    


    [Header("Player's Score")]
    [SerializeField] private int _score = 0;
    [SerializeField] UIManager _uiManager;


    enum ClipToPlay
    {
        laser = 1,
        triple_Laser

    }

    void Start()
    {
        // take the current position (move player to new position) = new position (0, 0, 0)
        transform.position = new Vector3(0, -1.8f, 0);

        _currentSpeed = _speed;
        _enginesDamaged = 0;
        _leftEngineDamaged = false;
        _rightEngineDamaged = false;
        _leftEngineDamageObject.SetActive(false);
        _rightEngineDamageObject.SetActive(false);

        _audioSource = GameObject.Find("Laser").GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("The laser AudioSource on the AudioManager/Laser is NULL");
        }
        _audioSource.clip = _laserAudioClip;


        _spawnManager = FindObjectOfType<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL");
        }

        _cameraPosition = FindObjectOfType<Camera>().GetComponent<Transform>();
        if (_cameraPosition == null)
        {
            Debug.LogError("Enemies _cameraPosition == null");
        }

    }

    void Update()
    {
        CalculateMovement();

        LaserShotShotAndSound();

    }

    private void LaserShotShotAndSound()
    {
        // shoot laser is slightly higher then normal triple shot
        if (Input.GetKey(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }

        // shot sounds a bit lower then a single laser shot
        if (_audioSource.isPlaying && _isTripleShotActive == true && _laserAudioPitch > 0.1f)
        {
            _audioSource.pitch = 0.75f;
        }
        else if (_audioSource.isPlaying && _isTripleShotActive == false && _laserAudioPitch > 1.7f)
        {
            _audioSource.pitch = 8f;
        }
        else // resets audio
        {
            _audioSource.pitch = 1.0f;
            
        }

        _audioSource.volume = 1.0f;

    }

    private void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(moveDirection * _currentSpeed * Time.deltaTime);
        

        // check Player Bounds
        // clamp the Y position using mathf's clamp function
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.6f, 0), transform.position.z);

        // check players X bounds and setup Screen Wrapping
        if (transform.position.x >= 11f)
        {
            transform.position = new Vector3(-11f, transform.position.y, transform.position.z);
        }
        else if (transform.position.x <= -11f)
        {
            transform.position = new Vector3(11f, transform.position.y, transform.position.z);
        }

    }

    private void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_isTripleShotActive)
        {
            Instantiate(_tripleLaserPrefab, transform.position, Quaternion.identity);
            PlayLaserShotAudio((int)ClipToPlay.triple_Laser);
        }
        else
        {
            Instantiate(_laserPrefab, new Vector3(transform.position.x, transform.position.y + _offset, 0), Quaternion.identity);
            PlayLaserShotAudio((int)ClipToPlay.laser);
        }

    }

    private void PlayLaserShotAudio(int clipToPlay)
    {
        switch (clipToPlay)
        {
            case 1:
                _laserAudioPitch = 1f; // make sure pitch is back to normal
                break;
            case 2:
                _audioSource.pitch = _laserAudioPitch;
                break;
            default:
                Debug.LogError("Error with autoClip to play number: " + clipToPlay);
                break;
        }
        _audioSource.Play();

    }

    public void Damage()
    {
        // players shield can take one hit then deactivates
        if (_isShieldActive)
        {
            _isShieldActive = false;

            _shieldVisualizer.SetActive(false);

            return;
        }

        _lives--;

        EngineDamageUpdate();

        _uiManager.UpdateLives(_lives);


        if (_lives <= 0)
        {

            _spawnManager.OnPlayersDeath(transform.position);

            Destroy(this.gameObject);
        }

    }

    private void EngineDamageUpdate()
    {
        _enginesDamaged++;
        switch (_enginesDamaged)
        {
            case 0:
                // no engine damage;
                break;
            case 1:
                if (UnityEngine.Random.Range(1, 3) == 1)
                {
                    _leftEngineDamaged = true;
                    _leftEngineDamageObject.SetActive(true);
                }
                else
                {
                    _rightEngineDamaged = true;
                    _rightEngineDamageObject.SetActive(true);
                }

                AudioSource.PlayClipAtPoint(_engineHitAudioClip, transform.position, 1f);

                break;
            case 2:
                if (_leftEngineDamaged)
                {
                    _rightEngineDamaged = true;
                    _rightEngineDamageObject.SetActive(true);
                }
                else
                {
                    _leftEngineDamaged = true;
                    _leftEngineDamageObject.SetActive(true);
                }

                AudioSource.PlayClipAtPoint(_engineHitAudioClip, transform.position, 1f);

                break;
            default:
                break;
        }

    }

    public void TripleShotActive()
    {
        if (!_isTripleShotActive)
        {
            _isTripleShotActive = true;
        }
        else
        {
            return;
        }

        StartCoroutine(TripleShotPowerDownRoutine());

    }


    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);

        _isTripleShotActive = false;

    }

    public void SpeedPowerupActive()
    {
        if(!_isSpeedBoostActive)
        {
            _isSpeedBoostActive = true;

            // update current movement speed
            _currentSpeed = _speed * _speedBoost;
        }
        else
        {
            return;
        }

        StartCoroutine(SpeedPowerDownRoutine());

    }

    IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);

        _isSpeedBoostActive = false;

        // update current movement speed
        _currentSpeed = _speed;

    }

    public void ShieldPowerupActive()
    {
        if (!_isShieldActive)
        {
            _isShieldActive = true;

            _shieldVisualizer.SetActive(true);
        }
        else
        {
            return;
        }

    }

    public void UpdatePlayerScore(int pointsAwarded)
    {
        // update players score
        _score += pointsAwarded;

        UpdateScoreUI();

    }


    private void UpdateScoreUI()
    { 
        // send current score to the UI
        _uiManager.UpdateScore(_score);

    }

}
