using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TileProperties : MonoBehaviour
{
    public GameObject Door;
    public GameVariables.TileStates state;

    ArrayList directions = new ArrayList();
    ArrayList doors = new ArrayList();


    void Awake()
    {

        while (directions.Count < 1)
            createDoors();
                

        foreach (GameVariables.Direction direction in directions) {
            if (direction == GameVariables.Direction.North)
                doors.Add(Instantiate(Door, transform.position + new Vector3(0, .01f, .5f), Quaternion.Euler(90, 0, 180), transform));
            if (direction == GameVariables.Direction.South)
                doors.Add(Instantiate(Door, transform.position + new Vector3(0, .01f, -.5f), Quaternion.Euler(90, 0, 0), transform));
            if (direction == GameVariables.Direction.East)
                doors.Add(Instantiate(Door, transform.position + new Vector3(.5f, .01f, 0), Quaternion.Euler(90, 0, 90), transform));
            if (direction == GameVariables.Direction.West)
                doors.Add(Instantiate(Door, transform.position + new Vector3(-0.5f, .01f, 0), Quaternion.Euler(90, 0, -90), transform));
        }

        state = GameVariables.TileStates.InCanvas;
    }

    void createDoors(){
        if (Random.value > 0.5f)
            directions.Add(GameVariables.Direction.North);
        if (Random.value > 0.5f)
            directions.Add(GameVariables.Direction.East);
        if (Random.value > 0.5f)
            directions.Add(GameVariables.Direction.West);
        if (Random.value > 0.5f && directions.Count < 3)
            directions.Add(GameVariables.Direction.South);
    }

    private void Update()
    {
        if (state == GameVariables.TileStates.Positioning){
            if (ValidatePosition()){
                gameObject.GetComponent<Renderer>().material.color = new Color(0, .5f, 0, 1);
            } else {
                gameObject.GetComponent<Renderer>().material.color = new Color(.5f, 0, 0, 1);
            }
        } else {
            gameObject.GetComponent<Renderer>().material.color = new Color(.36f, .23f, 0, 1);
        }
    }

    public bool ValidatePosition(){
        bool isValid = true;
        foreach(GameObject door in doors){
            isValid = isValid && !door.GetComponent<DoorController>().colliding;
        }
        return isValid;
    }
}
