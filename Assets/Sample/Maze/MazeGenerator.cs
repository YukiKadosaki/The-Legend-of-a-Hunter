using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private int m_array_size;
    [SerializeField] private float m_space;
    [SerializeField] private GameObject m_wall_prefab;
    private GameObject[,] m_wall_objects;
    private bool[,] m_maze_table;

    // Start is called before the first frame update
    void Start()
    {
        m_maze_table = new bool[m_array_size, m_array_size];
        m_wall_objects = new GameObject[m_array_size, m_array_size];
        MakeMaze();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DeleteWalls();
            MakeMaze();
        }
    }

    private void MakeMaze()
    {
        MakeMazeTable();

        for (int i = 0; i < m_array_size; i++)
        {
            for (int j = 0; j < m_array_size; j++)
            {
                if (m_maze_table[i, j])
                {
                    m_wall_objects[i, j] = Instantiate(m_wall_prefab);
                    m_wall_objects[i, j].transform.position = new Vector3(j * m_space, 0, i * m_space);
                }
                else
                {
                    m_wall_objects[i, j] = null;
                }
            }
        }
    }

    private void MakeMazeTable()
    {
        InitMazeTable();
        MakeWallTable();
        MakeStick();
        StickDownAll();
    }

    private void InitMazeTable()
    {
        for (int i = 0; i < m_array_size; i++)
        {
            for (int j = 0; j < m_array_size; j++)
            {
                m_maze_table[i, j] = false;
            }
        }
    }

    private void MakeWallTable()
    {

        for (int i = 0; i < m_array_size; i++)
        {
            m_maze_table[0, i] = true;
        }



        for (int i = 0; i < m_array_size; i++)
        {
            for (int j = 0; j < m_array_size; j += (m_array_size - 1))
            {
                m_maze_table[i, j] = true;
            }
        }



        for (int i = 0; i < m_array_size; i++)
        {
            m_maze_table[m_array_size - 1, i] = true;
        }

    }

    private void MakeStick()
    {
        for (int i = 2; i < m_array_size; i += 2)
        {
            for (int j = 2; j < m_array_size; j += 2)
            {
                m_maze_table[i, j] = true;
            }
        }
    }

    private void StickDownAll()
    {
        //周りを壁で囲んでいるのでループの範囲は-1
        for (int i = 2; i < m_array_size - 1; i += 2)
        {
            for (int j = 2; j < m_array_size - 1; j += 2)
            {
                int direction;

                //一番上の段だけ上にも倒せる
                if (i == 2)
                {
                    //一番上の段だけ上にも倒せる
                    direction = Random.Range(1, 4);
                    StickDown(direction, i, j);
                }
                else
                {
                    direction = Random.Range(1, 3);
                    StickDown(direction, i, j);
                }

            }
        }
    }

    private void StickDown(int dir, int i, int j)
    {
        if (dir == 1)
        {
            m_maze_table[i, ++j] = true;
        }
        else if (dir == 2)
        {
            m_maze_table[i + 1, j] = true;
        }
        else if (dir == 3)
        {
            m_maze_table[i, --j] = true;
        }
        else if (dir == 4)
        {
            m_maze_table[i - 1, j] = true;
        }
    }

    private void DeleteWalls()
    {
        for(int i = 0;i < m_array_size; i++)
        {
            for(int j = 0;j < m_array_size ; j++)
            {
                Destroy(m_wall_objects[i,j]);
            }
        }
    }
}
