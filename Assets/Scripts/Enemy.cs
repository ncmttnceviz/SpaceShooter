using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 4f;
    [SerializeField] private Player player;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject enemyLaserPrefab;
    [SerializeField] private float fireRate;
    [SerializeField] private float canFire = -1f;
    private AudioSource _audioManager;


    private void Start()
    {
        animator = GetComponent<Animator>();
        _audioManager = GameObject.Find("ExplosionMusic").GetComponent<AudioSource>();
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    private void Update()
    {
        CalculateMovement();
        Fire();
    }

    private void Fire()
    {
        if (Time.time > canFire)
        {
            fireRate = Random.Range(3f, 7f);
            canFire = Time.time + fireRate;
            GameObject enemyLaser = Instantiate(enemyLaserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    private void CalculateMovement()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (transform.position.y < -7f)
        {
            float randomx = Random.Range(-10f, 10f);

            transform.position = new Vector3(randomx, 7f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();
            player.Damage();
            animator.SetTrigger("onEnemyExplosionTrigger");
            _audioManager.Play();
            speed = 0;
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.35f);
        }

        if (other.CompareTag("Laser"))
        {
            Laser laser = other.GetComponent<Laser>();
            if (!laser.IsEnemyLaser())
            {
                player.addScore(10);
                Destroy(other.gameObject);
                animator.SetTrigger("onEnemyExplosionTrigger");
                _audioManager.Play();
                speed = 0;
                Destroy(GetComponent<Collider2D>());
                Destroy(this.gameObject, 2.35f);
            }
        }

       
    }
}