using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private float fireRate = 0.15f;
    [SerializeField] private float canFire = -1f;
    [SerializeField] private double lives = 3;
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private bool isTripleShotActive = false;
    [SerializeField] private bool isShieldActive = false;
    [SerializeField] private GameObject tripleShotPrefab;
    [SerializeField] private GameObject shieldVisualizer;
    [SerializeField] private int score = 0;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private GameObject leftEngine, rightEngine;
    [SerializeField] private AudioClip laserShootClip;
    [SerializeField] private Animator _animator;
    private AudioSource _audioSource, _audioManager;

    private void Start()
    {
        transform.Translate(new Vector3(0, 0, 0));
        spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioManager = GameObject.Find("ExplosionMusic").GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = laserShootClip;
    }

    private void Update()
    {
        CalculateMovement();
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            _animator.SetBool("TurnLeft", true);
            _animator.SetBool("TurnRight", false);
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            _animator.SetBool("TurnRight", false);
            _animator.SetBool("TurnLeft", false);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            _animator.SetBool("TurnRight", true);
            _animator.SetBool("TurnLeft", false);
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            _animator.SetBool("TurnRight", false);
            _animator.SetBool("TurnLeft", false);
        }

        if (Input.GetKey(KeyCode.Space) && Time.time > canFire) FireLaser();
    }

    private void CalculateMovement()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0) * speed * Time.deltaTime;

        transform.Translate(direction);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4.17f, 0), 0);

        if (transform.position.x >= 11.27f)
            transform.position = new Vector3(-11.27f, transform.position.y, 0);

        if (transform.position.x < -11.27f)
            transform.position = new Vector3(11.27f, transform.position.y, 0);
    }

    private void FireLaser()
    {
        canFire = Time.time + fireRate;
        if (isTripleShotActive)
        {
            Instantiate(tripleShotPrefab, transform.position + new Vector3(-2.17f, 0, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(laserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
        }

        _audioSource.Play();
    }

    public void Damage(bool isLaser = false)
    {
        if (isShieldActive)
        {
            shieldVisualizer.SetActive(false);
            isShieldActive = false;
            return;
        }

        lives = isLaser ? lives - 0.5 : lives--;
        _uiManager.UpdateLives((int)Math.Round(lives, 0));

        if ((int)lives == 2)
            leftEngine.SetActive(true);
        else if ((int)lives == 1)
            rightEngine.SetActive(true);

        if (lives < 1)
        {
            spawnManager.OnPlayerDeath();
            _audioManager.Play();
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    public void SpeedUpgradeActive()
    {
        speed = 7f;
        StartCoroutine(SpeedNormalizerRoutine());
    }

    public void ShieldUpgradeActive()
    {
        shieldVisualizer.SetActive(true);
        isShieldActive = true;
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        isTripleShotActive = false;
    }

    IEnumerator SpeedNormalizerRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        speed = 3.5f;
    }

    public void addScore(int points)
    {
        score += points;
        _uiManager.UpdateScore(score);
    }
}