using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEngine;

public class Masked_Updater : MonoBehaviour
{
    [SerializeField]
    private SpriteMask spriteMask;
    [SerializeField]
    private SpriteRenderer player;
    [SerializeField]
    private Sprite[] maskedIdle, maskedWalk;
    

    // Update is called once per frame
    void LateUpdate()
    {
        if(spriteMask == null || player == null) return;

        var cycle = Regex.Replace(player.sprite.name, "([A-z_]+0{0,2})", "");
        var cycleMask = Regex.Replace(spriteMask.sprite.name, "([A-z_]+0{0,2})", "");

        if(!cycle.Equals(cycleMask)){

            if(player.sprite.name.Contains("Walk")){
                var walkCycle = int.Parse(cycle);
                spriteMask.sprite = maskedWalk[walkCycle];
            } else {
                var idleCycle = int.Parse(cycle);
                spriteMask.sprite = maskedIdle[idleCycle];
            }

        }

    }
}
