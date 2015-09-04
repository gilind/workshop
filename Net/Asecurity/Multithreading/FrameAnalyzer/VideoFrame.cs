using System;

namespace FrameAnalyzer
{
    /// <summary>
    /// Снимок веб-камеры, базовый класс, не может иметь экземпляров.
    /// </summary>
    public abstract class VideoFrame
    {
        /// <summary>
        /// Обычный снимок.
        /// </summary>
        private class NormalFrame : VideoFrame
        {
        }

        /// <summary>
        /// Снимок, содержащий искомый объект.
        /// </summary>
        private class WantedFrame : VideoFrame
        {
            public WantedFrame() : base(WantedObject.SpecificObject)
            {
            }
        }

        /// <summary>
        /// Снимок, содержащий мусор вместо изображения.
        /// </summary>
        private class BrokenFrame : VideoFrame
        {
        }

        private static int _lastId;

        private DateTime _creationDateTime;

        private readonly WantedObject _wanted;

        protected VideoFrame()
            : this(null)
        {
        }

        protected VideoFrame(WantedObject wanted)
        {
            Id = _lastId++;

            _creationDateTime = DateTime.Now;
            _wanted = wanted;
        }

        /// <summary>
        /// Номер (идентификатор) снимка.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Время создания снимка.
        /// </summary>
        public string Time
        {
            get { return _creationDateTime.ToLongTimeString() + "."+_creationDateTime.Millisecond; }
        }

        public override string ToString()
        {
            return $"#{Id} ({Time})";
        }

        /// <summary>
        /// Содержит ли снимок искомый объект?
        /// </summary>
        public bool Contain(WantedObject wanted)
        {
            return _wanted == wanted;
        }

        private static readonly Random Random = new Random();

        /// <summary>
        /// Фабрика, с разной вероятностью генерирующая снимки разных типов.
        /// </summary>
        public static VideoFrame Generate()
        {
            int number = Random.Next(0, 99);

            // в 90% случаев создается обычный снимок
            if (number < 90)
            {
                return new NormalFrame();
            }

            // в (96-90)% случаев создается битый снимок
            if (number < 96)
            {
                return new BrokenFrame();
            }

            return new WantedFrame();
        }
    }
}