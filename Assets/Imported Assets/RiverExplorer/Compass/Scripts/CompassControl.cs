/**
 * Copyright (c) 2024 RiverExplorer▒ Games LLC, All Rights Reserved
 *
 * Licensed under the RiverExplorer Commercial Software Library License, you may

 * not use this file except in compliance with the License. You may obtain
 * a copy of the License at
 *
 * The purchaser of the software is granted the permission to use and incorporate it
 * into the purchaser's products for distribution, either commercially or non-commercially,
 * as long as the following conditions are met:
 *
 *  https://RiverExplorer.games/CommercialAssetLicense.html
 *
 * - The purchaser must not distribute the software source code outside of the purchaser's company or organization.
 *
 * - The purchaser must not distribute the software's precompiled binaries or other assets outside of the purchaser's company
 * or organization, except as part of an application executable.
 *
 * - The purchaser must not attempt to reverse-engineer or decompile the software's precompiled code.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
 * PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR
 * ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
 * ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 *
 * Unless required by applicable law or agreed to in writing, software  distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions
 * and imitations under the License.
 *
 * FULL Documentation can be found at:
 *
 *  https://RiverExplorer.games/Documentation/Unity/Assets/CompassHUDImage.html
 */

using UnityEngine;
using UnityEngine.UI;

using TMPro;

