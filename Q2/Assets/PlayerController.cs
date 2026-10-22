using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

// Attach to the Player sphere (needs Rigidbody + PlayerInput with Behavior = "Send Messages").
// Tags to create: PickUp, PowerUp, BreakableWall, Goal
public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public float jumpForce = 5f;
    public float powerUpSpeedMultiplier = 2f;
    public Color powerUpColor = Color.red;

    [Header("Scene references")]
    public GameObject startButton;     // the UI Button (c)
    public GameObject powerUpObject;   // the capsule, start it INACTIVE (e)
    public TextMeshProUGUI countText;
    public TextMeshProUGUI powerUpText; // "POWER-UP ACTIVE!" start INACTIVE (e.b)
    public TextMeshProUGUI winText;     // "You win!" start INACTIVE (g)

    private Rigidbody rb;
    private float moveX, moveY;
    private int count, totalPickUps;
    private bool started = false;
    private bool poweredUp = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;                       // (c) no gravity before Start
        totalPickUps = GameObject.FindGameObjectsWithTag("PickUp").Length;
        count = 0;
        SetCountText();
        if (powerUpObject) powerUpObject.SetActive(false);
        if (powerUpText) powerUpText.gameObject.SetActive(false);
        if (winText) winText.gameObject.SetActive(false);
    }

    // (c) hook this to the Button's OnClick()
    public void StartGame()
    {
        started = true;
        rb.useGravity = true;
        if (startButton) startButton.SetActive(false);
    }

    void OnMove(InputValue value)
    {
        Vector2 v = value.Get<Vector2>();
        moveX = v.x; moveY = v.y;
    }

    // (d) Space is bound to "Jump" in the default InputSystem_Actions
    void OnJump(InputValue value)
    {
        if (started && IsGrounded())
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    bool IsGrounded()
    {
        float r = GetComponent<SphereCollider>().radius * transform.localScale.y;
        return Physics.Raycast(transform.position, Vector3.down, r + 0.1f);
    }

    void FixedUpdate()
    {
        if (!started) { rb.linearVelocity = Vector3.zero; return; }  // (c) can't move yet
        rb.AddForce(new Vector3(moveX, 0f, moveY) * speed);
        // Unity 2022 or older: use rb.velocity instead of rb.linearVelocity
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickUp"))                     // (b)
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
            if (count >= totalPickUps && powerUpObject)
                powerUpObject.SetActive(true);               // (e) appears after all cubes
        }
        else if (other.CompareTag("PowerUp"))               // (e.a)
        {
            other.gameObject.SetActive(false);
            poweredUp = true;
            speed *= powerUpSpeedMultiplier;
            GetComponent<Renderer>().material.color = powerUpColor;
            if (powerUpText) powerUpText.gameObject.SetActive(true);   // (e.b)
        }
        else if (other.CompareTag("Goal"))                  // (g)
        {
            if (winText) { winText.text = "You win!"; winText.gameObject.SetActive(true); }
            gameObject.SetActive(false);                     // hide player last
        }
    }

    // (f) wall has a normal (non-trigger) collider, so it blocks until powered up
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("BreakableWall") && poweredUp)
            collision.gameObject.SetActive(false);
    }

    void SetCountText()
    {
        if (countText) countText.text = "Count: " + count + " / " + totalPickUps;
    }
}
