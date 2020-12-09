using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : Boss
{
    public static int COLUMN = 23;
    public static int ROW = 13;
    public GameObject player;
    public Vector3 pos;
    private int[,] terrainInfo = new int[COLUMN, ROW];
    private bool isGrasp = false;
    private bool isRunning = false;

    void Start()
    {
    }

    void Update()
    {
        StartCoroutine(MoveToDestination(player.transform.position));
        // if(!isGrasp){
        //     Terrain_grasp();
        // }else{

        // }
    }
    //目的地（destination）に障害物などを避けながら移動する 
    public override IEnumerator MoveToDestination(Vector3 destination)
    {
        if(isRunning){
            yield break;
        }
        isRunning = true;

        //自分の現在地から目的地までの方向
        Vector3 direction = (destination - this.transform.localPosition);
        direction.y = 0;

        this.transform.localPosition += Time.deltaTime * MoveSpeed * direction.normalized;

        yield return null;

        isRunning = false;
    }

    public void Terrain_grasp(){
        for(int j = 0; j < ROW; j++)
        {
            for(int i = 0; i < COLUMN; i++)
            {
                Vector3 origin = new Vector3(-22f + 2 * i, 20f, -12f + 2 * j);
                Ray terRay = new Ray(origin, Vector3.down);
                RaycastHit hit;
                if (Physics.Raycast(terRay, out hit, 25f))
                {
                    if (hit.collider.gameObject.layer == 8)
                        terrainInfo[i, j] = 1;
                    else
                        terrainInfo[i, j] = 0;
                }
            }
        }

        int plcol = ((int)player.transform.position.x + 23) / 2;
        int plrow = ((int)player.transform.position.z + 13) / 2;
        terrainInfo[plcol, plrow] = 2;

        // コンソール表示用(運用する際は消す)
        string text = "";
        for(int j = 0; j < ROW; j++){
            for(int i = 0; i < COLUMN; i++){
                text += terrainInfo[i, ROW-j-1].ToString() + " ";
            }
            text += "\n";
        }
        Debug.Log(text);
    }
}
