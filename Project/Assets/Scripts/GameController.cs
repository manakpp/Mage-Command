//  
//  File Name   :   PlayerController.cs
//


// Namespaces
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GameController : MonoBehaviour
{
	// Member Properties

	public LayerMask m_touchLayerMask;


	// Member Fields

	Mage m_mage;
	List<Tile> m_selectedTiles;

	// Member Methods

	void Awake()
	{
		m_selectedTiles = new List<Tile>();
	}


	void Start()
	{
		m_mage = GameObject.FindGameObjectWithTag("Mage").GetComponent<Mage>();
	}


	public enum GridTouchState
	{
		None,
		Began,		// Touch just started
		Pressed,	// Touch down since last frame
		Moved,		// Moved since last frame
		Held,		// Held down on one spot for specified time (different to Pressed)
		HeldMoved,	// Was held down and is now being moved (different to Moved)
		Ended,		// Touch was released
	}

	public enum GridSwipeDirection
	{
		None,
		Left,
		Right,
		Up,
		Down,
	}

	float m_touchTimeAccum;
	float m_timeForHold = 0.25f;
	GridTouchState m_touchState;
	GridSwipeDirection m_swipeDirection;
	Vector2 m_startPosition;
	Vector2 m_tapAndHoldMotionThreshold = new Vector2(5.0f, 5.0f);
	Vector2 m_posDeltaLastFrame;
	void Update()
	{
		Vector2 touchPosition = Input.mousePosition;

		// Check for pressed
		if (Input.GetMouseButtonDown(0))
		{
			// Need to 
			if (m_touchState == GridTouchState.None || m_touchState == GridTouchState.Ended)
			{
				m_touchTimeAccum = 0.0f;
				m_touchState = GridTouchState.Began;
				m_startPosition = Input.mousePosition;

				
			}
		}

		// Check for held down
		else if (Input.GetMouseButton(0))
		{
			if (m_touchState == GridTouchState.Began ||
				m_touchState == GridTouchState.Pressed)
			{
				// Touch time is accumulated to check for Hold
				m_touchTimeAccum += Time.deltaTime;
				if (m_touchTimeAccum > m_timeForHold)
				{
					OnHold(touchPosition);
					m_touchTimeAccum = m_timeForHold;
					m_touchState = GridTouchState.Held;
				}

				
				// Check for motion (for swipes)
				Vector2 posDelta = m_startPosition - touchPosition;
				if (Mathf.Abs(posDelta.x) > m_tapAndHoldMotionThreshold.x ||
					Mathf.Abs(posDelta.y) > m_tapAndHoldMotionThreshold.y)
				{
					// Check for next selected tile
					OnTouchMoveStart(touchPosition);

					if (m_swipeDirection != GridSwipeDirection.None)
						m_touchState = GridTouchState.Moved;

					m_posDeltaLastFrame = posDelta;
				}
			}
			else if (m_touchState == GridTouchState.Moved)
			{
				Vector2 posDelta = m_startPosition - touchPosition;
				if (m_posDeltaLastFrame != posDelta)
				{
					OnTouchMove(touchPosition);

					if (m_selectedTiles.Count == 0)
						m_touchState = GridTouchState.None;
				}

				m_posDeltaLastFrame = posDelta;
			}
			else if (m_touchState == GridTouchState.Held)
			{
				// Check for motion (motion will override the hold gesture)
				Vector2 posDelta = m_startPosition - touchPosition;
				if (Mathf.Abs(posDelta.x) > m_tapAndHoldMotionThreshold.x ||
					Mathf.Abs(posDelta.y) > m_tapAndHoldMotionThreshold.y)
				{
					// This cancels holding
					OnHoldAndMove(touchPosition);
					m_touchState = GridTouchState.HeldMoved;

					m_posDeltaLastFrame = posDelta;
				}
			}
			else if (m_touchState == GridTouchState.HeldMoved)
			{
				Vector2 posDelta = m_startPosition - touchPosition;
				if (m_posDeltaLastFrame != posDelta)
				{
					OnHoldAndMove(touchPosition);

					if (m_selectedTiles.Count == 0)
						m_touchState = GridTouchState.None;
				}

				m_posDeltaLastFrame = posDelta;
			}
		}
		
		// Check for release
		else if (Input.GetMouseButtonUp(0))
		{
			if (m_touchTimeAccum >= m_timeForHold)
				OnHoldRelease(touchPosition);
			else if (m_touchState == GridTouchState.Moved)
				OnSwipe(touchPosition);
			else if (m_touchState != GridTouchState.Pressed)
				OnTap(touchPosition);

			m_swipeDirection = GridSwipeDirection.None;
			m_touchState = GridTouchState.Ended;
		}
		else
		{
			m_touchState = GridTouchState.None;
		}

	}


	void OnTouchStart(Vector2 _touchPostion)
	{
		var tile = CheckForTile(_touchPostion);
		if (tile == null)
			return;

		m_selectedTiles.Add(tile);
	}


	void OnTouchMoveStart(Vector2 _touchPostion)
	{
		var tile = CheckForTile(_touchPostion);
		if (tile == null)
			return;

		if (m_selectedTiles.Exists(x => x == tile))
			return;
		
		HoldTile(tile);

		if (m_selectedTiles.Count > 1)
		{
			// Only need two tile to determine direction
			Vector3 tilePosA = m_selectedTiles[0].transform.position;
			Vector3 tilePosB = m_selectedTiles[1].transform.position;
			Vector3 direction = tilePosB - tilePosA;

			if (direction.x < 0.0f)
				m_swipeDirection = GridSwipeDirection.Left;
			else if (direction.x > 0.0f)
				m_swipeDirection = GridSwipeDirection.Right;
			else if (direction.z < 0.0f)
				m_swipeDirection = GridSwipeDirection.Down;
			else if (direction.z > 0.0f)
				m_swipeDirection = GridSwipeDirection.Up;
			else
				Debug.Log("WHAT HAPPEN!? " + direction);
		}
	}

	void OnTouchMove(Vector2 _touchPostion)
	{
		var tile = CheckForTile(_touchPostion);
		if (tile == null)
			return;

		if (m_selectedTiles.Exists(x => x == tile))
			return;

		int count = m_selectedTiles.Count;
		if (count < 2)
			return;

		// Check theyre in the same direction
		Vector3 tilePosA = m_selectedTiles[count - 1].transform.position;
		Vector3 tilePosB = tile.transform.position;
		Vector3 direction = tilePosB - tilePosA;

		GridSwipeDirection swipeDirection = GridSwipeDirection.None;
		
		if (direction.x < 0.0f)
			swipeDirection = GridSwipeDirection.Left;
		else if (direction.x > 0.0f)
			swipeDirection = GridSwipeDirection.Right;
		else if (direction.z < 0.0f)
			swipeDirection = GridSwipeDirection.Down;
		else if (direction.z > 0.0f)
			swipeDirection = GridSwipeDirection.Up;
		else
			Debug.Log("WHAT HAPPEN!? " + direction);

		if(swipeDirection == m_swipeDirection
			&& direction.sqrMagnitude < 5.0f)
			HoldTile(tile);
	}


	void OnTap(Vector2 _touchPostion)
	{
		var tile = CheckForTile(_touchPostion);
		if (tile == null)
			return;

		TapTile(tile);

		m_mage.OnTap(tile.transform.position);
	}

	void OnHold(Vector2 _touchPostion)
	{
		var tile = CheckForTile(_touchPostion);
		if (tile == null)
			return;

		HoldTile(tile);

		m_mage.OnTapAndHold(tile.transform.position);
	}

	void OnSwipe(Vector2 _touchPostion)
	{
		DeselectAllTiles();
	}


	void OnHoldAndMove(Vector2 _touchPostion)
	{
		if (m_selectedTiles.Count == 0)
			return;

		var tile = CheckForTile(_touchPostion);
		if (tile == null)
			return;

		if (m_selectedTiles[0] != tile)
			DeselectAllTiles();
	}


	void OnHoldRelease(Vector2 _touchPostion)
	{

		DeselectAllTiles();

		//m_mage.OnTapAndHoldEnded(tile.transform.position);
	}


	Tile CheckForTile(Vector2 _touchPosition)
	{
		Ray ray = Camera.main.ScreenPointToRay(_touchPosition);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 1000.0f, m_touchLayerMask.value))
			return hitInfo.collider.gameObject.GetComponent<Tile>();

		return null;
	}


	void TapTile(Tile _tile)
	{
		Color color = new Color(0.9f, 0.9f, 0.7f);
		_tile.Highlight(color, 0.1f);
	}


	void HoldTile(Tile _tile)
	{
		Color color = new Color(0.9f, 0.9f, 0.7f);
		_tile.Highlight(color, 0.0f);
		m_selectedTiles.Add(_tile);
	}


	void DeSelectTile(Tile _tile)
	{
		_tile.UnHighlight();
		m_selectedTiles.Remove(_tile);
	}


	void DeselectAllTiles()
	{
		while (m_selectedTiles.Count != 0)
		{
			DeSelectTile(m_selectedTiles[0]);
		}
	}

};
