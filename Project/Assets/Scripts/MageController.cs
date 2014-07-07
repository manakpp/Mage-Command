//  
//  File Name   :   PlayerController.cs
//


// Namespaces
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MageController : MonoBehaviour
{
	// Member Properties

	public LayerMask m_touchLayerMask;


	// Member Fields

	TKTapRecognizer m_tapRecognizer;
	TKLongPressRecognizer m_tapAndHoldRecognizer;
	TKSwipeRecognizer m_swipeRecognizer;
	Mage m_mage;

	// Member Methods

	void Awake()
	{
		m_tapRecognizer = new TKTapRecognizer();
		m_tapRecognizer.gestureRecognizedEvent += (r) =>
		{
			OnTap(r);
		};
		TouchKit.addGestureRecognizer(m_tapRecognizer);

		m_tapAndHoldRecognizer = new TKLongPressRecognizer();
		m_tapAndHoldRecognizer.gestureRecognizedEvent += (r) =>
		{
			OnTapAndHold(r);
		};
		TouchKit.addGestureRecognizer(m_tapAndHoldRecognizer);

		m_swipeRecognizer = new TKSwipeRecognizer();
		m_swipeRecognizer.gestureRecognizedEvent += (r) =>
		{
			OnSwipe(r);
		};
		TouchKit.addGestureRecognizer(m_swipeRecognizer);

	}


	void Start()
	{
		m_mage = GameObject.FindGameObjectWithTag("Mage").GetComponent<Mage>();	
	}


	void OnTap(TKTapRecognizer _tap)
	{
		Ray ray = Camera.main.ScreenPointToRay(_tap.touchLocation());
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 1000.0f, m_touchLayerMask.value))
		{
			m_mage.OnTap(hitInfo.point);
		}
	}


	void OnTapAndHold(TKLongPressRecognizer _tap)
	{
		Ray ray = Camera.main.ScreenPointToRay(_tap.touchLocation());
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 1000.0f, m_touchLayerMask.value))
		{
			m_mage.OnTapAndHold(hitInfo.point);
		}
	}


	void OnSwipe(TKSwipeRecognizer _swipe)
	{
		Vector3 start = Vector3.zero;
		Vector3 end = Vector3.zero;

		Ray ray = Camera.main.ScreenPointToRay(_swipe.startPoint);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 1000.0f, m_touchLayerMask.value))
		{
			start = hitInfo.point;
		}

		ray = Camera.main.ScreenPointToRay(_swipe.endPoint);
		if (Physics.Raycast(ray, out hitInfo, 1000.0f, m_touchLayerMask.value))
		{
			end = hitInfo.point;
		}

		if(start != Vector3.zero && end != Vector3.zero)
		{
			switch (_swipe.completedSwipeDirection)
			{
				case TKSwipeDirection.Left:
					{
						m_mage.OnSwipeLeft(start, end, _swipe.swipeVelocity);
					}
				 break;
				case TKSwipeDirection.Right:
					{
						m_mage.OnSwipeRight(start, end, _swipe.swipeVelocity);
					}
				 break;
				case TKSwipeDirection.Up:
					{
						m_mage.OnSwipeUp(start, end, _swipe.swipeVelocity);
					}
				 break;
				case TKSwipeDirection.Down:
					{
						m_mage.OnSwipeDown(start, end, _swipe.swipeVelocity);
					}
				 break;
				default:
				 break;
			}
		}
	}


};
