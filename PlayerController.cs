using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D playerRB2D;

    public float movementSpeed;
    public float movementSpeedVehicleFactor;
    public float movementSpeedWalkingFactor;
    public float rotationSpeed;
    public GameObject vehiclePrefab;
    public GameObject generatedVehicle;
    public Vector3 vehicleOffsetPos;
    public bool collidedVehicle;
    
    private bool _usingVehicle;
    public bool usingVehicle{
        get {return _usingVehicle;}
        set {
            if (_usingVehicle == false && value == true){
                Debug.Log("boolean variable changed from: " + _usingVehicle + " to: " + value);
            }
            else if (_usingVehicle == true && value == false){
                Debug.Log("boolean variable changed from: " + _usingVehicle + " to: " + value);
            }
            else {}

            _usingVehicle = value;
        }
    }

    void Awake(){
        usingVehicle = true;
        rotationSpeed = 2000f;
        movementSpeedWalkingFactor = 500f;
        movementSpeedVehicleFactor = 500f;
        movementSpeed = movementSpeedVehicleFactor * getTileVelocity();
        playerRB2D = GetComponent<Rigidbody2D>();
        playerRB2D.isKinematic = false;
        playerRB2D.angularDrag = 1000.0f;
        playerRB2D.gravityScale = 0.0f;
        vehicleOffsetPos = new Vector3(3.4f, 0.0f, 0.0f);
    }

    void FixedUpdate()
    {
        Vector2 targetVelocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        // Debug.Log("collided to vehicle: " + collidedVehicle + " , using vehicle: " + usingVehicle);

        if (targetVelocity != Vector2.zero) 
        {
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, targetVelocity);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        Move(targetVelocity);
    }

    void Move(Vector2 targetVelocity)
    {
        playerRB2D.velocity = (targetVelocity * movementSpeed) * Time.deltaTime;
        // playerRB2D.velocity = (targetVelocity * movementSpeed *getTileVelocity()) * Time.deltaTime;
    }

    public float getTileVelocity(){
        return 1.0f;
    }

    void Update()
    {    
        if(Input.GetKeyDown("e")){
            //to using the vehicle
            if(!usingVehicle && collidedVehicle){
                Destroy(generatedVehicle);
                movementSpeed = movementSpeedVehicleFactor * getTileVelocity();
                usingVehicle = true;
            }
            else if(usingVehicle)
            //to leave the vehicle
            {
                generatedVehicle = Instantiate(vehiclePrefab, transform.position, Quaternion.identity);
                transform.position += vehicleOffsetPos;
                movementSpeed = movementSpeedWalkingFactor * getTileVelocity();
                usingVehicle = false;
            }
            else{}
        }
    }


}
