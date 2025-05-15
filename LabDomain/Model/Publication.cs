using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LabDomain.Model;

public partial class Publication
{
    public int PublicationId { get; set; }
    [MinLength(4, ErrorMessage = "Name must be at least 4 characters long")]
    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateOnly PublicationDate { get; set; }

    public int Views { get; set; }

    public int Subject { get; set; }

    public int PublicationType { get; set; }

    public int Author { get; set; }

    public virtual Author AuthorNavigation { get; set; } = null!;

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual PublicationType PublicationTypeNavigation { get; set; } = null!;

    public virtual Subject SubjectNavigation { get; set; } = null!;
}
