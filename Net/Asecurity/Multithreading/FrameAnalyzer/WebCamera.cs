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
            try
            {
                while (true)
                {
                    // если идет чтение снимков, остановить генерацию снимков
                    //if (_paused)
                    //{
                    //    continue;
                    //}

                    if (token.IsCancellationRequested)
                    {
                        return;
                    }

                    // проверяем, не запрошена ли отмена операции
                    //token.ThrowIfCancellationRequested();

                    lock (_frames)
                    {
                        _frames.Add(VideoFrame.Generate());
                    }
                    Thread.Sleep(5);
                }
            }
            finally
            {
            }
        }

        //private bool _paused;

        /// <summary>
        /// Выключение камеры.
        /// </summary>
        public void Stop()
        {
            //try
            //{
                // запрашиваем отмену операции
                _cancellation.Cancel();
            //}
            //finally { }
        }

        /// <summary>
        /// Передает очередную порцию снимков.
        /// </summary>
        public IList<VideoFrame> GetFrames()
        {
            // поставить генерацию на паузу
            //_paused = true;
            lock (_frames)
            {

                List<VideoFrame> result = new List<VideoFrame>();

                result.AddRange(_frames);
                _frames.Clear();

                // продолжить генерацию
                //_paused = false;
                return result;
            }
        }
    }
}