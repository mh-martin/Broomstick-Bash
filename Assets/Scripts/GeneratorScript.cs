using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// room generation tutorial https://www.raywenderlich.com/5459-how-to-make-a-game-like-jetpack-joyride-in-unity-2d-part-2#toc-anchor-009 

public class GeneratorScript : MonoBehaviour
{
  public GameObject[] availableRooms;
  public List<GameObject> currentRooms;
  private float screenWidthInPoints;

  public GameObject[] availableObjects;
  public List<GameObject> objects;

  public float objectsMinDistance = 5.0f;
  public float objectsMaxDistance = 10.0f;

  public float objectsMinY = -1.4f;
  public float objectsMaxY = 1.4f;

  public float objectsMinRotation = -45.0f;
  public float objectsMaxRotation = 45.0f;

  private string lastSpawnedTag = "";
  private int sameTagSpawnCount = 0;
  public int maxSameSpawned = 3;

  void Start()
  {
    // calculate size of the screen
    float height = 2.0f * Camera.main.orthographicSize;
    screenWidthInPoints = height * Camera.main.aspect;

    StartCoroutine(GeneratorCheck());

  }

  void AddRoom(float farthestRoomEndX)
  {
    GameObject lastRoom = currentRooms[currentRooms.Count - 1];
    GameObject[] filteredRooms = availableRooms.Where(room => !lastRoom.CompareTag(room.tag)).ToArray();
    //picks random index of the room prefab
    int randomRoomIndex = Random.Range(0, filteredRooms.Length);
    //creates a room object using the index chosen
    GameObject room = Instantiate(filteredRooms[randomRoomIndex]);
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
    //if no room exists after this point then room needs to be added
    float addRoomX = playerX + screenWidthInPoints;
    //stores the point where level currently ends
    float farthestRoomEndX = 0;
    //point after which room should be removed
    float removeRoomX = playerX - screenWidthInPoints;
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

  void AddObject(float lastObjectX)
  {
    List<GameObject> filteredObjects = new List<GameObject>(availableObjects);
    // Compares the amount of object types spawned so far to the maximum of same type allowed
    if (sameTagSpawnCount == maxSameSpawned)
    {
      filteredObjects = filteredObjects.Where(go => !go.CompareTag(lastSpawnedTag)).ToList();
    }

    // generates random index to select a random object from array
    int randomIndex = Random.Range(0, filteredObjects.Count);
    // If the next object is an enemy, compares it to the previous object and rerolls if they're the same
    GameObject prefabObject = filteredObjects[randomIndex];
    if (objects.Count > 0 && prefabObject.CompareTag("Enemy") && prefabObject.name == objects.Last().name)
    {
      filteredObjects.RemoveAt(randomIndex);
      randomIndex = Random.Range(0, filteredObjects.Count);
    }
    // creates instance of the randomly selected object
    GameObject obj = Instantiate(filteredObjects[randomIndex]);
    obj.name = prefabObject.name;
    // sets the position
    float objectPositionX = lastObjectX + Random.Range(objectsMinDistance, objectsMaxDistance);
    float randomY = Random.Range(objectsMinY, objectsMaxY);
    obj.transform.position = new Vector3(objectPositionX, randomY, 0);
    // adds the created object to list for tracking and removal
    objects.Add(obj);
    if (!string.IsNullOrEmpty(lastSpawnedTag) && !obj.CompareTag(lastSpawnedTag))
    {
      sameTagSpawnCount = 0;
    }
    sameTagSpawnCount++;
    lastSpawnedTag = obj.tag;
  }

  void GenerateObjectsIfRequired()
  {
    // calculates key points ahead and behind the player
    float playerX = transform.position.x;
    float removeObjectsX = playerX - screenWidthInPoints;
    float addObjectX = playerX + screenWidthInPoints;
    float farthestObjectX = 0;
    // places objects that need to be removed to a list
    List<GameObject> objectsToRemove = new List<GameObject>();
    foreach (var obj in objects)
    {
      //position of the object
      float objX = obj.transform.position.x;
      //maximum objX value
      farthestObjectX = Mathf.Max(farthestObjectX, objX);
      //object that is behind is marked for removal
      if (objX < removeObjectsX)
      {
        objectsToRemove.Add(obj);
      }
    }
    //removes objects
    foreach (var obj in objectsToRemove)
    {
      objects.Remove(obj);
      Destroy(obj);
    }
    //if player is about to see the last object and there are no more objects ahead, calls a method to add a new one
    if (farthestObjectX < addObjectX)
    {
      AddObject(farthestObjectX);
    }
  }


  // periodically executes room generation script
  private IEnumerator GeneratorCheck()
  {
    while (true)
    {
      GenerateRoomIfRequired();
      GenerateObjectsIfRequired();
      yield return new WaitForSeconds(0.25f);
    }
  }


  // Update is called once per frame
  void Update()
  {

  }
}
