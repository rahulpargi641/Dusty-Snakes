using UnityEngine;


[CreateAssetMenu(fileName = "NewLevel", menuName = "ScriptableObjects/Level")]
public class LevelSO : ScriptableObject
{
    public LevelView levelView;
    public int levelWidth;
    public int levelHeight;
    public int scoreToWin;
    public int transitionDuration;
    public int textDisplayDuration;
}
