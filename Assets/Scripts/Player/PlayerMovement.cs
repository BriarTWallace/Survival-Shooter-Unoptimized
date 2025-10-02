using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
	public float speed = 6f;

	private Vector2 moveInput;

	private Vector3 movement;
	private Animator anim;
	private Rigidbody playerRigidbody;
	private int floorMask;
	private float camRayLength = 100f;

	private PlayerControls controls;

    private static readonly int hashIsWalking = Animator.StringToHash("IsWalking");

    void Awake()
	{
		floorMask = LayerMask.GetMask("Floor");
		anim = GetComponent<Animator>();
		playerRigidbody = GetComponent<Rigidbody>();

		controls = new PlayerControls();
	}

    private void OnEnable()
    {
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        controls.Enable();
    }

    private void OnDisable()
    {
		controls.Disable();
    }

    void FixedUpdate()
	{
        Move(moveInput.x, moveInput.y);
        Turning();
        Animating(moveInput.x, moveInput.y);
    }

	void Move(float h, float v)
	{
		movement.Set(h, 0f, v);
		movement = movement.normalized * speed * Time.deltaTime;

		playerRigidbody.MovePosition(transform.position + movement);
	}

	void Turning()
	{
		Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit floorHit;

		if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask)) {
			Vector3 playerToMouse = floorHit.point - transform.position;
			playerToMouse.y = 0f;

			Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
			playerRigidbody.MoveRotation(newRotation);
		}
	}

	void Animating(float h, float v)
	{
		bool walking = h != 0f || v != 0f;

        anim.SetBool(hashIsWalking, walking);
    }
}
