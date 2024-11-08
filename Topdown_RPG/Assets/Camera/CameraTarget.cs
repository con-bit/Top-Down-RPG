using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    /// <summary>
    /// Tell the camera to follow this entity.
    /// </summary>
    public void ActivateCameraTarget()
    {
        CameraFollower follower = FindObjectOfType<CameraFollower>();
        follower.FollowTarget = true;
        follower.SetTarget(transform);
    }
}
