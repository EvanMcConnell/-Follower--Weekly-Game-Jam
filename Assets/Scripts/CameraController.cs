using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float horizontal;
    private bool facingRight = true;
    public GameObject player, endzone;
    // Update is called once per frame
    
    void Update()
    {
        /*horizontal = Input.GetAxisRaw("Horizontal");
        if ((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight))
        {
            Flip();
        }*/

        //transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -5);

        transform.position = player.transform.position.x < 0.7 ? new Vector3(player.transform.position.x, 1.6f, -10) : transform.position;
        print(transform.position);
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
    }
}
