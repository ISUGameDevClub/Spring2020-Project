using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float MouseSensetivity;
    public float smoothing;

    private GameObject cr;
    private PlayerController pc;
    private Vector2 smoothedVelocity;
    public Vector2 currentLookingPos;
    void Start()
    {
        cr = transform.parent.parent.gameObject;
        pc = FindObjectOfType<PlayerController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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

        currentLookingPos.y = Mathf.Clamp(currentLookingPos.y, -80f, 80f);
        currentLookingPos.x %= 360;

        transform.localRotation = Quaternion.AngleAxis(-currentLookingPos.y, Vector3.right);
        if (pc.freeCam)
        {
            cr.transform.localRotation = Quaternion.AngleAxis(currentLookingPos.x - pc.wantedAngle, transform.up);
            cr.transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
        }
        else
            cr.transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    private void OnGUI()
    {
        GUI.Box(new Rect(Screen.width / 2, Screen.height / 2, 10, 10), "");
    }
}
