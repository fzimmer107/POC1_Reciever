using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RecieverTest : MonoBehaviour
{

    private PhotonView RPCPhotonView;
    private Dictionary<int, MeshFilter> recievedMeshes = new Dictionary<int, MeshFilter>();
    //private List<int> RecievedMeshesID = new List<int>();
    private Dictionary<int, int> updateCount = new Dictionary<int, int>();
    public GameObject meshPrefab;
    public GameObject cameraRepPrefab;

    // Start is called before the first frame update
    void Start()
    {
        RPCPhotonView = GetComponent<PhotonView>();
        
    }

    // Update is called once per frame
    void Update()
    {

    }


    [PunRPC]
    public void RPCTest(Vector3 position, int meshId)
    {
        Debug.Log($"MESH FROM MESHOBSERVER WITH ID: {meshId}: POSITION:({position.x},  y{position.y}, z{position.z})");
    }

    [PunRPC]
    public void RecieveMeshData(Vector3[] vertices, int[] triangles, Vector2[] uvs, Vector3 position, Vector3 rotation, int meshId, Vector3 camPos, Vector3 camRot)
    {

        BuildCameraRep(camPos, camRot,meshId);
        BuildMesh(vertices, triangles, uvs, position, rotation, meshId);
        
    }

    private void UpdataMeshData(Vector3[] vertices, int[] triangles, Vector2[] uvs, Vector3 position, Vector3 rotation, int meshId)
    {
        recievedMeshes[meshId].mesh.Clear();
        recievedMeshes[meshId].mesh.vertices = vertices;
        recievedMeshes[meshId].mesh.triangles = triangles;
        Debug.Log($"Updated Mesh with ID: {meshId}");
        
    }

    private void BuildCameraRep(Vector3 pos,Vector3 rot, int meshId)
    {
        //instantiate prefab on recieved position, to see if camera moves in scene
        GameObject prefab = Instantiate(cameraRepPrefab, pos, Quaternion.Euler(rot.x, rot.y, rot.z));
        prefab.name = $"{meshId}:camera";
    }

    private void BuildMesh(Vector3[] vertices, int[] triangles, Vector2[] uvs, Vector3 position, Vector3 rotation, int meshId)
    {
        GameObject prefab = Instantiate(meshPrefab, position, Quaternion.Euler(rotation.x, rotation.y, rotation.z));
        prefab.name = meshId.ToString();
        Debug.Log($"Creating Mesh with ID: {meshId}, at POSTITION: ({position.x},{position.y}, {position.z} and ROTATION ({rotation.x},{rotation.y}, {rotation.z})");


        Mesh recievedMesh = new Mesh();
        recievedMesh.vertices = vertices;
            recievedMesh.triangles = triangles;
            recievedMesh.uv = uvs;
            MeshFilter meshFilter = prefab.GetComponent<MeshFilter>();
            meshFilter.mesh = recievedMesh;      
    }

    [PunRPC]
    public void SendError(Vector3 position, int meshId)
    {
       Debug.Log($"MESH ID FROM EVENTDATA: {meshId} at position:({position.x},  y{position.y}, z{position.z})");
    }



    [PunRPC]
    public void RemoveMesh(int meshId)
    {
       

    }
}
