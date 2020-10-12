using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour
{
    [Header("Type")]
    [SerializeField]
    public ProjectileTypes.projectileTypes _myProjectType;

    [Header("General Settings")]
    public Rigidbody2D _rb;
    public bool isFromBlackhole = false;
    public Vector3 lastVelocity;

    [Header("Homing Projectile")]
    public Transform tracking_target;
    public float _HomingSpeed = 1f;
    public float rotationSpeed = 200f;
    public bool pauseHoming = false;

    [Header("Normal Projectile")]
    public Vector2 _moveDirection;
    public float _movementSpeed = 200f;

    [Header("Electro Projectile")]
    public float _electroWaitPeriod = 2f;
    public float _electroWaitTick = 1f;

    public float _currentElectroSpeed = 0f;
    public float _electroAcceleration = 2f;
    public float _electricStartSpeed = 4f;
    public float _electricMaxSpeed = 20f;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        if (_myProjectType == ProjectileTypes.projectileTypes.homing_missile)
            tracking_target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        lastVelocity = _rb.velocity;
    }

    private void FixedUpdate()
    {
        if (_myProjectType == ProjectileTypes.projectileTypes.homing_missile)
            moveAsHoming();

        if (_myProjectType == ProjectileTypes.projectileTypes.electric)        
            moveAsElectro();
        
        
    }

    #region Projectile Motion Methods
    private void moveAsHoming()
    {
        Vector2 direction = tracking_target.position - transform.position;

        direction.Normalize();

        float rotationAmount = Vector3.Cross(direction, transform.up).z;

        _rb.angularVelocity = -rotationAmount * rotationSpeed;
        _rb.velocity = transform.up * _HomingSpeed;
    }

    private void moveAsProjectile()
    {
        _rb.velocity = _moveDirection * _movementSpeed;
    }

    private void moveAsElectro()
    {
        _rb.velocity = transform.up * _currentElectroSpeed;

        _currentElectroSpeed += _electroAcceleration * Time.deltaTime;

        if (_currentElectroSpeed > _electricMaxSpeed)
            _currentElectroSpeed = _electricMaxSpeed;

        /**     Potential speed up tracking logic??
        if (_electroWaitPeriod >= 0)
        {
            _electroWaitPeriod -= _electroWaitTick * Time.deltaTime;
        }
        else
        {
            _currentElectroSpeed += _electroAcceleration * Time.deltaTime;

            if (_currentElectroSpeed > _electricMaxSpeed)
                _currentElectroSpeed = _electricMaxSpeed;
        }
    */
    }
    #endregion
    
    #region Homing Methods
    public void UpdateTarget(Transform newTarget)
    {
        Debug.Log("Update Target");
        tracking_target = newTarget;
    }
    public void ChangeTargetToBoss()
    {
        isFromBlackhole = true;
        tracking_target = GameObject.FindGameObjectWithTag("Boss").transform;
    }

    public void setRotationSpeed(float speed)
    {
        rotationSpeed = speed;
    }

    public void ChangeToHomingWait(float seconds)
    {
        StartCoroutine("ModifyHoming", seconds);
    }


    IEnumerator ModifyHoming(float seconds)
    {
        Debug.Log("modify");

        yield return new WaitForSeconds(seconds);

        _myProjectType = ProjectileTypes.projectileTypes.homing_missile;
    }
    #endregion

    #region General Methods
    public void DestoryProjectile()
    {
        Destroy(gameObject);
    }
    #endregion

    #region Blackhole Methods
    public void SetBlackHoleSpeed(float speed, float rotation)
    {
        _HomingSpeed = speed;
        rotationSpeed = rotation;
    }

    public void ChangeLocation(Transform pos)
    {
        gameObject.transform.position = pos.position;
        gameObject.transform.rotation = pos.rotation;
    }
    #endregion


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter Trigger");
        if (other.gameObject.tag == "BlackHoleEntrance")
        {
            if (_myProjectType == ProjectileTypes.projectileTypes.homing_missile)
            {
                UpdateTarget(other.transform);
                SetBlackHoleSpeed(other.GetComponent<Entrance_BlackHole>().homingSpeed, other.GetComponent<Entrance_BlackHole>().rotationSpeed);
            }            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Hit");
        if (collision.gameObject.tag == "BlackHoleEntrance")
        {
            if (_myProjectType == ProjectileTypes.projectileTypes.homing_missile)
            {
                ChangeTargetToBoss();
                ChangeLocation(collision.gameObject.GetComponent<Entrance_BlackHole>().testExit.transform);
            }
        }
        else if (collision.gameObject.tag == "Player")
        {
            //Player Takes Damage

        }
        else if (collision.gameObject.tag == "Boss")
        {
            //Boss Takes Damage
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Reflector")
        {
            _myProjectType = ProjectileTypes.projectileTypes.missile;

            float speed = lastVelocity.magnitude;
            Vector3 direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

            _rb.velocity = direction * Mathf.Max(speed, 0f);

            StartCoroutine("ModifyHoming", 0.5);
        }

        if (collision.gameObject.tag == "ElectroCannonBattery")
        {
            Destroy(gameObject);

            collision.gameObject.GetComponent<ElectroCannonBattery>().IncrementChargeCount();
        }
    }
}
