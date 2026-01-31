using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public Transform playerTransform;
    Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        var currentPos = this.transform.position;
        var targetPos = playerTransform.position;
        //float targetXPos = gameData.playerPosition.x;

        Vector3 newPos = Vector3.SmoothDamp(currentPos, targetPos, ref velocity, 0.15f * Time.deltaTime);
        this.transform.position = newPos;
    }
}
