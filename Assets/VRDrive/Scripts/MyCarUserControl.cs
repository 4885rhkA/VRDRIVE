using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Car
{
	[RequireComponent(typeof (MyCarController))]
	public class MyCarUserControl : MonoBehaviour
	{
		private MyCarController m_Car; // the car controller we want to use

		private void Awake()
		{
			// get the car controller
			m_Car = GetComponent<MyCarController>();
		}

		private void FixedUpdate()
		{
			float h = Input.GetAxis("Handle");
			if (h == 0) {
				h = CrossPlatformInputManager.GetAxis("Horizontal");
			}
			float v = Input.GetAxis("Accel") * (-1f);
			if (v < 0) {
				v = CrossPlatformInputManager.GetAxis ("Vertical");
			} else {
				v = (v  + 1f) * 0.5f;
			}
			float b = Input.GetAxis("Brake") * (-1f);
			if (b < 0) {
				b = CrossPlatformInputManager.GetAxis("Space");
			} else {
				b = (b  + 1f) * 0.5f;
			}
			if(b == 0) {
				if(v > 0) {
					m_Car.Move(h, v, v, 0);
				}
				else {
					m_Car.Move(h, 0, 0, 0);
				}
			}
			else {
				if(v < 0) {
					m_Car.Move(h, v, v, 0);
				}
				else {
					m_Car.Move(h, 0, 0, b);
				}
			}
		}
	}
}
