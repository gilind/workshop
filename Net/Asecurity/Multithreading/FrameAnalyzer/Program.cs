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

            IList<VideoFrame> frames = null;

            while (totalTime < T)
            {
                Thread.Sleep(t); // todo: в реальном приложении так делать нельзя, например нужно обслуживать UI

                frames = camera.GetFrames();

                totalTime = (int)DateTime.Now.Subtract(startTime).TotalMilliseconds;
                //DateTime.Now.Subtract(startTime).Milliseconds;
            }

            camera.Stop();

            IList < VideoFrame > wantedFrames = new List<VideoFrame>();

            if (frames != null)
            {
                Console.WriteLine("Total Frames: {0}", frames.Count);
                Console.WriteLine("Wanted Frames:");

                foreach (VideoFrame frame in frames)
                {
                    if (frame.Contain(WantedObject.SpecificObject))
                    {
                        wantedFrames.Add(frame);
                        Console.WriteLine(frame);
                    }
                }
            }

            Console.ReadLine();
        }
    }
}