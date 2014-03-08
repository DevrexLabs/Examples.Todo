using System;
using System.Linq;
using OrigoDB.Core;

namespace Todo.Core
{
    [Serializable]
    public class GetListNames : Query<TodoModel,string[]>
    {
        protected override string[] Execute(TodoModel m)
        {
            return m.Lists.Select(list => list.Name).ToArray();
        }
    }

    [Serializable]
    public class GetTasksByListName : Query<TodoModel, TaskInfo[]>
    {
        private string _listName;

        public GetTasksByListName(string listName)
        {
            _listName = listName;
        }

        protected override TaskInfo[] Execute(TodoModel m)
        {
            return m.Lists
                .Single(list => list.IsNamed(_listName))
                .Tasks.Select(task => new TaskInfo(task)).ToArray();
        }
    }
}
