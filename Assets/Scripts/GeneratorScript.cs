using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// room generation tutorial https://www.raywenderlich.com/5459-how-to-make-a-game-like-jetpack-joyride-in-unity-2d-part-2#toc-anchor-009 

public class GeneratorScript : MonoBehaviour
{
    public GameObject[] availableRooms;
    public List<GameObject> currentRooms;
    private float screenWidthInPoints;

    
    void Start()
    {
        // calculate size of the screen
        float height = 2.0f * Camera.main.orthographicSize;
        screenWidthInPoints = height * Camera.main.aspect;

        StartCoroutine(GeneratorCheck());

    }

    void AddRoom(float farthestRoomEndX)
    {
        //picks random index of the room prefab
        int randomRoomIndex = Random.Range(0, availableRooms.Length);
        //creates a room object using the index chosen
        GameObject room = (GameObject)Instantiate(availableRooms[randomRoomIndex]);
        //get the size of the floor which = width
        float roomWidth = room.transform.Find("floor").localScale.x;
        //calculates where center should be
        float roomCenter = farthestRoomEndX + roomWidth * 0.5f;
        //sets position of the room
        room.transform.position = new Vector3(roomCenter, 0, 0);
        //adds room to the list of current rooms
        currentRooms.Add(room);
    }

    private void GenerateRoomIfRequired()
    {
        //creates list of rooms that need to be removed
        List<GameObject> roomsToRemove = new List<GameObject>();
        //flag if more rooms need to be added
        bool addRooms = true;
        //saves player position
        float playerX = transform.position.x;
        //point after which room should be removed
        float removeRoomX = playerX - screenWidthInPoints;
        //if no room exists after this point then room needs to be added
        float addRoomX = playerX + screenWidthInPoints;
        //stores the point where level currently ends
        float farthestRoomEndX = 0;
        foreach (var room in currentRooms)
        {
            //enumerates currentRooms
            float roomWidth = room.transform.Find("floor").localScale.x;
            float roomStartX = room.transform.position.x - (roomWidth * 0.5f);
            float roomEndX = roomStartX + roomWidth;
            //if there is a room after addRoomX point then room doesn't need to be added
            if (roomStartX > addRoomX)
            {
                addRooms = false;
            }
            //if the room ends to the left of the removeRoomX point then it needs to be removed
            if (roomEndX < removeRoomX)
            {
                roomsToRemove.Add(room);
            }
            //finds rightmost point of the level
            farthestRoomEndX = Mathf.Max(farthestRoomEndX, roomEndX);
        }
        //removes rooms marked for removal
        foreach (var room in roomsToRemove)
        {
            currentRooms.Remove(room);
            Destroy(room);
        }
        //new room needs to be added
        if (addRooms)
        {
            AddRoom(farthestRoomEndX);
        }
    }

    // periodically executes room generation script
    private IEnumerator GeneratorCheck()
    {
        while (true)
        {
            GenerateRoomIfRequired();
            yield return new WaitForSeconds(0.25f);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
