using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TouchJoystickMove : MonoBehaviour
{
    // using https://developer.oculus.com/documentation/unity/unity-ovrinput

    public float movementSpeed = 1.0f;
    public float rotationSpeed = 45.0f;

    public float snapTurnAmount = 45.0f;
    public float snapTurnTime = 0.1f;

    public Transform camera;

    public RawImage tunnel;

    private bool isTurning = false;
    private float startYaw;
    private float endYaw;
    private float t = 1.1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        // Snap Turn
        if (OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).x > 0.9 && !isTurning)
        {
            isTurning = true;
            //transform.Rotate(Vector3.up, snapTurnAmount, Space.World);
            startYaw = transform.rotation.eulerAngles.y;
            endYaw = startYaw + snapTurnAmount;
            t = 0.0f;
        }
        else if (OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).x < -0.9 && !isTurning)
        {
            isTurning = true;
            //transform.Rotate(Vector3.up, -snapTurnAmount, Space.World);
            startYaw = transform.rotation.eulerAngles.y;
            endYaw = startYaw - snapTurnAmount;
            t = 0.0f;
        }
        else if (Mathf.Abs(OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).x) < 0.1 && isTurning)
        {
            isTurning = false;
        }


        if (t < 1.0f)
        {
            t += Time.deltaTime / snapTurnTime;
            float yaw = Mathf.Lerp(startYaw, endYaw, t);
            Vector3 curRot = transform.rotation.eulerAngles;

            transform.rotation = Quaternion.Euler(curRot.x, yaw, curRot.z);
        }




        // Joystick movement
        float movement = movementSpeed;
        movement *= Time.deltaTime;


        Vector2 joystickInput = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);

        float x = Mathf.Pow(joystickInput.x, 3);
        float z = Mathf.Pow(joystickInput.y, 3);

        Vector3 headDirectedMovement = ((camera.forward * movement * z) + (camera.right * movement * x));
        headDirectedMovement.y = 0.0f;

        transform.Translate(headDirectedMovement, Space.World);

        //Vector2 movement = new Vector2(movementSpeed, movementSpeed);
        //movement *= OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);
        //movement *= Time.deltaTime;
        //transform.Translate(new Vector3(movement.x, 0.0f, movement.y));



        Color tunnelColor = Color.white;
        tunnelColor.a = Mathf.Pow(joystickInput.magnitude, 5);

        tunnel.color = tunnelColor;

        //// Joystick rotation
        //float rotation = rotationSpeed * Time.deltaTime * OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).x;
        //transform.Rotate(Vector3.up, rotation, Space.World);



    }
}
