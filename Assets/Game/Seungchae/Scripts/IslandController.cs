using UnityEngine;

public class IslandController : MonoBehaviour
{
    [SerializeField]
    private float acceleration = 10f;

    [SerializeField]
    private float deceleration = 20f;
    
    [SerializeField]
    private float angularAcceleration = 5f;
    
    [SerializeField]
    private float angularDeceleration = 0.5f;

    [SerializeField]
    private float maxSpeed = 20f;

    [SerializeField]
    private float maxAngularSpeed = 2f;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Vector3 targetDirection = new (Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (targetDirection.sqrMagnitude < 3e-5 && Input.GetButton("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 500, 1 << LayerMask.NameToLayer("Plane"))) {
                Vector3 target = hit.point;
                target.y = transform.position.y;
                targetDirection = (target - transform.position).normalized;
            }
        }

        if (targetDirection.sqrMagnitude < 3e-5)
            return;

        Vector3 velocity = _rigidbody.velocity;
        if (velocity.sqrMagnitude < 3e-5)
            velocity = targetDirection;
        float currentSpeed = velocity.magnitude;
        Vector3 currentDirection = velocity / currentSpeed;

        if (currentSpeed > maxSpeed)
            _rigidbody.velocity = currentDirection * maxSpeed;
        
        if (_rigidbody.angularVelocity.sqrMagnitude > maxAngularSpeed * maxAngularSpeed)
            _rigidbody.angularVelocity = _rigidbody.angularVelocity.normalized * maxAngularSpeed;

        if (Vector3.Dot(currentDirection, targetDirection) < 0)
            _rigidbody.AddForce(targetDirection * deceleration, ForceMode.Acceleration);
        else
            _rigidbody.AddForce(targetDirection * acceleration, ForceMode.Acceleration);

        Vector3 rotationalDirection = Vector3.Cross(currentDirection, targetDirection);
        if (Vector3.Dot(_rigidbody.angularVelocity, rotationalDirection) < 0f)
            _rigidbody.AddTorque(Vector3.Cross(currentDirection, targetDirection) * angularDeceleration, ForceMode.Acceleration);
        else
            _rigidbody.AddTorque(Vector3.Cross(currentDirection, targetDirection) * angularAcceleration, ForceMode.Acceleration);
    }
}
