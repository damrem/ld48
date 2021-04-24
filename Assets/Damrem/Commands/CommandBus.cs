using Damrem.Collections;
using System.Collections.Generic;

namespace Damrem.Commands {



    public class CommandBus {

        static CommandBus instance;
        public static CommandBus Instance {
            get {
                if (instance == null) instance = new CommandBus();
                return instance;
            }
        }
        CommandBus() { }

        readonly List<Command> CommandQueue = new List<Command>();

        public void QueueCommand(Command command) {
            CommandQueue.Add(command);

            if (CommandQueue.Count == 1) ExecuteNextCommand();
        }

        void ExecuteNextCommand() {
            if (CommandQueue.Count == 0) return;

            var command = CommandQueue.Shift();
            command.Execute();
            command.OnCompleted += ExecuteNextCommand;
        }
    }
}