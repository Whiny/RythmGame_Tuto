using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform player = null;
    [SerializeField] float followSpeed = 15;

    Vector3 playerDistance = new Vector3();

    float hisDistance = 0;
    [SerializeField] float zoomDistance = -1.25f;
    
    // Start is called before the first frame update
    void Start()
    {
        playerDistance = transform.position - player.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 t_destPos = player.position + playerDistance + transform.forward *hisDistance;
        transform.position = Vector3.Lerp(transform.position, t_destPos, followSpeed*Time.deltaTime);
    }

    public IEnumerator ZoomCam()
    {
        hisDistance = zoomDistance;

        yield return new WaitForSeconds(0.15f);

        hisDistance = 0;
    }
}
