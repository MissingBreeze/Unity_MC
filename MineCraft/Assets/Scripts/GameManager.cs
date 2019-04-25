using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static int rangomSeed;

    private void Awake()
    {
        System.TimeSpan timespan = System.DateTime.UtcNow - new System.DateTime(1970, 2, 1, 0, 0, 0, 0);
        rangomSeed = (int)timespan.TotalSeconds;
    }
}
