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
            _frames.Clear();
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
                // если идет чтение снимков, остановить генерацию снимков
                if (_paused)
                {
                    continue;
                }

                // проверяем, не запрошена ли отмена операции
                token.ThrowIfCancellationRequested();

                _frames.Add(VideoFrame.Generate());
                Thread.Sleep(10);
            }
        }

        private bool _paused;

        /// <summary>
        /// Выключение камеры.
        /// </summary>
        public void End()
        {
            // запрашиваем отмену операции
            _cancellation.Cancel();
        }

        /// <summary>
        /// Передает очередную порцию снимков.
        /// </summary>
        public IList<VideoFrame> GetFrames()
        {
            // поставить генерацию на паузу
            _paused = true;

            List<VideoFrame> result = new List<VideoFrame>();

            result.AddRange(_frames);
            _frames.Clear();

            // продолжить генерацию
            _paused = false;
            return result;
        }
    }
}