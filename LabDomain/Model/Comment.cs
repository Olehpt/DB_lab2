using System;
using System.Collections.Generic;

namespace LabDomain.Model;

public partial class Comment
{
    public int CommentId { get; set; }

    public string Content { get; set; } = null!;

    public DateOnly PublicationDate { get; set; }

    public int Author { get; set; }

    public int Publication { get; set; }
}
