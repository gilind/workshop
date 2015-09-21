namespace FileEmulation.Core.Commands
{
    /// <summary>
    /// �������� FORMAT (Format) - ������� (�����������) ������, �������� ���������� ��������������� �������. FORMAT
    /// </summary>
    internal class FORMATCommand : ICommand
    {
        internal FORMATCommand()
        {}


        /// <summary>
        /// ��������� �������.
        /// </summary>
        public void Execute()
        {
            FileSystem.Instance.Format();
        }
    }
}