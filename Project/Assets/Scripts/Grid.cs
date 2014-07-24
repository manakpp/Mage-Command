//  
//  File Name   :   Grid.cs
//


// Namespaces
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Grid : MonoBehaviour
{

	// Member Types


	// Member Delegates & Events


	// Member Properties

	public static int Columns
	{
		get { return s_columns; }
	}


	public static int Rows
	{
		get { return s_rows; }
	}


	public List<Tile> Tiles
	{
		get { return m_tiles; }
	}


	// Member Fields

	public GameObject m_tilePrefabA;
	public GameObject m_tilePrefabB;
	public Vector3 m_gridTopLeftPosition = new Vector3(-4.0f, 0.0f, 5.0f);

	public int m_columns = 11;
	public int m_row = 5;

	List<Tile> m_tiles;

	static int s_columns = 11;
	static int s_rows = 5;


	// Member Methods


	void Awake()
	{
		s_columns = m_columns;
		s_rows = m_row;

		if (m_tilePrefabA == null || m_tilePrefabB == null)
		{
			Debug.Log("Need a tile A and B prefab set. Whole game is going to break.");
			return;
		}

		var tileA = m_tilePrefabA.GetComponent<Tile>();
		var tileB = m_tilePrefabB.GetComponent<Tile>();

		if (tileA == null || tileB == null)
		{
			Debug.Log("Need a tile A and B prefab set. Whole game is going to break.");
			return;
		}

		float gridSize = tileA.transform.localScale.x * 2.0f;

		int columns = Columns;
		int rows = Rows;

		Vector3 offset = Vector3.zero;

		m_tiles = new List<Tile>(columns * rows);

		GameObject newTile = null;

		for (int i = 0; i < rows; ++i)
		{
			offset.z = i * -gridSize;

			for (int j = 0; j < columns; ++j)
			{
				offset.x = j * gridSize;

				if ((i * rows + j) % 2 == 0)
					newTile = GameObject.Instantiate(m_tilePrefabA, m_gridTopLeftPosition + offset, Quaternion.identity) as GameObject;
				else
					newTile = GameObject.Instantiate(m_tilePrefabB, m_gridTopLeftPosition + offset, Quaternion.identity) as GameObject;

				newTile.transform.parent = transform;
				newTile.name = string.Format("Tile: {0}x {1}y (ID: {2})", j, i, i * columns + j);

				var tile = newTile.GetComponent<Tile>();
				tile.Row = i;
				tile.Column = j;

				m_tiles.Add(newTile.GetComponent<Tile>());

			}
		}

		// Make links
		for (int i = 0; i < m_tiles.Count; ++i)
		{
			m_tiles[i].MakeLinks(i, m_tiles);
		}
	}




};
