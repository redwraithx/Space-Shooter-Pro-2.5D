using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private float _speed = 3.0f;

    [SerializeField] private int powerupID; // ID for powerups: 0 = triple shot, 1 = speed, 2 = shields

    [SerializeField] private AudioClip _pickupAudioClip;


    void Update()
    {
        PowerupMovement();

        if (transform.position.y < -4.5f)
        {
            Destroy(this.gameObject);
        }

    }

    private void PowerupMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.name == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_pickupAudioClip, transform.position);

            if (player != null)
            {
                // check powerup ID's with switch
                switch (powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedPowerupActive();
                        break;
                    case 2:
                        player.ShieldPowerupActive();
                        break;

                    default:
                        Debug.LogError($"Error with powerupID, currently {powerupID} gives an error");
                        break;
                }
            }

            Destroy(this.gameObject);
        }

    }

}
