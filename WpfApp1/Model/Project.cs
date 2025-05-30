using System;
using System.Collections.Generic;
using WpfApp1.Model;
using WpfApp1.Models;

public class Project
{
    public long Id { get; set; }
    public string Details { get; set; }
    public string Name { get; set; }
    public DateTime? Start { get; set; }
    public ProjectStatus Status { get; set; }
    public long ClientId { get; set; }
    public Client Client { get; set; }

    public ICollection<CandidateSelection> CandidateSelections { get; set; }
    public ICollection<ProjectEmployee> ProjectEmployees { get; set; }
    public ICollection<ProjectVersion> ProjectVersions { get; set; } = new List<ProjectVersion>();
    public Project()
    {
        CandidateSelections = new HashSet<CandidateSelection>();
        ProjectEmployees = new HashSet<ProjectEmployee>();
    }
}

public enum ProjectStatus
{
    Planned,
    InProgress,
    Completed
}
