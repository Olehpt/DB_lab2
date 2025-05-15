using System;
using System.Collections.Generic;

namespace LabDomain.Model;

public partial class Author
{
    public int AuthorId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Info { get; set; }

    public DateOnly SignUpDate { get; set; }

    public int? Organization { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual Organization? OrganizationNavigation { get; set; }

    public virtual ICollection<Publication> Publications { get; set; } = new List<Publication>();
}
