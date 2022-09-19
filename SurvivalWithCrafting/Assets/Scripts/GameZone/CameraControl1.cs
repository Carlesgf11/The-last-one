using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CinematicState { ACTIVE, DESACTIVE };
public class CameraControl1 : MonoBehaviour
{
    static public CinematicState state;
    public Transform Target;

    public GameObject target;
    private Vector3 finalPos;
    public float speed;

    public List<GameObject> allTerrains;
    public GameObject currentTerrain;
    private Transform limitLeft, limitRight;

    void Start()
    {
        ChangeTerrain(0);
        state = CinematicState.ACTIVE;
    }
    public void DesactiveCinematic()
    {
        state = CinematicState.DESACTIVE;
    }

    public void ChangeTerrain(int _terrain)
    {
        currentTerrain = allTerrains[_terrain];
        limitLeft = currentTerrain.transform.Find("Left");
        limitRight = currentTerrain.transform.Find("Right");
    }

    void Update()
    {
        if(state == CinematicState.DESACTIVE)
        {
            Vector3 finalPos = Target.position;
            finalPos.z = -10;
            transform.position = Vector3.Lerp(transform.position, finalPos, 5);
        }

        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    ChangeTerrain(0);
        //}
        //else if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    ChangeTerrain(1);
        //}
        //else if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    ChangeTerrain(2);
        //}
        //else if (Input.GetKeyDown(KeyCode.Alpha4))
        //{
        //    ChangeTerrain(3);
        //}
        //else if (Input.GetKeyDown(KeyCode.Alpha5))
        //{
        //    ChangeTerrain(4);
        //}
        //else if (Input.GetKeyDown(KeyCode.Alpha6))
        //{
        //    ChangeTerrain(5);
        //}
        //else if (Input.GetKeyDown(KeyCode.Alpha7))
        //{
        //    ChangeTerrain(6);
        //}


        if (target.transform.parent != null)
        {

            finalPos = new Vector3(target.transform.position.x, target.transform.position.y, -10);
            transform.position = Vector3.Lerp(transform.position, finalPos, speed * Time.deltaTime);

            if(transform.position.x < limitLeft.position.x)
            {
                transform.position = new Vector3(limitLeft.position.x, transform.position.y, transform.position.z);
            }
            if (transform.position.x > limitRight.position.x)
            {
                transform.position = new Vector3(limitRight.position.x, transform.position.y, transform.position.z);
            }
            if (transform.position.y < 0)
            {
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            }
            if (transform.position.y > 0)
            {
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            }
        }
    }

    void FixedUpdate()
    {

        if (target.transform.parent == null)
        {
            finalPos = new Vector3(target.transform.position.x, target.transform.position.y, -10);
            transform.position = Vector3.Lerp(transform.position, finalPos, speed * Time.deltaTime);
            if (transform.position.x < limitLeft.position.x)
            {
                transform.position = new Vector3(limitLeft.position.x, transform.position.y, transform.position.z);
            }
            if (transform.position.x > limitRight.position.x)
            {
                transform.position = new Vector3(limitRight.position.x, transform.position.y, transform.position.z);
            }
            if (transform.position.y < 0)
            {
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            }
            if (transform.position.y > 0)
            {
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            }
        }
    }
}
