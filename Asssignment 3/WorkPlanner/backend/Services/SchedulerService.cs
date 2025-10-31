using System;
using System.Collections.Generic;
using System.Linq;
using WorkPlanner.API.Models;

namespace WorkPlanner.API.Services;

public class SchedulerService
{
    public ScheduleResponse ScheduleTasks(ScheduleRequest request)
    {
        try
        {
            var tasks = request.Tasks;
            
            // Validate input
            if (tasks == null || !tasks.Any())
            {
                return new ScheduleResponse { Error = "No tasks provided" };
            }

            // Check for missing dependencies
            var taskTitles = new HashSet<string>(tasks.Select(t => t.Title));
            var missingDependencies = tasks
                .SelectMany(t => t.Dependencies)
                .Where(dep => !taskTitles.Contains(dep))
                .Distinct()
                .ToList();

            if (missingDependencies.Any())
            {
                return new ScheduleResponse 
                { 
                    Error = $"Tasks have missing dependencies: {string.Join(", ", missingDependencies)}" 
                };
            }

            // Check for circular dependencies
            if (HasCircularDependencies(tasks))
            {
                return new ScheduleResponse { Error = "Circular dependency detected in tasks" };
            }

            // Perform topological sort with due date as tiebreaker
            var sortedTasks = TopologicalSort(tasks);
            
            return new ScheduleResponse 
            { 
                RecommendedOrder = sortedTasks.Select(t => t.Title).ToList() 
            };
        }
        catch (Exception ex)
        {
            return new ScheduleResponse { Error = $"Error scheduling tasks: {ex.Message}" };
        }
    }

    private bool HasCircularDependencies(List<TaskDto> tasks)
    {
        var visited = new HashSet<string>();
        var recursionStack = new HashSet<string>();
        var taskMap = tasks.ToDictionary(t => t.Title);

        foreach (var task in tasks)
        {
            if (HasCycle(task, taskMap, visited, recursionStack))
            {
                return true;
            }
        }

        return false;
    }

    private bool HasCycle(
        TaskDto task, 
        Dictionary<string, TaskDto> taskMap,
        HashSet<string> visited, 
        HashSet<string> recursionStack)
    {
        if (recursionStack.Contains(task.Title))
        {
            return true;
        }

        if (visited.Contains(task.Title))
        {
            return false;
        }

        visited.Add(task.Title);
        recursionStack.Add(task.Title);

        foreach (var depTitle in task.Dependencies)
        {
            if (taskMap.TryGetValue(depTitle, out var dependentTask))
            {
                if (HasCycle(dependentTask, taskMap, visited, recursionStack))
                {
                    return true;
                }
            }
        }

        recursionStack.Remove(task.Title);
        return false;
    }

    private List<TaskDto> TopologicalSort(List<TaskDto> tasks)
    {
        var taskMap = tasks.ToDictionary(t => t.Title);
        var visited = new HashSet<string>();
        var result = new List<TaskDto>();
        var temp = new HashSet<string>();

        foreach (var task in tasks)
        {
            if (!visited.Contains(task.Title))
            {
                Visit(task, taskMap, visited, temp, result);
            }
        }

        // Sort tasks with the same dependencies by due date
        return result
            .OrderBy(t => t.DueDate)
            .ThenBy(t => t.Title)
            .ToList();
    }

    private void Visit(
        TaskDto task, 
        Dictionary<string, TaskDto> taskMap,
        HashSet<string> visited, 
        HashSet<string> temp, 
        List<TaskDto> result)
    {
        if (temp.Contains(task.Title))
        {
            return;
        }

        if (visited.Contains(task.Title))
        {
            return;
        }

        temp.Add(task.Title);

        foreach (var depTitle in task.Dependencies)
        {
            if (taskMap.TryGetValue(depTitle, out var dependentTask))
            {
                Visit(dependentTask, taskMap, visited, temp, result);
            }
        }

        temp.Remove(task.Title);
        visited.Add(task.Title);
        result.Add(task);
    }
}
