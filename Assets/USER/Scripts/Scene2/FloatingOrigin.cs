// FloatingOrigin.cs
// Written by Peter Stirling
// 11 November 2010
// Uploaded to Unify Community Wiki on 11 November 2010
// Updated to Unity 5.x particle system by Tony Lovell 14 January, 2016
// fix to ensure ALL particles get moved by Tony Lovell 8 September, 2016
// URL: http://wiki.unity3d.com/index.php/Floating_Origin

//Modified further by MirrorFish - Feb 23, 2020 (https://youtu.be/qIxifMcvYTs)
//Modiefied further by Moz - Feb 20, 2023

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(Camera))]
public class FloatingOrigin : MonoBehaviour
{
    public float threshold = 100.0f;
    public LevelGeneration levelLayoutGenerator;

    private void LateUpdate()
    {
        //Child the camera to the player, then this will work
        Vector3 currCamPos = gameObject.transform.position;
        currCamPos.y = 0;

        if(currCamPos.magnitude > threshold)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                foreach(GameObject go in SceneManager.GetSceneAt(i).GetRootGameObjects())
                {
                    //Moving things back towards the origin
                    go.transform.position -= currCamPos;
                }
            }

            Vector3 originDelta = Vector3.zero - currCamPos;
            levelLayoutGenerator.UpdateSpawnOrigin(originDelta);
            //Debug.Log("recentring, current origin delta = " + originDelta);
        }
    }
}
