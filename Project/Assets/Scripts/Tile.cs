//  
//  File Name   :   Tile.cs
//


// Namespaces
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Tile : MonoBehaviour
{

	// Member Types

	public enum TileLinkDirection
	{
		North = 0,
		South = 1,
		East = 2,
		West = 3,
		MAX
	}


	public enum PerpendicularDirection
	{
		Vertical,
		Horizontal,
	}


	// Member Delegates & Events


	// Member Properties

	public Tile[] Links
	{
		get { return m_tileLinks; }
	}


	public int Row
	{
		get { return m_row; }
		set { m_row = value; } 
	}


	public int Column
	{
		get { return m_column; }
		set { m_column = value; } 
	}


	// Member Fields

	Tile[] m_tileLinks;
	Color m_defaultColor;
	Color m_startColor;
	Material m_material;
	float m_duration;
	float m_timeAccum;
	private int m_row;
	private int m_column;
	

	// Member Methods

	public void Highlight(Color _color, float _duration)
	{
		m_startColor = _color;
		m_material.color = _color;
		m_duration = _duration;
		m_timeAccum = 0.0f;	
	}


	public void UnHighlight()
	{
		m_material.color = m_defaultColor;
	}


	void Awake()
	{
		m_material = renderer.material;
		m_defaultColor = m_material.color;
	}


	void Update()
	{
		if (m_timeAccum == m_duration)
			return;

		m_timeAccum += Time.deltaTime;

		if (m_timeAccum > m_duration)
			m_timeAccum = m_duration;

		m_material.color = Color.Lerp(m_startColor, m_defaultColor, m_timeAccum / m_duration);
	}


	public void MakeLinks(int _currentTileID, List<Tile> _tiles)
	{
		m_tileLinks = new Tile[4];

		for (TileLinkDirection direction = TileLinkDirection.North; direction < TileLinkDirection.MAX; ++direction)
		{
			m_tileLinks[(int)direction] = FindLinkedTile(_currentTileID, _tiles, direction);
		}
	}


	public static void GetLineStartEnd(Tile _startTile, GameController.GridSwipeDirection _direction, out Tile _start, out Tile _end)
	{
		List<Tile> tiles = new List<Tile>();

		// Find one direction
		Tile nextTile = _startTile;
		Tile lastFoundLinkedTile = _startTile;

		while (nextTile != null)
		{
			TileLinkDirection direction = ConvertSwipeDirectionToLinkDirection(_direction);
			nextTile = nextTile.Links[(int)direction];

			if(nextTile != null)
				lastFoundLinkedTile = nextTile;
		}

		_start = lastFoundLinkedTile;

		// Find the other direction
		nextTile = _startTile;
		lastFoundLinkedTile = _startTile;

		while (nextTile != null)
		{
			TileLinkDirection direction = SwitchDirection(ConvertSwipeDirectionToLinkDirection(_direction));
			nextTile = nextTile.Links[(int)direction];

			if (nextTile != null)
				lastFoundLinkedTile = nextTile;
		}

		_end = lastFoundLinkedTile;
	}


	public static TileLinkDirection ConvertSwipeDirectionToLinkDirection(GameController.GridSwipeDirection _direction)
	{
		switch (_direction)
		{
			case GameController.GridSwipeDirection.Left: return TileLinkDirection.West;
			case GameController.GridSwipeDirection.Right: return TileLinkDirection.East;
			case GameController.GridSwipeDirection.Up: return TileLinkDirection.North;
			case GameController.GridSwipeDirection.Down: return TileLinkDirection.South;
			default: return TileLinkDirection.MAX;
		}
	}


	public Tile FindTilePerpendicularTo(Tile _tileA, PerpendicularDirection _direction)
	{
		if (_direction == PerpendicularDirection.Horizontal)
			return Game.Instance.Grid.Tiles[(Row * Grid.Columns) + _tileA.Column];

		return Game.Instance.Grid.Tiles[(Column * Grid.Rows) + _tileA.Row];
	}


	static TileLinkDirection SwitchDirection(TileLinkDirection _direction)
	{
		switch (_direction)
		{
			case TileLinkDirection.West: return TileLinkDirection.East;
			case TileLinkDirection.East: return TileLinkDirection.West;
			case TileLinkDirection.North: return TileLinkDirection.South;
			case TileLinkDirection.South: return TileLinkDirection.North;
			default: return TileLinkDirection.MAX;
		}
	}


	Tile FindLinkedTile(int _currentTileID, List<Tile> _tiles, TileLinkDirection _direction)
	{
		Tile tile = null;

		switch (_direction)
		{
			case TileLinkDirection.North:
				{
					// Check that this tile is not the top row
					if (_currentTileID < Grid.Columns)
						return null;

					tile = _tiles[_currentTileID - Grid.Columns];
				}
				break;
			case TileLinkDirection.South:
				{
					// Check that this tile is not the bottom row
					if (_currentTileID >= Grid.Columns * Grid.Rows - Grid.Columns)
						return null;

					tile = _tiles[_currentTileID + Grid.Columns];
				}
				break;
			case TileLinkDirection.East:
				{
					// Check that this tile is not the west column
					if (_currentTileID == 0 ||
						_currentTileID % Grid.Columns == 0)
						return null;

					tile = _tiles[_currentTileID - 1];
				}
				break;
			case TileLinkDirection.West:
				{
					// Check that this tile is not the east column
						if (_currentTileID == Grid.Columns - 1)
							return null;

					// NOTE: Couldn't come up with something nice here.
					for (int i = 1; i < Grid.Rows + 1; ++i)
					{
						if (_currentTileID == Grid.Columns * i - 1)
							return null;
					}

					tile = _tiles[_currentTileID + 1];
				}
				break;
			default:
				break;
		}

		return tile;
	}
};
