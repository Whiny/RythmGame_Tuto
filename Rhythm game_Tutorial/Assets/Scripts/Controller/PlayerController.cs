using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static bool s_canPresskey = true;

    //이동
    [SerializeField] float moveSpeed = 3;
    Vector3 dir = new Vector3();
    public Vector3 destPos = new Vector3();
    Vector3 originPos = new Vector3();

    //회전
    [SerializeField] float spinSpeed = 270;
    Vector3 rotDir = new Vector3();
    Quaternion destRot = new Quaternion();

    //들썩임
    [SerializeField] float recoilPosY;
    [SerializeField] float recoilSpeed = 1.5f;

    [SerializeField] Transform fakeCube = null;
    [SerializeField] Transform realCube = null;
    

    TimingManager timingManager;
    CameraController cameraController;
    StatusManager statusManager;

    Rigidbody rigidbody;
    bool canMove = true;
    bool isFalling = false;

    // Start is called before the first frame update
    void Start()
    {
        timingManager = FindObjectOfType<TimingManager>();
        cameraController = FindObjectOfType<CameraController>();
        statusManager = FindObjectOfType<StatusManager>();
        recoilPosY = realCube.position.y + 0.25f;
        rigidbody = GetComponentInChildren<Rigidbody>();
        originPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        CheckFalling();
        if(Input.GetKeyDown(KeyCode.A)||Input.GetKeyDown(KeyCode.S)||Input.GetKeyDown(KeyCode.D)||Input.GetKeyDown(KeyCode.W))
        {
            if(canMove && s_canPresskey&&!isFalling)
            {
                Calc();
                if(timingManager.CheckTiming())
                {
                    StartAction();
                }
            }
            
        }
    }

    void Calc()
    {
        dir.Set(Input.GetAxisRaw("Vertical"),0, Input.GetAxisRaw("Horizontal"));

        destPos = transform.position + new Vector3(-dir.x,0,dir.z);

        rotDir = new Vector3(-dir.z, 0f, -dir.x);

        fakeCube.RotateAround(transform.position,rotDir, spinSpeed);

    }

    void StartAction()
    {
        
        destRot = fakeCube.rotation;

        StartCoroutine(MoveCo());
        StartCoroutine(SpinCo());
        StartCoroutine(RecoilCo());
        StartCoroutine(cameraController.ZoomCam());
    }

    IEnumerator MoveCo()
    {
        while(Vector3.SqrMagnitude(transform.position - destPos) >= 0.0001)
        {
            transform.position = Vector3.MoveTowards(transform.position, destPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        
        transform.position = destPos;
    }

    IEnumerator SpinCo()
    {
        while(Quaternion.Angle(realCube.rotation, destRot)> 0.5f)
        {
            realCube.rotation = Quaternion.RotateTowards(realCube.rotation,destRot,spinSpeed*Time.deltaTime);
            yield return null;
        }
        realCube.rotation = destRot;
    }

    IEnumerator RecoilCo()
    {
        while(realCube.position.y < recoilPosY)
        {
            realCube.position += new Vector3(0, recoilSpeed * Time.deltaTime, 0);
            yield return null;
        }

        while(realCube.position.y>0)
        {
            realCube.position -= new Vector3(0, recoilSpeed * Time.deltaTime, 0);
            yield return null;
        }
        realCube.localPosition = new Vector3(0f,0f,0f);
    }

    void CheckFalling()
    {
        if(!isFalling&&canMove)
        {
            if(!Physics.Raycast(transform.position, Vector3.down, 1.1f))
            {
                Falling();
            }
        }
    }

    void Falling()
    {
        isFalling = true;
        rigidbody.useGravity = true;
        rigidbody.isKinematic = false;
    }

    public void ResetFalling()
    {
        statusManager.DecreaseHP(1);
        
        if(!statusManager.IsDead())
        {
            isFalling = false;
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;

            transform.position = originPos;
            realCube.localPosition = new Vector3(0,0,0);

        }
        
        
    }
}