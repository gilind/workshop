using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FrameAnalyzer
{
    /// <summary>
    /// Эмуляция Web-камеры.
    /// </summary>
    public class WebCamera
    {
        public WebCamera()
        {
            _frames = new List<VideoFrame>();
        }

        private CancellationTokenSource _cancellation;

        /// <summary>
        /// Включение камеры.
        /// </summary>
        public void Start()
        {
            // очистить список снимков
            lock (_frames)
            {
                _frames.Clear();
            }

            _cancellation = new CancellationTokenSource();

            // запускаем асинхронную операцию
            Task.Run(() => GenerateFrames(_cancellation.Token), _cancellation.Token);
        }

        private readonly IList<VideoFrame> _frames;

        /// <summary>
        /// Метод генерации снимков, запускаемый асинхронно.
        /// </summary>
        private void GenerateFrames(CancellationToken token)
        {
            while (true)
            {
                // проверяем, не запрошена ли отмена операции
                if (token.IsCancellationRequested)
                {
                    return;
                }

                lock (_frames)
                {
                    _frames.Add(VideoFrame.Generate());
                }

                // уменьшить скорость генерации снимков
                Thread.Sleep(1);
            }
        }

        /// <summary>
        /// Выключение камеры.
        /// </summary>
        public void Stop()
        {
            _cancellation.Cancel();
        }

        /// <summary>
        /// Передает очередную порцию снимков.
        /// </summary>
        public IList<VideoFrame> GetFrames()
        {
            List<VideoFrame> result = new List<VideoFrame>();

            lock (_frames)
            {
                result.AddRange(_frames);
                _frames.Clear();
            }

            return result;
        }
    }
}