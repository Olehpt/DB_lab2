using System;
using System.Collections.Generic;

namespace LabDomain.Model;

public partial class Subject
{
    public int SubjectId { get; set; }

    public string Name { get; set; } = null!;

    public string? Info { get; set; }
}
