using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LabDomain.Model;

public partial class Subject
{
    public int SubjectId { get; set; }
    [MinLength(2, ErrorMessage = "Name must be at least 2 characters long")]
    public string Name { get; set; } = null!;

    public string? Info { get; set; }

    public virtual ICollection<Publication> Publications { get; set; } = new List<Publication>();
}
