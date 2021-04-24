using System;

namespace Damrem.Commands {

    public abstract class Command {

        internal abstract void Execute();

        internal event Action OnCompleted;

        protected void Complete() {
            OnCompleted.Invoke();
            OnCompleted = null;
        }
    }
}