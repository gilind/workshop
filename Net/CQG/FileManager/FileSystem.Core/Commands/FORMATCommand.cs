namespace FileEmulation.Core.Commands
{
    /// <summary>
    /// Комманда FORMAT (Format) - очищает (форматирует) раздел, корневая директория устанавливается текущей. FORMAT
    /// </summary>
    internal class FORMATCommand : ICommand
    {
        internal FORMATCommand()
        {}


        /// <summary>
        /// Выполнить команду.
        /// </summary>
        public void Execute()
        {
            FileSystem.Instance.Format();
        }
    }
}