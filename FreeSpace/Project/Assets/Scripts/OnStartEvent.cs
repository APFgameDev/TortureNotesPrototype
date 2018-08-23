using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnStartEvent : MonoBehaviour {
    [SerializeField]
    UnityEvent onStartUpEvents;

	// Use this for initialization
	void Start () {
        onStartUpEvents.Invoke();

    }

}
