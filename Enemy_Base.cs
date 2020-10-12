using UnityEngine;

public class Enemy_Base : MonoBehaviour
{
    public GameObject assets;
    public Transform target_player;
    private Rigidbody2D _rb;

    public Transform gunBarrel;

    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
        assets = GameObject.FindGameObjectWithTag("GameAssets");
    }

    void Update()
    {
        Vector3 relativePos = target_player.position - transform.position;
        float angleToTarget = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg;
        _rb.rotation = angleToTarget;

        if(Input.GetKeyDown(KeyCode.F))
        {
            Instantiate(GameAssets.i.homingProjectile, gunBarrel.transform.position, gunBarrel.rotation);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Instantiate(GameAssets.i.electroProjectile, gunBarrel.transform.position, gunBarrel.rotation);
        }
    }
}
