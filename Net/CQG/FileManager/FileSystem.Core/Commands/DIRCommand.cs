namespace FileEmulation.Core.Commands
{
    /// <summary>
    /// Комманда DIR (Directory) - выводит на экран содержимое раздела в виде дерева, начиная с корневой директории. DIR
    /// </summary>
    internal class DIRCommand : ICommand
    {
        internal DIRCommand()
        {}


        /// <summary>
        /// Выполнить команду.
        /// </summary>
        public void Execute()
        {
            FileSystem.Instance.Print();
        }
    }
}