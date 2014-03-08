using System;
using System.Linq;
using OrigoDB.Core;

namespace Todo.Core.Commands
{
    [Serializable]
    public class ClearCompletedCommand : Command<TodoModel>
    {
        private Guid _taskId;


        public ClearCompletedCommand(Guid taskId)
        {
            _taskId = taskId;
        }

        protected override void Execute(TodoModel model)
        {
            var task = model.Lists.SelectMany(l => l.Tasks).Single(t => t.Id == _taskId);
            task.Completed = null;
        }
    }
}