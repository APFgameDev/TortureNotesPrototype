
using Annotation.SO.UnityEvents;
using System.Collections.Generic;
using UnityEngine;

namespace Annotation
{

    [RequireComponent(typeof(LineRenderer))]
    public class Laser : MonoBehaviour
    {

        #region Laser Inputs
        [SerializeField]
        private SO.Vector2Variable m_thumbAxis;
        [SerializeField]
        private Laser m_otherLaser;
        [SerializeField]
        private SO.InputEventGroup m_triggerInputEventGroup;
        [SerializeField]
        private SO.InputEventGroup m_gripInputEventGroup;

        public Vector2 GetThumbAxisValue { get { return m_thumbAxis.Value; } }
        public Laser GetOtherLaser { get { return m_otherLaser; } }
        public SO.InputEventGroup GetTriggerEvents { get { return m_triggerInputEventGroup; } }
        public SO.InputEventGroup GetGripEvents { get { return m_gripInputEventGroup; } }
        #endregion

        [SerializeField]
        [Range(0.1f, 60f)]
        private float m_maxLaserDistance = 20f;

        public float GetMaxLaserDistance {get { return m_maxLaserDistance; } }

        private LineRenderer m_lineRenderer;

        #region VR Interactable variables
        private VRInteractable m_heldInteractable;
        private VRInteractable m_closestInteractable;
        private VRInteractionData m_vrInteractionData;
        #endregion

        public static readonly Color DEFAULT_LASER_COLOR = new Color(1, 0, 0);

        private void Awake()
        {
            //set up vr interaction data
            {
                m_vrInteractionData.m_handTrans = transform;
                m_vrInteractionData.m_laser = this;
                m_vrInteractionData.ChangeColor = ChangeLaseColor;
                m_vrInteractionData.GetClosestLaserPointOnPlane = CalculateClosestPointOnLaserFromInteractableOnPlane;
                m_vrInteractionData.GetClosestLaserPoint = CalculateClosestPointOnLaserFromInteractable;
            }

            //set up input event callbacks
            {
                m_triggerInputEventGroup.OnPressed.UnityEvent.AddListener(OnClickPressed);
                m_triggerInputEventGroup.OnHeld.UnityEvent.AddListener(OnClickHeld);
                m_triggerInputEventGroup.OnReleased.UnityEvent.AddListener(OnClickReleased);

                m_gripInputEventGroup.OnPressed.UnityEvent.AddListener(OnSecondaryClickPressed);
                m_gripInputEventGroup.OnHeld.UnityEvent.AddListener(OnSecondaryClickHeld);
                m_gripInputEventGroup.OnReleased.UnityEvent.AddListener(OnSecondaryClickReleased);
            }

            // set up line renderer
            {
                if (m_lineRenderer == null)
                    m_lineRenderer = GetComponent<LineRenderer>();
                m_lineRenderer.positionCount = 2;
            }

            //set laser to default color
            ChangeLaseColor(DEFAULT_LASER_COLOR);
        }
        void Update()
        {
            Vector3 laserStartPointWorld = transform.position;

            {
                float closestInteractableDist = m_maxLaserDistance;

                // call hover enter and exits 
                // only if there is no interactable held or stick hover is false
                if ( m_heldInteractable == null || 
                    (m_closestInteractable != null && m_closestInteractable.StickyHover == false) )
                {
                    //used to find closest interactable
                    VRInteractable closestInteractable = null;

                    //raycast
                    RaycastHit[] hit;
                    Ray ray = new Ray(transform.position, transform.forward);
                    hit = Physics.RaycastAll(ray, m_maxLaserDistance);

                    //cycle through all hits
                    for (int i = 0; i < hit.Length; i++)
                    {
                        VRInteractable vrInteractable = hit[i].transform.GetComponent<VRInteractable>();

                        // validation for interactable
                        if (vrInteractable != null && vrInteractable.enabled)
                        {
                            vrInteractable.m_hitPoint = hit[i].point;

                            //find closest interactable calculation
                            if (hit[i].distance < closestInteractableDist)
                            {
                                closestInteractable = vrInteractable;
                                closestInteractableDist = hit[i].distance;
                            }
                        }
                    }

                    if (closestInteractable != m_closestInteractable)
                    {
                        if (closestInteractable != null)
                            closestInteractable.OnHoverEnter(m_vrInteractionData);
                        if (m_closestInteractable != null)
                            m_closestInteractable.OnHoverExit(m_vrInteractionData);

                        m_closestInteractable = closestInteractable;
                    }
                }


                //Set LineRenderer Positions
                m_lineRenderer.SetPosition(0, laserStartPointWorld);
                m_lineRenderer.SetPosition(1, transform.position + transform.forward * closestInteractableDist);
            }
        }

