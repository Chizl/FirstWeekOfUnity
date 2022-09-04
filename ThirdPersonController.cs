using UnityEngine;
using System.Collections;

public class ThirdPersonController : MonoBehaviour {

	public float walkSpeed = 2;
	public float runSpeed = 6;
    public float gravity = -19.62f;
    public float jumpHeight = 3;
    [Range(0,1)]
    public float airControlPercent;

	public float turnSmoothTime = 0.2f;
	float turnSmoothVelocity;

	public float speedSmoothTime = 0.1f;
	float speedSmoothVelocity;
	float currentSpeed;
    float velocityY;
    int jumpCount;

	Transform cameraT;
    CharacterController controller;

	void Start () {
		cameraT = Camera.main.transform;
        controller = GetComponent<CharacterController> ();
	}

	void Update () {

		Vector2 inputDir = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical")).normalized;
        bool running = Input.GetKey (KeyCode.LeftShift);

        Move(inputDir, running);

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
	}

    void Move(Vector2 inputDir, bool running)
    {
        if (inputDir != Vector2.zero) 
        {
			float targetRotation = Mathf.Atan2 (inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
			transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
		}

		float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
		currentSpeed = Mathf.SmoothDamp (currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));

		velocityY += Time.deltaTime * gravity;        
        Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;

        controller.Move(velocity * Time.deltaTime);

        if (controller.isGrounded) {
            velocityY = -2f;
            jumpCount = 0;
        }
    }

    void Jump() {
        if (controller.isGrounded)
        {
            velocityY = Mathf.Sqrt(-2 * gravity * jumpHeight);
            jumpCount++;
        }
        else if (!controller.isGrounded && jumpCount < 2)
        {
            velocityY = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpCount++;
            jumpCount++;
        }
    }

    float GetModifiedSmoothTime(float smoothTime) {
        if (controller.isGrounded)
        {
            return smoothTime;
        }
        if (airControlPercent == 0)
        {
            return float.MaxValue;
        }

        return smoothTime / airControlPercent;
    }
}