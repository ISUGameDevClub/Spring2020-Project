using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float MouseSensetivity;
    public float smoothing;

    private GameObject cr;
    private Vector2 smoothedVelocity;
    private PlayerController pc;
    public Vector2 currentLookingPos;
    void Start()
    {
        currentLookingPos.x = transform.root.transform.eulerAngles.y;
        pc = FindObjectOfType<PlayerController>();
        cr = pc.transform.GetChild(0).gameObject;
    }

    void FixedUpdate()
    {
        rotateCamera();
    }

    private void rotateCamera()
    {
        Vector2 inputValues = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        inputValues = Vector2.Scale(inputValues, new Vector2(MouseSensetivity * smoothing, MouseSensetivity * smoothing));

        smoothedVelocity.x = Mathf.Lerp(smoothedVelocity.x, inputValues.x, 1f / smoothing);
        smoothedVelocity.y = Mathf.Lerp(smoothedVelocity.y, inputValues.y, 1f / smoothing);


        currentLookingPos += smoothedVelocity;

        currentLookingPos.y = Mathf.Clamp(currentLookingPos.y, -90f, 80f);
        currentLookingPos.x %= 360;
        if (currentLookingPos.x < 0)
            currentLookingPos.x = 360.0f - Mathf.Abs(currentLookingPos.x);


        transform.localRotation = Quaternion.AngleAxis(-currentLookingPos.y, Vector3.right);
    }

    public void SetCameraRotaterWallRunningRight()
    {
        float lookingDifference = (currentLookingPos.x - pc.wantedAngle) % 360;
        if (lookingDifference < 0)
            lookingDifference = 360.0f - Mathf.Abs(lookingDifference);
        if (lookingDifference > 50 && lookingDifference < 125)
        {
            currentLookingPos.x = (pc.wantedAngle + 50) % 360;
        }
        else if(lookingDifference > 125 && lookingDifference < 200)
        {
            currentLookingPos.x = (pc.wantedAngle + 200) % 360;
        }
        cr.transform.localRotation = Quaternion.AngleAxis(currentLookingPos.x - pc.wantedAngle, pc.transform.up);
    }

    public void SetCameraRotaterWallRunningLeft()
    {
        float lookingDifference = (currentLookingPos.x - pc.wantedAngle) % 360;
        if (lookingDifference < 0)
            lookingDifference = 360.0f - Mathf.Abs(lookingDifference);
        if (lookingDifference < 310 && lookingDifference > 235)
        {
            currentLookingPos.x = (pc.wantedAngle + 310) % 360;
        }
        else if(lookingDifference < 235 && lookingDifference > 160)
        {
            currentLookingPos.x = (pc.wantedAngle + 160) % 360;
        }
        cr.transform.localRotation = Quaternion.AngleAxis(currentLookingPos.x - pc.wantedAngle, pc.transform.up);
    }

    public void SetCameraRotatorNuetral()
    {
        cr.transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    private void OnGUI()
    {
        GUI.Box(new Rect(Screen.width / 2, Screen.height / 2, 10, 10), "");
    }
}
