using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    #region Player Variables
    [Tooltip("Rotation speed of the player")]
    [SerializeField]
    float rotationSpeed = 5f;
    #endregion

    #region Laser Variables
    [Tooltip("Line renderer component used to render the 'laser'.")]
    LineRenderer laserSight;

    [Tooltip("Set this to whatever you like so long as min is zero")]
    [Range(0, 50)] 
    [SerializeField]
    private float laserLength;

    [Tooltip("Set this to whatever you like so long as min is above zero")]
    [Range(0.001f, 5)]
    [SerializeField]
    private float laserWidth;
    #endregion

    private void Awake()
    {
        InitialiseLaser();
    }
    private void Update()
    {
        MoveLaser();
        RotatePlayer();
    }

    void InitialiseLaser()
    {
        laserSight = GetComponentInChildren<LineRenderer>();
        laserSight.startWidth = laserWidth;
        laserSight.endWidth = laserWidth;
    }

    void RotatePlayer()
    {
        Vector3 horizontalRotationAngle = Input.GetAxisRaw("Horizontal") * Vector3.up;
        Vector3 verticalRotationAngle = Input.GetAxisRaw("Vertical") * Vector3.right;
        Vector3 finalRotation = (horizontalRotationAngle + verticalRotationAngle) * rotationSpeed * Time.deltaTime;
        transform.Rotate(finalRotation);
    }

    /// <summary>
    /// Responsible for set the new positions of the line renderer
    /// </summary>
    void MoveLaser()
    {
        //Initialise laser positions and rotation
        Vector3 laserRootPosition = laserSight.transform.position;      //Root position of the laser attached to the player
        Vector3 laserEndPosition;                                       //Final position of the laser, not yet set
        Vector3 fwd = laserSight.transform.forward;                     //Forward transform of the laser

        //Set raycast
        RaycastHit hit;   
        bool isHit = Physics.Raycast(laserRootPosition, fwd, out hit, laserLength);   //Boolean to determine if the laser hits something within laser length, could use Mathf.Inifity instead of laserLength but your laser's distance could hit a very far off object in this case

        laserEndPosition = isHit ? hit.point : laserRootPosition + (fwd * laserLength); //If statement: (IF) the isHit is true => Set final position of laser equal to the point hit, (ELSE) set the point to the laser's length forward of it's root position

        Vector3[] laserEnds = new Vector3[] { laserRootPosition, laserEndPosition };  //Vector3 array holding the laser positions

        laserSight.SetPositions(laserEnds);     //Set the lasers position 
    }
}
