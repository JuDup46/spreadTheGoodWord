using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Bullet bulletPrefab;

    public float thrustSpeed = 1.0f;

    public float turnSpeed = 1.0f;

    private Rigidbody2D _rigidbody;

    private bool _thrusting;

    private float _turnDirection;

    public AudioSource audioSource;

    public AudioClip soundShoot;



    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (GameManager.Instance.gameState != GameState.playing) return;

        _thrusting = Input.GetKey(KeyCode.UpArrow);

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _turnDirection = 1.0f;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            _turnDirection = -1.0f;
        }
        else
        {
            _turnDirection = 0.0f;
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.gameState != GameState.playing) return;

        if (_thrusting)
        {
            _rigidbody.AddForce(this.transform.up * thrustSpeed);
        }

        if (_turnDirection != 0)
        {
            _rigidbody.AddTorque(_turnDirection * this.turnSpeed);
        }

    }


    private void Shoot()
    {
        audioSource.PlayOneShot(soundShoot);
        Bullet bullet = Instantiate(this.bulletPrefab, this.transform.position, this.transform.rotation);
        bullet.Project(this.transform.up);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameManager.Instance.gameState != GameState.playing) return;

        if (collision.gameObject.tag == "Ennemy")
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = 0.0f;

            this.gameObject.SetActive(false);

            FindObjectOfType<GameManager>().PlayerDied();
        }
    }

}
