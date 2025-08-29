using System;
using System.Collections.Generic;

namespace KanbanBoard.Models
{
    public class Comment
    {
        public Guid Id { get; set; }          // Unique ID for the comment
        public Guid TaskId { get; set; }      // The task this comment belongs to
        public string Content { get; set; }   // Comment text
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}