using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy : MonoBehaviour
{

    public Sprite[] sprites;

    public float size = 1.0f;

    public float minSize = 0.5f;

    public float maxSize = 1.5f;

    public float speed = 50.0f;

    public float maxLifeTime = 30.0f;

    private SpriteRenderer _spriteRenderer;

    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        if (GameManager.Instance.gameState != GameState.playing) return;

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();

    }
    // Start is called before the first frame update
    private void Start()
    {
        if (GameManager.Instance.gameState != GameState.playing) return;

        _spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];

        this.transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value * 360.0f);
        this.transform.localScale = Vector3.one * this.size;

        _rigidbody.mass = this.size * 2.0f;
    }

    public void SetTrajectory(Vector2 direction)
    {
        if (GameManager.Instance.gameState != GameState.playing) return;

        _rigidbody.AddForce(direction * this.speed);

        Destroy(this.gameObject, this.maxLifeTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameManager.Instance.gameState != GameState.playing) return;

        if (collision.gameObject.tag == "Bullet")
        {
            if ((this.size * 0.5f) >= this.minSize)
            {
                CreateSplit();
                CreateSplit();
            }
            FindObjectOfType<GameManager>().EnnemyDestroyed(this);
            Destroy(this.gameObject);
        }

    }

    private void CreateSplit()
    {
        if (GameManager.Instance.gameState != GameState.playing) return;

        Vector2 position = this.transform.position;
        position += Random.insideUnitCircle * 0.5f;

        Ennemy half = Instantiate(this, position, this.transform.rotation);

        half.size = this.size * 0.5f;

        half.SetTrajectory(Random.insideUnitCircle.normalized);
    }


}


