using System.Collections.Generic;
using UnityEngine.Audio;

namespace Microlight.MicroAudio
{
    // ****************************************************************************************************
    // Manager for playing MicroInfitnity sound effects
    // ****************************************************************************************************
    public class MicroInfinitySounds
    {
        readonly List<MicroInfinityInstance> instanceList;

        internal MicroInfinitySounds()
        {
            instanceList = new List<MicroInfinityInstance>();

            MicroAudio.UpdateEvent += Update;
        }

        ~MicroInfinitySounds()
        {
            MicroAudio.UpdateEvent -= Update;
        }

        void Update()
        {
            foreach (MicroInfinityInstance instance in instanceList)
            {
                instance.Update();
            }
        }

        internal MicroInfinityInstance PlayInfinitySound(MicroInfinitySoundGroup infinityGroup, AudioMixerGroup mixerGroup)
        {
            MicroInfinityInstance newInstance = new MicroInfinityInstance(infinityGroup, mixerGroup);
            instanceList.Add(newInstance);
            newInstance.OnEnd += FinishGroup;
            return newInstance;
        }

        void FinishGroup(MicroInfinityInstance instance)
        {
            instanceList.Remove(instance);
        }
    }
}