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

        public Task? GetTaskById(int id) => _taskRepository.GetById(id);

        public List<Task> GetTaskByUserId(int userId) => _taskRepository.GetAllByUserId(userId);

        public int CreateTask(Task task) => _taskRepository.Insert(task);

        public void UpdateTask(Task task) => _taskRepository.Update(task);

        public void DeleteTask(int id) => _taskRepository.Delete(id);

        public List<Task> GetOpenTasksByUserId(int userId) => _taskRepository.GetOpenByUserId(userId);

        public List<Task> GetCompletedTasksByUserId(int userId) => _taskRepository.GetCompletedByUserId(userId);
    }
}