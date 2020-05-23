using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryPredictor : MonoBehaviour
{
    [Header("Line renderer variabkes")]
    public LineRenderer line;
    [Range(2, 30)]
    public int resolution;

    [Header("Formula variables")]
    private Vector2 velocity;
    private float Force;
    public float yLimit;
    private float g;

    [Header("Linecast Variables")]
    [Range(2, 30)]
    public int linecastResolution;
    public LayerMask canHit;

    void Start()
    {
        g = Mathf.Abs(Physics2D.gravity.y);
    }

    void Update()
    {
        StartCoroutine(RenderLine());
    }

    public void setVelocity(Vector2 vel)
    {
        this.velocity = vel;
    }

    public void setForce(float f)
    {
        this.Force = f;
    }


    private IEnumerator RenderLine()
    {
        line.positionCount = resolution + 1;
        line.SetPositions(CalculateLineArray());
        yield return null;
    }

    private Vector3[] CalculateLineArray()
    {
        Vector3[] lineArray = new Vector3[resolution + 1];

        var lowestTimeValue = MaxTimeY() / resolution;

        for (int i = 0; i < lineArray.Length; i++)
        {
            var t = lowestTimeValue * i;
            lineArray[i] = CalculateLinePoint(t);
        }

        return lineArray;
    }

    private Vector2 HitPosition()
    {
        var lowestTimeValue = MaxTimeY() / linecastResolution;

        for (int i = 0; i < linecastResolution + 1; i++)
        {
            var t = lowestTimeValue * i;
            var tt = lowestTimeValue * (i + 1);

            var hit = Physics2D.Linecast(CalculateLinePoint(t), CalculateLinePoint(tt), canHit);

            if (hit)
                return hit.point;
        }

        return CalculateLinePoint(MaxTimeY());
    }

    private Vector3 CalculateLinePoint(float t)
    {
        float x = velocity.x * t;
        float y = (velocity.y * t) - (g * Mathf.Pow(t, 2) / 2);
        return new Vector3(x * Force, y * Force);
    }

    private float MaxTimeY()
    {
        var v = velocity.y;
        var vv = v * v;

        var t = (v + Mathf.Sqrt(vv + 2 * g * (transform.position.y - yLimit))) / g;
        return t;
    }

    private float MaxTimeX()
    {
        var x = velocity.x;
        if (x == 0)
        {
            velocity.x = 000.1f;
            x = velocity.x;
        }

        var t = (HitPosition().x - transform.position.x) / x;
        return t;
    }
}
