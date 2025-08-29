using System;
using System.Collections.Generic;

namespace KanbanBoard.Models
{
    public class KanbanTask
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Description { get; set; }
        public StatusOfTask Status { get; set; } = StatusOfTask.ToDo;
        public List<Comment> Comments { get; set; } = new List<Comment>();

        public string Urgency { get; set; }

        public DateOnly DueDate { get; set; }
    }

    public enum StatusOfTask
    {
        ToDo,
        InProgress,
        Completed,
        Resolved
    }
}

