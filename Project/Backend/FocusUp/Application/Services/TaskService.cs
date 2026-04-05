using System;

namespace FocusUp.Application.Services
{
    public class TaskService
    {
        private readonly TaskRepository _taskRepository;
        private readonly UserStatsRepository _userStatsRepository;

        public TaskService(TaskRepository taskRepository, UserStatsRepository userStatsRepository)
        {
            _taskRepository = taskRepository;
            _userStatsRepository = userStatsRepository;
        }

        public Task? GetTaskById(int id)
        {
            throw new NotImplementedException();
        }

        public List<Task> GetTaskByUserId(int userId)
        {
            throw new NotImplementedException();
        }

        public int CreateTask(Task task)
        {
            throw new NotImplementedException();
        }

        public void UpdateTask(Task task)
        {
            throw new NotImplementedException();
        }

        public void DeleteTask(int id)
        {
            throw new NotImplementedException();
        }

        public List<Task> GetOpenTasksByUserId(int userId)
        {
            throw new NotImplementedException();
        }

        public List<Task> GetCompletedTasksByUserId(int userId)
        {
            throw new NotImplementedException();
        }
    }
}