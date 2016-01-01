﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AudioEndPointControllerWrapper;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using SoundSwitch.Framework.Audio;
using SoundSwitch.Properties;

namespace SoundSwitch.Framework.NotificationManager.Notification
{
    public class NotificationSound : INotification
    {
        private readonly MMDeviceEnumerator _deviceEnumerator;

        public NotificationSound()
        {
            _deviceEnumerator = new MMDeviceEnumerator();
        }

        public void NotifyDefaultChanged(IAudioDevice audioDevice)
        {
            if (audioDevice.Type != AudioDeviceType.Playback)
                return;
            var task = new Task(() =>
            {
                var device = _deviceEnumerator.GetDevice(audioDevice.Id);
                using (var output = new WasapiOut(device, AudioClientShareMode.Shared, true, 10))
                {
                    output.Init(new WaveFileReader(Resources.NotificationSound));
                    output.Play();
                    while (output.PlaybackState == PlaybackState.Playing)
                    {
                        Thread.Sleep(500);
                    }
                }
            });
            task.Start();
        }
    }
}