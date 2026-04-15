using FocusUp.Infrastructure.Repositories;
using System;
using FocusUp.Common.Exceptions;

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

        public int CreateTask(Task task) {

            var userStats = _userStatsRepository.GetByUserId(task.UserId) ?? throw new UserStatsNotFoundException(task.UserId);
            userStats.IncrementTasksOpen();
            _userStatsRepository.UpdateTaskCounters(userStats.UserId, userStats.TasksDone, userStats.TasksOpen);

            return _taskRepository.Insert(task);
        }
        

        public void UpdateTask(Task task) =>_taskRepository.Update(task);

        public void DeleteTask(int id) {

            var task = GetTaskById(id) ?? throw new TaskNotFoundException(id);

            var userStats = _userStatsRepository.GetByUserId(task.UserId) ?? throw new UserStatsNotFoundException(task.UserId);
            userStats.DecrementTasksOpen();
            _userStatsRepository.UpdateTaskCounters(userStats.UserId, userStats.TasksDone, userStats.TasksOpen);

            _taskRepository.Delete(id);
        } 

        public List<Task> GetOpenTasksByUserId(int userId) => _taskRepository.GetOpenByUserId(userId);

        public List<Task> GetCompletedTasksByUserId(int userId) => _taskRepository.GetCompletedByUserId(userId);
    }
}