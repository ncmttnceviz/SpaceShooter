using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private byte speed = 8;
    [SerializeField] private bool isEnemyLaser;


    private void Update()
    {
        Vector3 direction = isEnemyLaser ? Vector3.down : Vector3.up;
        transform.Translate(direction * speed * Time.deltaTime);

        if ((!isEnemyLaser && transform.position.y >= 6) || (isEnemyLaser && transform.position.y <= -6) )
        {
            if (transform.parent)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    public void AssignEnemyLaser()
    {
        isEnemyLaser = true;
    }

    public bool IsEnemyLaser()
    {
        return isEnemyLaser;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isEnemyLaser)
        {
            Player player = other.GetComponent<Player>();
            player.Damage(true);
        }
    }
}