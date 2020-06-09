using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private bool facingRight = true;
    private GameObject target, endzone;

    public static CameraController CreateComponent(GameObject where, GameObject target, GameObject endzone)
    {
        CameraController cam = where.AddComponent<CameraController>();
        cam.target = target;
        cam.endzone = endzone;
        return cam;
    }

    void Start()
    {
        if (!target){ target = GameObject.Find("Player"); }

    }

    void Update()
    {
        transform.position = target.transform.position.x < 0.7 ? new Vector3(target.transform.position.x, 1.6f, -10) : transform.position;
        //print(transform.position);
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
    }
}
