using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private int powerId;
    [SerializeField] private AudioClip audioClip;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (transform.position.y < -4.5f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(audioClip, transform.position);

            switch (powerId)
            {
                case 0:
                    player.TripleShotActive();
                    break;
                case 1:
                    player.SpeedUpgradeActive();
                    break;
                case 2:
                    player.ShieldUpgradeActive();
                    break;
            }

            Destroy(this.gameObject);
        }
    }
}