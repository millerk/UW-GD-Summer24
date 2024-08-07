using UnityEngine;

public class CannonShooting : MonoBehaviour
{

    public Transform firePoint;
    public GameObject projectile;
    public Cannon cannonDef;
    public Animator animator;
    private float _cannonForce = 20f;
    private bool _canFire = true;

    void Start()
    {
        CannonContainer cannonBuilder = GetComponentInParent<CannonContainer>();
        cannonDef = cannonBuilder.cannonDef;
        projectile = cannonDef.projectile;
        CannonballLogic projectileConfig = projectile.GetComponent<CannonballLogic>();
        projectileConfig.maxDistance = cannonDef.attackDistance;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.GameIsPaused && Input.GetButtonDown("Fire1") && _canFire)
        {
            _canFire = false;
            animator.SetTrigger("fire");
        }
    }

    // Callback used by animator when reaching the frame to create and launch projectile
    public void Shoot()
    {
        GameObject _cannonball = Instantiate(projectile, firePoint.position, firePoint.rotation);
        Rigidbody2D _rb = _cannonball.GetComponent<Rigidbody2D>();
        _rb.AddForce(-firePoint.right * _cannonForce, ForceMode2D.Impulse);
    }

    // Callback used by animator when the firing animation has finished
    public void ReadyToShoot()
    {
        _canFire = true;
    }
}
