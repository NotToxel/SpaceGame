/**
 * RiverExplorer™ Games LLC.
 * 
 * Visit https://RiverExplorer.US/Learn to take some Unity classes.
 * Many are free.
 */
using UnityEngine;

using RiverExplorer.HUD.Compass;

namespace RiverExplorer.HUD.Compass.Demo
{

    /**
     * @class DemoControl
     * A simple demo script to show how to use the CompassControl.
     */
    public class DemoControl
        : MonoBehaviour
    {
        /**
         * @brief The HUD object that will be used to report the heading, elevation, and Roll.
         * You must have one in your scene to use this script.
         */
        [SerializeField] CompassControl HUD;
        [SerializeField] GameObject Model;

        [SerializeField] float XRange = 20.0f;
        [SerializeField] float ZRange = 20.0f;

        [SerializeField] float X = 0.0f;
        [SerializeField]float Y = 0.0f;
        [SerializeField]float Z = 0.0f;
        [SerializeField] float XIncrement = 0.05f;
        [SerializeField] float YIncrement = 0.1f;
        [SerializeField]float ZIncrement = 0.5f;

        bool DemoOn = true;
        float NextYawChange = 0.0f;

        private void Awake()
        {
            if (HUD == null) {
                /**
                 * If the HUD is not set in the inspector, then we can't do anything.
                 */
                Debug.LogError("HUD is not set in the inspector.");

            } else {

                /**
                 * Don't get elevation change notifications if the change is less than set here.
                 */
                HUD.PitchDeadZone = 0.1f;

                /**
                 * Register to get notifications when the heading changes.
                 */
                HUD.OnHeadingChanged += OnHeadingData;

                /**
                 * Don't get heading change notifications if the change is less than set here.
                 */
                HUD.HeadingDeadZone = 0.1f;
                /**
                * Register to get notifications when the elevation changes.
                */
                HUD.OnPitchChanged += OnElevationData;

                /**
                 * Don't get Roll change notifications if the change is less than set here.
                 */
                HUD.RollDeadZone = 0.1f;

                /**
                * Register to get notifications when the Roll (left/right angle) changes.
                */
                HUD.OnRollChanged += OnRollData;

                NextYawChange = Time.time + Random.Range(0.5f, 10.0f);
            }

            return;
        }

        /**
         * @brief Called when the Roll changes more than the dead zone.
         */
        private void OnRollData(float LeftRight)
        {
            //Debug.Log("Pitch: " + LeftRight);
        }

        /**
         * @brief Called when the heading changes more than the dead zone.
         */
        private void OnHeadingData(float Facing)
        {
            //Debug.Log("Heading: " + Facing);
        }

        /**
         * @brief Called when the elevation changes more than the dead zone.
         */
        private void OnElevationData(float UpDown)
        {
            //Debug.Log("Pitch: " + UpDown);
        }

        public void Update()
        {
            // All of this code in Update() is just some random movement to show the compass moving.
            // You don't need any of this in your code.
            //
            if (DemoOn) {
                X += XIncrement;
                if (X > XRange) {
                    X = XRange;

                    XIncrement *= -1;
                }
                if (X < -XRange) {
                    X = -XRange;
                    XIncrement *= -1;
                }

                Y += YIncrement;
                Y %= 360.0f;

                if (Time.time > NextYawChange) {
                    NextYawChange = Time.time + Random.Range(0.5f, 10.0f);
                    YIncrement *= -1;
                }

                Z += ZIncrement;
                if (Z > ZRange) {
                    Z = ZRange;
                    ZIncrement *= -1;
                }
                if (Z < -ZRange) {
                    Z = -ZRange;
                    ZIncrement *= -1;
                }

                Model.transform.rotation = Quaternion.Euler(X, Y, Z);

            }

            return;
        }

        /**
         * The user pressed the down button in the demo.
         */
        public void DownPressed()
        {
            Model.transform.Rotate(Vector3.right, Time.deltaTime * 100.0f);

            return;
        }

        /**
         * The user pressed the up button in the demo.
         */
        public void UpPressed()
        {
            Model.transform.Rotate(Vector3.left, Time.deltaTime * 100.0f);

            return;
        }


        /**
         * The user pressed the left button in the demo.
         */
        public void LeftPressed()
        {
            Model.transform.Rotate(Vector3.down, Time.deltaTime * 100.0f);
        }

        /**
         * The user pressed the right button in the demo.
         */
        public void RightPressed()
        {
            Model.transform.Rotate(Vector3.up, Time.deltaTime * 100.0f);

            return;
        }

        /**
         * The user pressed the demo toggle button in the demo.
         * Checked, it auto runs. Un-checked, the demo stops and user
         * presses the up, down, left, and right buttons to move the model.
         */
        public void DemoToggled()
        {
            DemoOn = !DemoOn;
            //Debug.Log("DemoOn: " + DemoOn);

            return;
        }
    }
}
