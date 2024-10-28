using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Components;
using UnityEngine;

public class OwnerNetworkAnimator : NetworkAnimator
{
    // Makes the network animator owner authoritative mode
    // This means owners update their animator state then the server will 
    // update the animation state and broadcast it to the other clients
    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }
}
