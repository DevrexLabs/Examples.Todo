using System;
using System.Collections.Generic;
using System.Linq;
using OrigoDB.Core;
using Todo.Core;

namespace Todo.CLI
{
    class Program
    {
        private static List<string> lists;
        private static string currentList;
        static List<TaskInfo> currentTasks;
        private static IEngine<TodoModel> _db;

        static void Main(string[] args)
        {
            var mode = args.Length > 0 ? args[0] : "embedded";
            var host = args.Length > 1 ? args[1] : "localhost";
            var port = args.Length > 2 ? Int32.Parse(args[2]) : 3001;
            

            var connectionString = string.Format("mode={0};host={1};port={2}", mode, host, port);

            Console.Write("Connecting/Loading...");
            _db = Engine.For<TodoModel>(connectionString);
            Console.WriteLine("ok!");

            Console.Write("Retrieving list names...");
            lists = _db.Execute(new GetListNames()).ToList();
            Console.WriteLine("ok!");
            Console.WriteLine("Type help for commands, exit to quit");

            currentList = lists.FirstOrDefault();
            
            if(currentList != null) GetCurrentTasks();

            while(true)
            {
                Console.Write("[{0}]> ", currentList);
                string line = Console.ReadLine();

                if( line == null || line.ToLower() == "exit") break;
                else if (line.StartsWith("addlist "))
                {
                    string listName = line.Substring(8);
                    if (lists.Contains(listName))
                    {
                        Console.WriteLine("List already exists");
                    }
                    else
                    {
                        lists.Add(listName);
                        _db.Execute(new AddListCommand(listName));
                        if(lists.Count == 1)
                        {
                            currentList = listName;
                            currentTasks = new List<TaskInfo>();
                        }
                    }
                }
                else if (line.StartsWith("setlist "))
                {
                    string listName = line.Substring(8);
                    if (!lists.Contains(listName)) Console.WriteLine("No such list");
                    else
                    {
                        currentList = listName;
                        GetCurrentTasks();
                    }
                }
                else if (line.ToLower() == "help") Console.WriteLine("commands: exit, addlist <list>, setlist <list>, add <item>, showlists, showitems");
                else if (line.StartsWith("add "))
                {
                    if(currentList == null)
                    {
                        Console.WriteLine("You must add a list before you can add items!");
                    }
                    string item = line.Substring(4);
                    if(currentTasks.Any(t => t.Title == item))
                    {
                        Console.WriteLine("Item already exists");
                    }
                    else
                    {
                        _db.Execute(new AddTaskCommand(new Task(item), currentList));
                        GetCurrentTasks();
                    }
                }
                else if (line.ToLower() == "showlists")
                {
                    lists.ForEach(Console.WriteLine);
                }
                else if (line.ToLower() == "showitems")
                {
                    currentTasks.ForEach(t => Console.WriteLine("{0}", t.Title));
                }
                else
                {
                    Console.WriteLine("ERROR, unrecognized command");
                }
            }
            Config.Engines.CloseAll();
        }

        private static void GetCurrentTasks()
        {
            currentTasks = _db.Execute(new GetTasksByListName(currentList)).ToList();
        }

    }
}
