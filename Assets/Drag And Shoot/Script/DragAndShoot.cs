using UnityEngine;
using UnityEngine.EventSystems;


public class DragAndShoot : MonoBehaviour
{
    public GameObject bob;
    [Header("Movement")]
    public float maxPower;
    [Tooltip("Set gravity to 0 if you want a top down ball game like billiardo.")]
    public float gravity = 0;
    [Tooltip("Slow the ball movement while aiming to make it easier to aim.")]
    [Range(0f, 0.1f)] public float slowMotion;

    [Tooltip("Allows you to aim and shot even when the ball is still moving.")]
    public bool shootWhileMoving = false;
    [Tooltip("Drag forward to aim instead of reverse aiming.")]
    public bool forwardDraging = true;
    [Tooltip("Show the draging line in the screen so you will not get confused where you aiming")]
    public bool showLineOnScreen = false;
    [Tooltip("Allow you to click whenever in the screen to start aiming, turn it off if you only want to start aiming while clicking in the ball")]
    public bool freeAim = true;

    Transform direction;
    Rigidbody2D rb;
    LineRenderer line;
    LineRenderer screenLine;

    // Vectors // 
    Vector2 startPosition;
    Vector2 targetPosition;
    Vector2 startMousePos;
    Vector2 currentMousePos;

    float shootPower;
    bool canShoot = true;



    void Start()
    {
        // rb = gameObject.GetComponentInParent<Rigidbody2D>();    // new!
        line = GetComponent<LineRenderer>();
        direction = transform.GetChild(0);
        screenLine = direction.GetComponent<LineRenderer>();

        bob = BobMovement.instance.gameObject;
        rb = bob.GetComponent<Rigidbody2D>();

        rb.gravityScale = 0;
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            // if (EventSystem.current.currentSelectedGameObject) return;  //ENABLE THIS IF YOU DONT WANT TO IGNORE UI
            if (freeAim)
                MouseClick();
            else
                BallClick();
        }
        if (Input.GetMouseButton(0) && isAiming)
        {
            // if (EventSystem.current.currentSelectedGameObject) return;  //ENABLE THIS IF YOU DONT WANT TO IGNORE UI
            MouseDrag();

            if (shootWhileMoving) rb.velocity /= (1 + slowMotion);

        }

        if (Input.GetMouseButtonUp(0) && isAiming)
        {
            // if (EventSystem.current.currentSelectedGameObject) return;  //ENABLE THIS IF YOU DONT WANT TO IGNORE UI
            MouseRelease();
        }


        if (shootWhileMoving)
            return;

        if (rb.velocity.magnitude < 0.7f)
        {
            rb.velocity = new Vector2(0, 0); //ENABLE THIS IF YOU WANT THE BALL TO STOP IF ITS MOVING SO SLOW
            canShoot = true;
        }
    }

    private bool objectClicked()
    {

        RaycastHit2D hit = Physics2D.CircleCast(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.2f, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            return true;
        }
        return false;
    }


    // MOUSE INPUTS
    void MouseClick()
    {

        isAiming = true;

        if (shootWhileMoving)
        {
            // Vector2 dir = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // transform.right = dir * 1;
            // Vector2 dir = transform.parent.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // transform.parent.right = dir * 1;            
            Vector2 dir = bob.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            bob.transform.right = dir * 1;

            startMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else
        {
            if (canShoot)
            {
                // Vector2 dir = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                // transform.right = dir * 1;
                Vector2 dir = bob.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                bob.transform.right = dir * 1;                

                startMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }

    }

    private bool isAiming = false;

    void BallClick()
    {
        if (!objectClicked())
            return;

        isAiming = true;

        if (shootWhileMoving)
        {
            // Vector2 dir = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // transform.right = dir * 1;
            Vector2 dir = bob.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            bob.transform.right = dir * 1;            

            startMousePos = transform.position;
        }
        else
        {
            if (canShoot)
            {
                // Vector2 dir = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                // transform.right = dir * 1;
                Vector2 dir = bob.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                bob.transform.right = dir * 1;                

                startMousePos = transform.position;
            }
        }

    }
    void MouseDrag()
    {
        if (!freeAim)
            // startMousePos = transform.position;
            startMousePos = bob.transform.position;

        if (shootWhileMoving)
        {
            LookAtShootDirection();
            DrawLine();

            if (showLineOnScreen)
                DrawScreenLine();

            float distance = Vector2.Distance(currentMousePos, startMousePos);

            if (distance > 1)
            {
                line.enabled = true;

                if (showLineOnScreen)
                    screenLine.enabled = true;
            }
        }
        else
        {
            if (canShoot)
            {
                LookAtShootDirection();
                DrawLine();

                if (showLineOnScreen)
                    DrawScreenLine();

                float distance = Vector2.Distance(currentMousePos, startMousePos);

                if (distance > 1)
                {
                    line.enabled = true;

                    if (showLineOnScreen)
                        screenLine.enabled = true;
                }
            }
        }

    }
    void MouseRelease()
    {
        if (shootWhileMoving /*&& !EventSystem.current.IsPointerOverGameObject()*/)
        {
            Shoot();
            screenLine.enabled = false;
            line.enabled = false;
        }
        else
        {
            if (canShoot /*&& !EventSystem.current.IsPointerOverGameObject()*/)
            {
                Shoot();
                screenLine.enabled = false;
                line.enabled = false;
            }
        }

        isAiming = false;

    }


    // ACTIONS  
    void LookAtShootDirection()
    {
        Vector3 dir = startMousePos - currentMousePos;

        if (forwardDraging)
        {
            // transform.right = dir * -1;
            bob.transform.right = dir * -1;
        }
        else
        {
            //
            bob.transform.right = dir;
        }


        float dis = Vector2.Distance(startMousePos, currentMousePos);
        dis *= 4;


        if (dis < maxPower)
        {
            direction.localPosition = new Vector2(dis / 6, 0);
            shootPower = dis;
        }
        else
        {
            shootPower = maxPower;
            direction.localPosition = new Vector2(maxPower / 6, 0);
        }

    }
    public void Shoot()
    {
        canShoot = false;
        rb.velocity = transform.right * shootPower;
    }


    void DrawScreenLine()
    {
        screenLine.positionCount = 1;
        screenLine.SetPosition(0, startMousePos);


        screenLine.positionCount = 2;
        screenLine.SetPosition(1, currentMousePos);
    }

    void DrawLine()
    {

        startPosition = transform.position;

        line.positionCount = 1;
        line.SetPosition(0, startPosition);


        targetPosition = direction.transform.position;
        currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        line.positionCount = 2;
        line.SetPosition(1, targetPosition);
    }

    Vector3[] positions;


}
