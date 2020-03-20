using System;
using System.Collections.Generic;

namespace ChessRPGMac
{
    public class TaskManager : ITask
    {
        public event TaskFinishedHandler TaskFinished;

        List<ITask> tasksToDo;

        public TaskManager(params ITask[] tasks)
        {
            tasksToDo = new List<ITask>();
            foreach (ITask task in tasks)
            {
                if (task == null)
                    continue;
                tasksToDo.Add(task);
                task.TaskFinished += FinishedTaskCheck;
            }
        }

        public void AddTask(ITask task)
        {
            tasksToDo.Add(task);
            task.TaskFinished += FinishedTaskCheck;
        }

        public void StartTask()
        {
            foreach (ITask task in tasksToDo)
                task.StartTask();
        }

        public bool IsEmpty() { return tasksToDo.Count == 0; }

        private void FinishedTaskCheck(ITask task)
        {
            if (!tasksToDo.Remove(task))
                throw new TaskNotIncludedException();
            if (tasksToDo.Count == 0)
                TaskFinished(this);
        }

        public class TaskNotIncludedException : Exception
        {

        }
    }
}
