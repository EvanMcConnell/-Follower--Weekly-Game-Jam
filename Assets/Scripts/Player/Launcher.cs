using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    [SerializeField]
    [Header("Rigidbody2D")]
    private Rigidbody2D rb;

    [Header("Floats")]
    public float launchForce = 100f;

    [Header("Vectors")]
    private Vector2 launchDir;
    [SerializeField]
    private Vector2 trajectoryCorrection;



    public static Launcher CreateComponent(GameObject where, Rigidbody2D rb)
    {
        Launcher l = where.AddComponent<Launcher>();
        l.rb = rb;
        return l;
    }

    void Start() {
    }

    public void launch(Vector2 launchDir)
    {
        rb.AddForce(launchDir * launchForce * trajectoryCorrection, ForceMode2D.Impulse);
        rb.drag = 0.5f;
        print("Launched: " + launchDir + ", " + launchForce);
    }
}
