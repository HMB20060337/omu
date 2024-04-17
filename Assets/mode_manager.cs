using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mode_manager : MonoBehaviour
{
    public ModeDatabase modeDatabase;
    public Image artworkSprite;
    public Image artworkSpriteLeft;
    public Image artworkSpriteRight;


    private int selectedOpt = 0;
    void Start()
    {
        updateMode(selectedOpt);
    }

    public void nextOption()
    {
        selectedOpt++;
        if(selectedOpt >= modeDatabase.ModeCount) {
            selectedOpt = 0;
        }
        updateMode(selectedOpt);
    }

    public void backOption()
    {
        selectedOpt--;
        if(selectedOpt < 0) {
        selectedOpt = 2 ;}
        updateMode(selectedOpt);
    }

    private void updateMode(int selectedOpt)
    {

        int pre = 0;
        int next = 0;

        if(selectedOpt == 0)
        {
            pre = 2;
            next = 1;
        }
        if(selectedOpt == 1)
        {
            pre = 0;
            next = 2;
        }
        if(selectedOpt == 2)
        {
            pre = 1;
            next = 0;
        }

        Mode mode = modeDatabase.getMode(selectedOpt);
        Mode mode1 = modeDatabase.getMode(pre);
        Mode mode2 = modeDatabase.getMode(next);
        artworkSprite.sprite = mode.modeSprite;
        artworkSpriteLeft.sprite = mode1.modeSprite;
        artworkSpriteRight.sprite = mode2.modeSprite;
    }

  
}
