namespace FileEmulation.Core.Commands
{
    /// <summary>
    /// �������� DIR (Directory) - ������� �� ����� ���������� ������� � ���� ������, ������� � �������� ����������. DIR
    /// </summary>
    internal class DIRCommand : ICommand
    {
        internal DIRCommand()
        {}


        /// <summary>
        /// ��������� �������.
        /// </summary>
        public void Execute()
        {
            FileSystem.Instance.Print();
        }
    }
}