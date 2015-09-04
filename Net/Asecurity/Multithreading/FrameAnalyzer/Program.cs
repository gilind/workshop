using System;
using System.Collections.Generic;
using System.Threading;

namespace FrameAnalyzer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            const int t = 100; // мс, время цикла обработки снимков камеры
            const int T = 1000; // мс, общее время работы программы

            WebCamera camera = new WebCamera();
            camera.Start();

            DateTime startTime = DateTime.Now;
            int totalTime = 0;
            //int elapsedTime = 0;

            while (totalTime < T)
            {
                Thread.Sleep(t);

                IList<VideoFrame> frames = camera.GetFrames();

                totalTime = DateTime.Now.Subtract(startTime).Milliseconds;
            }
        }
    }
}