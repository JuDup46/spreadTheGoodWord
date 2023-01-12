using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    waiting,
    playing,
    dying
}

public class GameManager : MonoBehaviour
{
    public GameObject UiTitle;

    public GameObject UiAuthor;

    public GameObject UiBtnPlay;

    public static GameManager Instance { get; private set; }

    public Player player;

    public Ennemy ennemy;

    public ParticleSystem explosion;

    public int lives = 3;

    public float respawnInVulnerabilityTime = 3.0f;

    public float respawnTime = 3.0f;

    const float delayBeforeUI = 3f;

    public GameState gameState;

    public AudioSource audioSource;

    public AudioClip soundBurst;




    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

   

    private void Start()
    {
        gameState = GameState.waiting;
    }

    public void LaunchGame()
    {
        Invoke(nameof(Respawn), this.respawnTime);

        UiTitle.SetActive(false);
        UiAuthor.SetActive(false);
        UiBtnPlay.SetActive(false);

        gameState = GameState.playing;
    }

    public void EnnemyDestroyed(Ennemy ennemy)
    {
        AudioSource.PlayClipAtPoint(soundBurst, transform.position);
        this.explosion.transform.position = ennemy.transform.position;
        this.explosion.Play();

        scoreManager.instance.AddPoint(ennemy.size);
    }


    public void PlayerDied()
    {
        AudioSource.PlayClipAtPoint(soundBurst, transform.position);
        this.explosion.transform.position = this.player.transform.position;
        this.explosion.Play();
        this.lives--;

        if (this.lives <= 0)
        {
            GameOver();
        }
        else
        {
            Invoke(nameof(Respawn), this.respawnTime);
        }
        

    }

    private void Respawn()
    {
        this.player.transform.position = Vector3.zero;
        this.player.gameObject.layer = LayerMask.NameToLayer("Ignore Collisions");
        this.player.gameObject.SetActive(true);


        Invoke(nameof(TurnOnCollisions), this.respawnInVulnerabilityTime);
    }

    private void TurnOnCollisions()
    {
        this.player.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private void GameOver()
    {
        
        StartCoroutine(nameof(GameOver2));
        this.lives = 3;
        

    }



    IEnumerator GameOver2()
    {
        yield return new WaitForSeconds(delayBeforeUI);

        allEnnemyDestroyed();
        scoreManager.instance.zeroPoint();
        UiTitle.SetActive(true);
        UiAuthor.SetActive(true);
        UiBtnPlay.SetActive(true);

        gameState = GameState.waiting;
    }

    private void allEnnemyDestroyed()
    {

        

        var test = GameObject.FindGameObjectsWithTag("Ennemy");
        foreach (var ennemyTag in test)
        {
            AudioSource.PlayClipAtPoint(soundBurst, transform.position);
            Destroy(ennemyTag);
        }
    }
}
