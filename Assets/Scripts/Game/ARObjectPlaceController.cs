using Messaging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARObjectPlaceController : MonoBehaviour
{
    [SerializeField] private ARRaycastManager _rayCastManager;

    // TODO change this to an object picker so we can place different objects.
    [SerializeField] private GameObject _selectedObjectToPlace;

    private List<ARRaycastHit> _hitResults = new List<ARRaycastHit>();

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount <= 0)
        {
            return;
        }

        var touch = Input.GetTouch(0);
        _rayCastManager.Raycast(touch.position, _hitResults, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon);

        if (_hitResults.Count <= 0)
        {
            return;
        }

        var firstHitPose = _hitResults[0].pose;

        Instantiate(_selectedObjectToPlace, firstHitPose.position, firstHitPose.rotation);
        MessageBus.Get().Publish(new ToastMessage
        {
            title = "Object placed",
            message = $"At location {firstHitPose.position}"
        });
    }
}
