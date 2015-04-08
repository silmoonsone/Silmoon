using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Silmoon.Media.Controls
{
    [
   ComVisible(true),
   Guid("88884578-81ea-4850-9911-13ba2d71000d"),
   ]
    public class AudioControl
    {

        Microsoft.DirectX.AudioVideoPlayback.Audio a;
        string _filePath;

        public Microsoft.DirectX.AudioVideoPlayback.Audio A
        {
            get { return a; }
            set { a = value; }
        }
        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; }
        }

        public AudioControl()
        {

        }

        private void InitAudio()
        {
            if (_filePath == null)
            {
                throw new ArgumentException("无效的属性 FilePath");
            }
            else
            {
                a = new Microsoft.DirectX.AudioVideoPlayback.Audio(_filePath);
            }
        }

        public void Play()
        {
            if (a == null) { InitAudio(); }
            a.Play();
        }
    }
}