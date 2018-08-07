
using System.Collections.Generic;
using UnityEngine;

namespace Annotation
{

    [RequireComponent(typeof(LineRenderer))]
    public class Laser : MonoBehaviour
    {
        #region Laser Inputs
        [SerializeField]
        private SO.BoolVariable m_clickVar;
        [SerializeField]
        private SO.BoolVariable m_secondaryClickVar;
        [SerializeField]
        private SO.Vector2Variable m_thumbAxis;
        #endregion

        [SerializeField]
        private SO.BoolVariable m_otherLaserClickVar;

        [SerializeField]
        Transform m_otherHand;

        [SerializeField]
        [Range(0.1f, 60f)]
        private float m_maxLaserDistance = 20f;

        private LineRenderer m_lineRenderer;

        #region VR Interactable variables
        private VRInteractable m_heldInteractable;
        private VRInteractionData m_vrInteractionData;

        private List<VRInteractable> m_interactablesCollidedWithThisFrame = new List<VRInteractable>();
        private List<VRInteractable> m_interactablesCollidedWithLastFrame = new List<VRInteractable>();
        #endregion

        private bool m_isClicked = false;

        public static readonly Color DEFAULT_LASER_COLOR = new Color(1, 0, 0);

        private void Awake()
        {
            //set up vr interaction data
            {
                m_vrInteractionData.handTrans = transform;
                m_vrInteractionData.changeColor = ChangeLaseColor;
                m_vrInteractionData.GetClosestLaserPointOnPlane = CalculateClosestPointOnLaserFromInteractableOnPlane;
                m_vrInteractionData.GetClosestLaserPoint = CalculateClosestPointOnLaserFromInteractable;

            }

            // set up line renderer
            {
                if (m_lineRenderer == null)
                    m_lineRenderer = GetComponent<LineRenderer>();
                m_lineRenderer.positionCount = 2;
            }

            ChangeLaseColor(DEFAULT_LASER_COLOR);
        }
        void Update()
        {
            Vector3 laserStartPointWorld = transform.position;
            Vector3 maxLaserEndPointWorld = transform.position + transform.forward * m_maxLaserDistance;

            //Check click and secondary click Inputs
            bool isClickHeld = m_clickVar.Value;

            // add input data to m_VRInteractableData
            {
                m_vrInteractionData.secondaryClickPressed = m_secondaryClickVar.Value;

                // get joystick movement direction
                m_vrInteractionData.movementDirection = m_thumbAxis.Value;
            }

            m_interactablesCollidedWithThisFrame.Clear();

            //used to find closest interactable
            VRInteractable closestInteractable = null;
            float closestInteractableDist = float.MaxValue;


            //Check if we should Collect Vr Interactables collided with this frame
            //Check For On Hover Enters
            {
                // do raycast all if there is no interactable held
                if (m_heldInteractable == null)
                {
                    //raycast
                    RaycastHit[] hit;
                    Ray ray = new Ray(transform.position, transform.forward);
                    hit = Physics.RaycastAll(ray, m_maxLaserDistance);

                    m_interactablesCollidedWithThisFrame.Capacity = hit.Length;

                    //cycle through all hits
                    for (int i = 0; i < hit.Length; i++)
                    {
                        VRInteractable vrInteractable = hit[i].transform.GetComponent<VRInteractable>();

                        // validation for interactable
                        if (vrInteractable != null && vrInteractable.enabled)
                        {
                            vrInteractable.hitPoint = hit[i].point;

                            //find closest interactable calculation
                            if (hit[i].distance < closestInteractableDist)
                            {
                                closestInteractable = vrInteractable;
                                closestInteractableDist = hit[i].distance;
                            }
                            // add to interactable list collided with this frame
                            m_interactablesCollidedWithThisFrame.Add(vrInteractable);

                            //is the interactable a new collision we collected on this frame call on hover enter
                            if (m_interactablesCollidedWithLastFrame.Contains(vrInteractable) == false)
                                vrInteractable.OnHoverEnter(m_vrInteractionData);
                        }
                    }
                }
                //only add held interactable and set it as closest interactable
                else
                {
                    m_interactablesCollidedWithThisFrame.Add(m_heldInteractable);
                    closestInteractable = m_heldInteractable;
                    closestInteractableDist = Vector3.Distance(transform.position, m_heldInteractable.transform.position);
                }
            }

            //Set LineRenderer Positions
            {
                m_lineRenderer.SetPosition(0, laserStartPointWorld);

                // set lin end pos to closest interactalbe if there is on
                if (closestInteractable != null)
                    m_lineRenderer.SetPosition(1, transform.position + transform.forward * closestInteractableDist);
                else
                    m_lineRenderer.SetPosition(1, maxLaserEndPointWorld);
            }

            //Check For On Click
            {
                //Did we start holding click this frame?
                if (isClickHeld == true && m_isClicked == false)
                {
                    m_isClicked = true;
                    //call on click on our closest interactable
                    if (closestInteractable != null)
                    {
                        m_heldInteractable = closestInteractable;
                        closestInteractable.OnClick(m_vrInteractionData);
                    }
                }
                // did we stop holding click this frame?
                else if (isClickHeld == false && m_isClicked == true)
                    m_isClicked = false;
            }

            //Check For On Click Released or held
            {
                //We are holding a Interactable
                if (m_heldInteractable != null)
                {
                    // if click released release held interactable
                    if (isClickHeld == false)
                    {
                        m_heldInteractable.OnClickRelease(m_vrInteractionData);
                        m_heldInteractable = null;
                    }
                    // keep calling on click held
                    else
                        m_heldInteractable.OnClickHeld(m_vrInteractionData);
                }
            }

            //Check For On Hover Exits
            {
                //loop through our last frame collisions 
                for (int i = 0; i < m_interactablesCollidedWithLastFrame.Count; i++)
                {
                    //check if we are still colliding with them this frame call on hover exit if not
                    if (m_interactablesCollidedWithThisFrame.Contains(m_interactablesCollidedWithLastFrame[i]) == false)
                        m_interactablesCollidedWithLastFrame[i].OnHoverExit(m_vrInteractionData);
                }
            }

            //Update m_interactablesCollidedWithLastFrame List
            {
                m_interactablesCollidedWithLastFrame.Clear();
                m_interactablesCollidedWithLastFrame.AddRange(m_interactablesCollidedWithThisFrame);
            }
        }


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