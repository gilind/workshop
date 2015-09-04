namespace Monitoring
{
    /// <summary>
    /// Объект, который необходимо найти в снимке.
    /// В задании он называется Image.
    /// </summary>
    public class WantedObject
    {
        /// <summary>
        /// Конструктор приватный, значит создать экземпляр можно только через SpecificObject.
        /// </summary>
        private WantedObject()
        {
        }

        private static WantedObject _specificObject;

        /// <summary>
        /// Простейшая реализация паттерна Singleton, обеспечивает создание не более одного экзепляра.
        /// </summary>
        public static WantedObject SpecificObject
        {
            get { return _specificObject ?? (_specificObject = new WantedObject()); }
        }
    }
}