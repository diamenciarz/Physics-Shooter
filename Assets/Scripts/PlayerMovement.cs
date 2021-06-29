using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed;


    PhotonView view;
    private Rigidbody2D myRigidbody2D;
    private Quaternion deltaRotation = Quaternion.Euler(0,0,90);
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine)
        {
            CheckForInput();
            LookAtMouse();
        }
    }
    private void CheckForInput()
    {
        Vector3 moveOneStepVector = Vector3.zero;

        moveOneStepVector = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"),0) * movementSpeed;

        transform.position += moveOneStepVector * Time.deltaTime;
    }
    private void LookAtMouse()
    {
        Vector3 translatedMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        translatedMousePosition.z = transform.position.z;
        Vector3 relativePositionFromPlayerToMouse = translatedMousePosition - transform.position;
        float angleFromZeroToItem = Vector3.SignedAngle(Vector3.up, relativePositionFromPlayerToMouse, Vector3.forward);

        transform.rotation = Quaternion.Euler(0,0, angleFromZeroToItem) * deltaRotation;
    }
}
