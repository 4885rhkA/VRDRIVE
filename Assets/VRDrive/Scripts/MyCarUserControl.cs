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
			// pass the input to the car!
			float h = Input.GetAxis("Handle");
			if (h == 0) {
				h = CrossPlatformInputManager.GetAxis("Horizontal");
			}
			float v = CrossPlatformInputManager.GetAxis("Vertical");
			#if !MOBILE_INPUT
			float b = CrossPlatformInputManager.GetAxis("Space");
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
			#else
			m_Car.Move(h, v, v, 0f);
			#endif
		}
	}
}
