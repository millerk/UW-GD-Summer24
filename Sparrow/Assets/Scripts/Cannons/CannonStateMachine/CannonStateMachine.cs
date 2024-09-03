using UnityEngine;

public class CannonStateMachine : MonoBehaviour
{
    
    public Transform projectileSpawnPoint;
    public GameObject projectile;
    public Animator animator;
    public GameObject target;
    public Cannon cannonDef;
    public CannonballLogic projectileConfig;
    public Rigidbody2D cannonRb;
    public Rigidbody2D shipRb;
    private bool _canFire = true;

    // State variables
    CannonBaseState _currentState;
    CannonStateFactory _states;
    public CannonBaseState CurrentState { get {return _currentState;} set { _currentState = value;} }
    
    public Transform ProjectileSpawnPoint { get {return projectileSpawnPoint;} set {projectileSpawnPoint = value;} }
    public Animator CannonAnim { get {return animator;} set {animator = value;}}
    public GameObject Target { get {return target;} }
    public CannonballLogic ProjectileConfig  {get {return projectileConfig;} set {projectileConfig = value;} }
    public Rigidbody2D CannonRb { get {return cannonRb;} set {cannonRb = value;} }
    public Rigidbody2D ShipRb { get {return shipRb;} set {shipRb = value;} }
    public bool CanFire { get {return _canFire;} set {_canFire = value;} }



    // Start is called before the first frame update
    void Start()
    {
        // Initialize the state machine
        _states = new CannonStateFactory(this);
        _currentState = _states.Searching();
        _currentState.EnterState();
    }

    // Update is called once per frame
    void Update()
    {
        _currentState.UpdateState();
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
        if (target == null)
        {
            _currentState = _states.Searching();
            _currentState.EnterState();
        }
    }

    // Callback used by animator when reaching the frame to create and launch projectile
    public void Shoot()
    {
        GameObject _cannonball = Instantiate(projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        Rigidbody2D _rb = _cannonball.GetComponent<Rigidbody2D>();
        _rb.AddForce(-projectileSpawnPoint.right * projectileConfig.projectileSpeed, ForceMode2D.Impulse);
        _canFire = false;
    }

    // Callback used by animator when the firing animation has finished
    public void ReadyToShoot()
    {
        _canFire = true;
    }
}
