using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActSpriteMotion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SetSpriteImgSizeOrigin();

        Debug.LogError("ActSpriteMotion Name : " + this.gameObject.name);

        UI2DSprite _bg = GetComponent<UI2DSprite>();
        // _bg.MakePixelPerfect();
        _bg.width = 1024;
        _bg.height = 1600;
    }

    //// 프리팹에 들어있는 스프라이트들 찾아서 원사이즈로 수정
    void SetSpriteImgSizeOrigin()
    {
        Transform _tran = this.gameObject.transform;

        for (int i = 0; i < _tran.childCount; i++)
        {
            if (_tran.GetChild(i).GetComponent<UI2DSprite>() != null)
            {
                if (_tran.GetChild(i).GetComponent<UI2DSprite>().sprite2D != null)
                {
                    _tran.GetChild(i).GetComponent<UI2DSprite>().MakePixelPerfect();
                }
            }
        }
    }


}
