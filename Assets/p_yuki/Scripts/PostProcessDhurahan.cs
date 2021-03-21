using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


public class PostProcessDhurahan : MonoBehaviour
{
    private PostProcessVolume postProcessVolume;
    private PostProcessProfile postProcessProfile;
    private Transform m_Player;
    private Transform m_Boss;

    private Grain grain;
    private bool hasGrainEffect;

    // Start is called before the first frame update
    void Start()
    {
        postProcessVolume = this.GetComponent<PostProcessVolume>();


        //　このボリュームのみ適用
        postProcessProfile = postProcessVolume.profile;

        //　エフェクトがあるかどうかの判断と取得を同時に行う
        hasGrainEffect = postProcessProfile.TryGetSettings<Grain>(out grain);

        m_Player = GameObject.FindGameObjectWithTag("Player").transform;
        m_Boss = GameObject.FindGameObjectWithTag("Boss").transform;

        
    }

    // Update is called once per frame
    void Update()
    {

        if (hasGrainEffect) {

            grain.enabled.Override(true);

            float d;
            d = 5 / Vector3.Distance(m_Player.localPosition, m_Boss.localPosition);

            //急にモザイクをかける
            if (d >= 0.3f)
            {
                grain.intensity.Override(d);
            }
            else
            {
                grain.intensity.Override(0);
            }

        }
    }

    void OnDestroy()
    {
        //　作成したプロファイルの削除
        RuntimeUtilities.DestroyProfile(postProcessProfile, true);
    }
}
