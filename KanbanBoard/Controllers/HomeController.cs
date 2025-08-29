using Microsoft.AspNetCore.Mvc;
using KanbanBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KanbanBoard.Controllers
{
    public class HomeController : Controller
    {
        private static List<KanbanTask> tasks = new List<KanbanTask>();

        public IActionResult Index() => View(tasks);

        [HttpPost]
        public IActionResult AddTask([FromBody] TaskInputModel input)
        {
            if (input == null || string.IsNullOrWhiteSpace(input.Title))
                return BadRequest("Task input is null or missing Title.");

            tasks.Add(new KanbanTask
            {
                Id = Guid.NewGuid(),
                Title = input.Title,
                Description = input.Description,
                Urgency = input.Urgency, // just assign the string
                Status = StatusOfTask.ToDo,
                Comments = new List<Comment>(),
                DueDate = input.DueDate
            });

            return Ok();
        }

        [HttpPost]
        public IActionResult MoveTask([FromBody] MoveTaskInput input)
        {
            if (!Guid.TryParse(input.Id, out var taskId))
                return BadRequest("Invalid task id.");

            var task = tasks.FirstOrDefault(t => t.Id == taskId);
            if (task != null)
            {
                task.Status = Enum.Parse<StatusOfTask>(input.NewStatus);
                // comments are still stored in task.Comments
            }
            return Ok();
        }

        [HttpPost]
        public IActionResult AddComment([FromBody] Comment input)
        {
            if (input is null) return BadRequest("No input provided.");
            if (input.TaskId == Guid.Empty) return BadRequest("Invalid task id.");
            if (string.IsNullOrWhiteSpace(input.Content)) return BadRequest("Comment is empty.");

            var task = tasks.FirstOrDefault(t => t.Id == input.TaskId);
            if (task is null) return NotFound("Task not found.");

            // Ensure the list exists and append a new comment
            task.Comments ??= new List<Comment>();
            var comment = new Comment
            {
                TaskId = input.TaskId,
                Content = input.Content.Trim()
            };
            task.Comments.Add(comment);

            // Return the new comment metadata in case the client wants to render it without reload
            return Ok(new
            {
                commentId = comment.Id,
                createdAt = comment.CreatedAt
            });
        }

        [HttpPost]
        public IActionResult DeleteTask([FromBody] DeleteTaskInput input)
        {
            var task = tasks.FirstOrDefault(t => t.Id == Guid.Parse(input.TaskId));
            if (task != null) tasks.Remove(task);
            return Ok();
        }
    }

    // Models for inputs
    public class TaskInputModel { public string Title { get; set; } public string Description { get; set; } public string Urgency { get; set; } public DateOnly DueDate { get; set; } }
    public class MoveTaskInput { public string Id { get; set; } public string NewStatus { get; set; } }
    public class CommentInput { public string TaskId { get; set; } public string Content { get; set; } }
    public class DeleteTaskInput { public string TaskId { get; set; } }
}

