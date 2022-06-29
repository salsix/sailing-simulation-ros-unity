using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosSailTwist = RosMessageTypes.UnityRoboticsDemo.UnitySailTwistMsg;


public class RigController : MonoBehaviour
{
    public GameObject mast;
    public GameObject rig;
    public float currentRot = 0;
    private float goalRot = 0;
    private int rotDir;
    private float rotSpeed = 50;
    private bool isRotating = false;

    // Start is called before the first frame update
    void Start()
    {
        ROSConnection.GetOrCreateInstance().Subscribe<RosSailTwist>("twist", TwistChange);
    }

    void TwistChange(RosSailTwist twistMessage) 
    {
        if (twistMessage.twist >= -90 && twistMessage.twist <= 90 && twistMessage.twist != goalRot) {
            goalRot = twistMessage.twist;
            rotDir = goalRot < currentRot ? -1 : 1;
            isRotating = true;
        }
    }

    private void Update() {
    
        if (isRotating) {
            if (rotDir == 1 && currentRot < goalRot) {
                rig.transform.RotateAround(mast.transform.position, mast.transform.up, rotSpeed * Time.deltaTime);
                currentRot = currentRot + rotSpeed * Time.deltaTime;
            } else if (rotDir == -1 && currentRot > goalRot) {
                rig.transform.RotateAround(mast.transform.position, mast.transform.up, -rotSpeed * Time.deltaTime);
                currentRot = currentRot - rotSpeed * Time.deltaTime;
            } else {
                isRotating = false;
            }
        }
    }
}
