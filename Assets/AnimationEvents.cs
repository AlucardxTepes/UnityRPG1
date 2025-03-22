using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    private Player player;
    
    void Start()
    {
        player = GetComponentInParent<Player>();
    }

    private void AnimationTrigger()
    {
        player.SetAttackOver();
    }

}
