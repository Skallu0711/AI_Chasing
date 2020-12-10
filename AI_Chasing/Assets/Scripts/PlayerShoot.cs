using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public float range = 10f;

    private Vector3 mousePosition;
    private Vector3 direction;

    public LineRenderer lineRenderer;
    
    void Start()
    {
        lineRenderer.positionCount = 2;
    }
    
    void Update()
    {
        Shoot();
    }

    void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            direction = (mousePosition - transform.position).normalized;
            
            lineRenderer.positionCount = 2;
            lineRenderer.SetWidth(0.05f, 0.05f);

            RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, direction, range);

            if (raycastHit2D.collider.CompareTag("Enemy"))
            {
                Debug.Log("hit");
                
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, raycastHit2D.point);
                lineRenderer.material.color = Color.green;

                StartCoroutine(WaitAndClearLine(lineRenderer, 0.2f));
            }
            else
            {
                Debug.Log("miss");
                
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, transform.position + direction * range);
                lineRenderer.material.color = Color.white;

                StartCoroutine(WaitAndClearLine(lineRenderer, 0.2f));
            }
        }
    }

    IEnumerator WaitAndClearLine(LineRenderer lr, float time)
    {
        yield return new WaitForSeconds(time);
        lr.positionCount = 0;
    }
    
}
