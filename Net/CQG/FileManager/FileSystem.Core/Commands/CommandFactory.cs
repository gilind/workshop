using FileSystem.Core.Commands.ConcreteClasses;
using FileSystem.Core.Exceptions;

namespace FileSystem.Core.Commands
{
#if UNIT
    public
#else
    internal
#endif
        enum CommandTypes
    {
        NOP,
        MD,
        CD,
        RD,
        DELTREE,
        MF,
        MHL,
        MDL,
        DEL,
        COPY,
        MOVE,
        DIR
    }


#if UNIT
    public
#else
    internal
#endif
        class CommandFactory
    {
        private CommandFactory()
        {
        }


        public static ICommand CreateCommand(FileManager manager, string commandLine)
        {
            manager.Parser.Parse(commandLine);

            switch (manager.Parser.CommandType)
            {
                case CommandTypes.NOP:
                    return new NOPCommand(manager.Parser.Operands);
                case CommandTypes.MD:
                    return new MDCommand(manager, manager.Parser.Operands);
                case CommandTypes.CD:
                    return new CDCommand(manager, manager.Parser.Operands);
                case CommandTypes.RD:
                    return new RDCommand(manager, manager.Parser.Operands);
                case CommandTypes.DELTREE:
                    return new DELTREECommand(manager, manager.Parser.Operands);
                case CommandTypes.MF:
                    return new MFCommand(manager, manager.Parser.Operands);
                case CommandTypes.MHL:
                    return new MHLCommand(manager, manager.Parser.Operands);
                case CommandTypes.MDL:
                    return new MDLCommand(manager, manager.Parser.Operands);
                case CommandTypes.DEL:
                    return new DELCommand(manager, manager.Parser.Operands);
                case CommandTypes.COPY:
                    return new COPYCommand(manager, manager.Parser.Operands);
                case CommandTypes.MOVE:
                    return new MOVECommand(manager, manager.Parser.Operands);
                case CommandTypes.DIR:
                    return new DIRCommand(manager, manager.Parser.Operands);
                default:
                    throw new UnknownCommandException(commandLine);
            }
        }
    }
}