using UnityEngine;

public class ItemID : MonoBehaviour
{
    public string idAudioClip;
    public static ItemID instance;
   
    public string SetItemID(string id)
    {
        idAudioClip = id;
        return id ;
    }

}
