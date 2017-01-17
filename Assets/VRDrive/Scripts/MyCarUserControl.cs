using System;
using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.VR;

/// <summary>
/// My car user control.
/// </summary>
namespace UnityStandardAssets.Vehicles.Car {
	[RequireComponent(typeof(MyCarController))]
	public class MyCarUserControl : MonoBehaviour {

		private bool keyboardMode = false;
		private bool handleMode = true;
		private bool pedalMode = true;
		private bool pseudoPedalMode = false;

		private MyCarController m_Car;
		private bool push = true;
		private float delay = 0.2f;

		/// <summary>
		/// Awake this instance.
		/// </summary>
		void Awake() {
			// get the car controller
			m_Car = GetComponent<MyCarController>();
		}

		/// <summary>
		/// Start this instance.
		/// </summary>
		void Start() {
			keyboardMode = GameController.instance.KeyboardMode;
			handleMode = GameController.instance.HandleMode;
			pedalMode = GameController.instance.PedalMode;
			pseudoPedalMode = GameController.instance.PseudoPedalMode;
		}

		/// <summary>
		/// Fixeds the update.
		/// </summary>
		void FixedUpdate() {
			// LR
			float h = 0;
			if(keyboardMode) {
				h = CrossPlatformInputManager.GetAxis("Horizontal");
			}
			else if(handleMode) {
				h = Input.GetAxis("Handle");
			}

			// Straight
			float v = 0;
			if(keyboardMode) {
				v = CrossPlatformInputManager.GetAxis("Vertical");
			}
			else if(pedalMode) {
				if (pseudoPedalMode &&  Input.GetAxis("Accel") != 0) {
					v = 1f;
				}
				else {
					v = Input.GetAxis("Accel") * (-1f);
					v = (v + 1f) * 0.5f;
				}
			}

			// Brake
			float s = 0;
			if(keyboardMode) {
				s = CrossPlatformInputManager.GetAxis("Space");
			}
			else if(pedalMode) {
				if (pseudoPedalMode && Input.GetAxis ("Brake") != 0) {
					s = 1f;
				}
				else {
					s = Input.GetAxis("Brake") * (-1f);
					s = (s + 1f) * 0.5f;
				}
			}

			// Back = backtrigger + straight
			bool b = CrossPlatformInputManager.GetButton("BackTrigger");

			// Decide
			bool d = CrossPlatformInputManager.GetButtonUp("Decide");

            // Reset Orientation
            if (CrossPlatformInputManager.GetButton("Reset"))
            {
                InputTracking.Recenter();
            }

            if (s > 0) {
				m_Car.Move(h, 0, 0, s); // Stop
			}
			else {
				if(v == 0) {
					m_Car.Move(h, 0, 0, 0); // Do nothing
				}
				else {
					if(!b) {
						m_Car.Move(h, v, 0, 0); // Go
					}
					else {
						m_Car.Move(h, 0, (-1) * v, 0); // Back
					}
				}
			}

			if(d && push) {
				push = false;
				GameController.instance.ChangeGameScene(gameObject.name);
				StartCoroutine(PreventSuccessionPush());
			}
		}

		/// <summary>
		/// Prevents the succession push.
		/// </summary>
		/// <returns>The succession push.</returns>
		private IEnumerator PreventSuccessionPush() {
			yield return new WaitForSeconds(delay);
			push = true;
		}
	}
}
