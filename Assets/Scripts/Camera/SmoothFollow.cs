using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public Transform playerTransform;
    public float smoothCameraFollow;
    Vector3 velocity = Vector3.zero;

    Vector3 lockPosition;

    bool followPlayer;

    private void OnEnable()
    {
        followPlayer = false;
        EventRepository.OnTileEnter += LockToCenterOfTheTile;
        EventRepository.OnTileExit += FollowPlayer;
    }

    private void OnDisable()
    {
        EventRepository.OnTileEnter -= LockToCenterOfTheTile;
        EventRepository.OnTileExit -= FollowPlayer;
    }

    void LateUpdate()
    {
        SmoothFollowPlayer();
    }

    private void SmoothFollowPlayer()
    {
        var currentPos = this.transform.position;
        Vector3 targetPos;

        if (followPlayer)
            targetPos = playerTransform.position;
        else
            targetPos = lockPosition;

        //float targetXPos = gameData.playerPosition.x;

        Vector3 newPos = Vector3.SmoothDamp(currentPos, targetPos, ref velocity, smoothCameraFollow * Time.deltaTime);
        this.transform.position = newPos;
    }

    void LockToCenterOfTheTile(object sender, SpecialTileEventArgs e)
    {
        followPlayer = false;
        lockPosition = new Vector3(e.targetPosition.x, 0f, e.targetPosition.z);

        //Debug.Log($"{e.targetPosition}");
    }

    void FollowPlayer(object sender, SpecialTileEventArgs e)
    {
        followPlayer = true;
    }


}