        public void ForceHoldObject(VRInteractable vRInteractable,bool callReleaseOnAlreadyHeld = true)
        {
            //We are holding a Interactable
            if (m_heldInteractable != null && callReleaseOnAlreadyHeld == true)
                m_heldInteractable.OnClickRelease(m_vrInteractionData);

            m_heldInteractable = vRInteractable;

            if (m_heldInteractable != null)
                m_heldInteractable.OnClick(m_vrInteractionData);
        }

        #region InputCallBacks

        void OnClickPressed()
        {
            WhenClicked((vrI) => vrI.OnClick(m_vrInteractionData), WhatIsHold.Primary);
        }

        void OnClickHeld()
        {
            WhenHeld((vrI) => vrI.OnClickHeld(m_vrInteractionData));
        }

        void OnClickReleased()
        {
            WhenReleased((vrI) => vrI.OnClickRelease(m_vrInteractionData), WhatIsHold.Primary);
        }

        void OnSecondaryClickPressed()
        {
            WhenClicked((vrI) => vrI.OnSecondaryClick(m_vrInteractionData), WhatIsHold.Secondary);
        }

        void OnSecondaryClickHeld()
        {
            WhenHeld((vrI) => vrI.OnSecondaryClickHeld(m_vrInteractionData));
        }

        void OnSecondaryClickReleased()
        {
            WhenReleased((vrI) => vrI.OnSecondaryClickRelease(m_vrInteractionData), WhatIsHold.Secondary);
        }

        #region WhenFunctions

        void WhenClicked(System.Action<VRInteractable> callBack, WhatIsHold whatIsHold)
        {
            if (m_heldInteractable == null)
            {
                if (m_closestInteractable != null)
                {
                    callBack(m_closestInteractable);

                    if (m_closestInteractable.GetWhatIsHold() == whatIsHold)
                        m_heldInteractable = m_closestInteractable;
                }
            }
            else
                m_heldInteractable.OnSecondaryClick(m_vrInteractionData);
        }

        void WhenHeld(System.Action<VRInteractable> callBack)
        {
            if (m_heldInteractable == null)
            {
                if (m_closestInteractable != null)
                    callBack(m_closestInteractable);
            }
            else
                callBack(m_heldInteractable);
        }

        void WhenReleased(System.Action<VRInteractable> callBack, WhatIsHold whatIsHold)
        {
            if (m_heldInteractable == null)
            {
                if (m_closestInteractable != null)
                    callBack(m_closestInteractable);
            }
            else
            {
                callBack(m_heldInteractable);

                if (m_heldInteractable.GetWhatIsHold() == whatIsHold)
                    m_heldInteractable = null;
            }
        }
        #endregion
        #endregion

        #region VRInteractionDataCallBacks

        Vector3 CalculateClosestPointOnLaserFromInteractable(Vector3 pos)
        {
            return transform.position + Vector3.Project(pos - transform.position, transform.forward);
        }

        Vector3 CalculateClosestPointOnLaserFromInteractableOnPlane(Vector3 pos, Vector3 normal)
        {
            return pos + Vector3.ProjectOnPlane(CalculateClosestPointOnLaserFromInteractable(pos) - pos, normal);
        }

        void ChangeLaseColor(Color color)
        {
            m_lineRenderer.material.SetColor("_Color", color);
        }

        #endregion
    }
}