using System.Collections.Generic;

namespace CybersecurityChatBot
{
    public class TaskManager
    {
        private TaskStorageHelper _storage;

        public TaskManager()
        {
            _storage = new TaskStorageHelper();
        }
        //Add a new task with optional reminder
        public string AddTask(string title, string description, string reminder)
        {
            _storage.AddTask(title, description, reminder);
            ActivityLogger.Log($"Task added: '{title}'" +
                (string.IsNullOrEmpty(reminder) ? " (no reminder set)" :
                $" (Reminder set for {reminder})"));
            return $"Task added: '{title}'. Would you like to set a reminder?";
        }

        public List<CyberTask> GetAllTasks()
        {
            return _storage.LoadTasks();
        }

        public void MarkAsComplete(int id)
        {
            var tasks = _storage.LoadTasks();
            var task = tasks.Find(t => t.Id == id);
            if (task != null)
            {
                _storage.MarkAsComplete(id);
                ActivityLogger.Log($"Task marked complete: '{task.Title}'");
            }
        }

        public void DeleteTask(int id)
        {
            var tasks = _storage.LoadTasks();
            var task = tasks.Find(t => t.Id == id);
            if (task != null)
            {
                _storage.DeleteTask(id);
                ActivityLogger.Log($"Task deleted: '{task.Title}'");
            }
        }
    }
}
