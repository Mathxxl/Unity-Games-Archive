using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 200;

    [SerializeField] private Color colorMin;

    [SerializeField] private Color colorMax;

    [SerializeField] private bool ennemyBullet;


    private Rigidbody _rb;
    
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    
    private void OnEnable()
    {
        _rb.velocity =  speed * transform.forward;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ennemyBullet && (other.CompareTag("Player") || other.CompareTag("Shield")))
        {
            gameObject.SetActive(false);
        }
        
        if (!ennemyBullet && other.CompareTag("Ennemy"))
        {
            gameObject.SetActive(false);
        }
    }
}