namespace RiverExplorer.HUD.Compass
{
    /**
     * This class controls the compass display.
     */
    public class CompassControl 
        : MonoBehaviour
    {
        [Tooltip("The object to track for heading, Roll and elevation.")]
        [SerializeField] GameObject Track;

        [Header("Compass Display Options")]
        [Tooltip ("Show the heading in degrees.")]
        [SerializeField] bool ShowHeading = true;
        [Tooltip("The dead zone for the heading delegate. Movement less than this value will not be reported. (zero is all)")]
        [SerializeField] public float HeadingDeadZone = 0.0f;

        [Header("Pitch Display Options")]
        [Tooltip("Show the pitch in degrees.")]
        [SerializeField] bool ShowPitch = true;
        [Tooltip("The dead zone for the pitch delegate. Movement less than this value will not be reported. (zero is all)")]
        [SerializeField] public float PitchDeadZone = 0.0f;

        [Header("Roll Display Options")]
        [Tooltip("Show the Roll in degrees.")]
        [SerializeField] bool ShowRoll = true;
        [Tooltip("The dead zone for the Roll delegate. Movement less than this value will not be reported. (zero is all)")]
        [SerializeField] public float RollDeadZone = 0.0f;

        /**
         * The slider to display the elevation.
         */
        GameObject PitchIndicator;

        /**
         * The slider to display the Roll.
         */
        GameObject RollIndicator;


        /**
         * The text to display the heading.
         */
        TextMeshProUGUI HeadingTMPro;

        /**
         * The text to display the elevation.
         */
        TextMeshProUGUI PitchTMPro;

        /**
         * The text to display the Roll.
         */
        TextMeshProUGUI RollTMPro;

        /**
         * The text labels for the compass directions.
         */
        TextMeshProUGUI North;
        TextMeshProUGUI West;
        TextMeshProUGUI South;
        TextMeshProUGUI East;

        GameObject NorthWestSpot;
        GameObject NorthEastSpot;
        GameObject SouthWestSpot;
        GameObject SouthEastSpot;

        TextMeshProUGUI NorthWest;
        TextMeshProUGUI NorthEast;
        TextMeshProUGUI SouthWest;
        TextMeshProUGUI SouthEast;

        /**
         * The image to rotate to match the heading.
         */
        GameObject CenterCompass;

        /**
         * The heading of the tracked object changed.
         * Changes greater than the HeadingDeadZone will be reported.
         */
        public delegate void HeadingChanged(float Heading);
        public event HeadingChanged OnHeadingChanged;

        /**
         * The heading of the tracked object.
         */
        public float Heading
        {
            get; private set;
        }
        private float _LastHeading = 0.0f;

        /**
         * The heading of the tracked object changed.
         * Changes greater than the PitchDeadZone will be reported.
         */
        public delegate void PitchChanged(float Elevation);
        public event PitchChanged OnPitchChanged;

        public float Pitch
        {
            get; private set;
        }
        private float _LastPitch = 0.0f;

        /**
         * The heading of the tracked object changed.
         * Changes greater than the RollDeadZone will be reported.
         */
        public delegate void RollChanged(float Roll);
        public event RollChanged OnRollChanged;

       public float Roll
        {
            get; private set;
        }
        private float _LastRoll = 0.0f;

        private void Awake()
        {

            // >>>> If Track is not set, nothing will work. <<<
            //
            if (Track == null) {
                Debug.LogError("Track object is not set.");

            } else {

                // Gather all of the parts of the compass.
                //
                HeadingTMPro = transform.Find("Heading").GetComponent<TextMeshProUGUI>();

                PitchIndicator = transform.Find("PitchIndicator").gameObject;
                PitchTMPro = transform.Find("PitchText").GetComponent<TextMeshProUGUI>();

                RollIndicator = transform.Find("RollIndicator").gameObject;
                RollTMPro = transform.Find("RollText").GetComponent<TextMeshProUGUI>();

                CenterCompass = transform.Find("CenterCompass").gameObject;

                North = transform.Find("CenterCompass/N").GetComponent<TextMeshProUGUI>();
                West = transform.Find("CenterCompass/W").GetComponent<TextMeshProUGUI>();
                South = transform.Find("CenterCompass/S").GetComponent<TextMeshProUGUI>();
                East = transform.Find("CenterCompass/E").GetComponent<TextMeshProUGUI>();

                NorthWestSpot = transform.Find("CenterCompass/NWSpot").gameObject;
                NorthEastSpot = transform.Find("CenterCompass/NESpot").gameObject;
                SouthWestSpot = transform.Find("CenterCompass/SWSpot").gameObject;
                SouthEastSpot = transform.Find("CenterCompass/SESpot").gameObject;

                NorthWest = transform.Find("NW").GetComponent<TextMeshProUGUI>();
                NorthEast = transform.Find("NE").GetComponent<TextMeshProUGUI>();
                SouthWest = transform.Find("SW").GetComponent<TextMeshProUGUI>();
                SouthEast = transform.Find("SE").GetComponent<TextMeshProUGUI>();

                _LastPitch = Track.transform.rotation.eulerAngles.x;
                _LastHeading = Track.transform.rotation.eulerAngles.y;
                _LastPitch = Track.transform.rotation.eulerAngles.z;
            }

            return;
        }


        void Update()
        {
   
            // PITCH
            //
            Pitch = Track.transform.rotation.eulerAngles.x;
            if (ShowPitch) {
                PitchIndicator.gameObject.SetActive(true);
                
                if (Pitch > 180) {
                    Pitch = Pitch - 360;
                } 

            } else {
                PitchIndicator.gameObject.SetActive(false);
            }
            if (Mathf.Abs(Pitch - _LastPitch) > PitchDeadZone) {
                float PitchChange = Pitch - _LastPitch;

                if (ShowPitch) {
                    PitchTMPro.text = (-Pitch).ToString("F2") + "°\nPitch";

                    PitchIndicator.transform.rotation = Quaternion.Euler(Pitch,
                                                45,
                                                PitchIndicator.transform.rotation.z);
                }
                OnPitchChanged?.Invoke(Pitch);
                _LastPitch = Pitch;
            }

            // HEADING
            //
            Heading = Track.transform.eulerAngles.y;
            if (Mathf.Abs(Heading - _LastHeading) > HeadingDeadZone) {
                this.HeadingTMPro.text = Heading.ToString("F2") + "°";
                OnHeadingChanged?.Invoke(Heading);
                _LastHeading = Heading;
                // Rotate the compass image to match the heading.
                //
                CenterCompass.transform.rotation = Quaternion.Euler(CenterCompass.transform.rotation.eulerAngles.x,
                                                                    33,
                                                                    Heading);
            }
           

            // Roll
            //
            Roll = Track.transform.rotation.eulerAngles.z;
            if (ShowRoll) {
                RollIndicator.gameObject.SetActive(true);
               
                if (Roll > 180) {
                    Roll = Roll - 360;
                }

            } else {
                RollIndicator.gameObject.SetActive(false);
            }
            if (Mathf.Abs(Roll - _LastRoll) > RollDeadZone) {
                float RollChange = Roll - _LastRoll;

                if (ShowRoll) {
                    RollTMPro.text = (-Roll).ToString("F2") + "°\nRoll";

                    RollIndicator.transform.Rotate(0, 0, RollChange);
                }
                OnRollChanged?.Invoke(Roll);
                _LastRoll = Roll;
            }

            North.text = "<rotate=\"" + (-Heading).ToString("F2") + "\">N</rotate>";
            West.text = "<rotate=\"" + (-Heading).ToString("F2") + "\">W</rotate>";
            South.text = "<rotate=\"" + (-Heading).ToString("F2") + "\">S</rotate>";
            East.text = "<rotate=\"" + (-Heading).ToString("F2") + "\">E</rotate>";

            NorthWest.transform.position = NorthWestSpot.transform.position;
            NorthEast.transform.position = NorthEastSpot.transform.position;
            SouthWest.transform.position = SouthWestSpot.transform.position;
            SouthEast.transform.position = SouthEastSpot.transform.position;

            return;
        }
    }
}
