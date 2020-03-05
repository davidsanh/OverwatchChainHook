using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HookScript : MonoBehaviour
{
    public string[] tagsToCheck;
    //Force applied to nova bomb upon spawn
    public float speed, returnSpeed;
    public float range, stopRange;

    //Private variables
    [HideInInspector]
    public Transform caster, collidedWith;
    private LineRenderer line;
    private bool hasCollided;

    private void Start()
    {
        line = transform.Find("Line").GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (caster)
        {
            line.SetPosition(0, caster.position);
            line.SetPosition(1, transform.position);
            //Check if we have impacted
            if (hasCollided)
            {
                transform.LookAt(caster);
                var dist = Vector3.Distance(transform.position, caster.position);
                if (dist < stopRange)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                var dist = Vector3.Distance(transform.position, caster.position);
                if (dist > range)
                {
                    Collision(null);
                }
            }

            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            if (collidedWith) { collidedWith.transform.position = transform.position; }
        }
        else { Destroy(gameObject); }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Here add as many checks as you want for your nova bomb's collision
        if (!hasCollided && tagsToCheck.Contains(other.gameObject.tag))
        {
            Collision(other.transform);
        }
    }

    void Collision(Transform col)
    {
        speed = returnSpeed;
        //Stop movement
        hasCollided = true;
        if (col)
        {
            transform.position = col.position;
            collidedWith = col;
        }
    }
}
