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
    private Vector3 moveDirectionVector;
    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (view.IsMine)
        {
            CheckForInput();
            LookAtMouse();
        }
    }
    private void CheckForInput()
    {
        moveDirectionVector = Vector3.zero;

        moveDirectionVector = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"),0);

        myRigidbody2D.velocity = moveDirectionVector * movementSpeed;
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
