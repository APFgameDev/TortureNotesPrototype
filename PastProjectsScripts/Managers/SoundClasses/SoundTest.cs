using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NS_Sound;

public class SoundTest : MonoBehaviour {

    bool m_testBool = false;

	// Use this for initialization
	void Start ()
    {
        SoundController.AttachAndPlaySound(SoundType.Music, this.transform, 0);
        SoundController.AttachAndPlaySound(SoundType.Music, this.transform, 1);
        SoundController.AttachAndPlaySound(SoundType.Music, this.transform, 2);
    }
	
	// Update is called once per frame
	void Update () {
		if (!m_testBool)
        {
            m_testBool = true;

            SoundObject o1 = SoundController.FindLiveSoundObject(SoundType.Music, "Music1");
            if (o1 == null)
                Debug.LogError("SoundTest: Sound 1 not found");

            SoundObject o2 = SoundController.FindLiveSoundObject(SoundType.Music, "Music2");
            if (o2 == null)
                Debug.LogError("SoundTest: Sound 2 not found");

            SoundObject o3 = SoundController.FindLiveSoundObject(SoundType.Music, "Music3");
            if (o3 == null)
                Debug.LogError("SoundTest: Sound 3 not found");
        }
	}
}
