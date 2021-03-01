using UnityEngine;

[CreateAssetMenu(menuName = "DataSettings")]
public class CommonDataSettings : ScriptableObject
{
    public bool difficulty_mid;
    public bool difficulty_hight;
    public readonly int GAME = 1;
    public readonly int MENU = 2;
    public readonly int SORRY = 3;

    public bool toGame = false;
    public bool toMenu = false;
    public bool sorry = false;
}
