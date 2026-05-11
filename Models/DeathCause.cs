namespace docs_project.Models;

public class DeathCause
{
    public string Year { get; set; } = "";
    public string LeadingCause { get; set; } = "";
    public string Sex { get; set; } = "";
    public string RaceEthnicity { get; set; } = "";
    public string Deaths { get; set; } = "";
    public string DeathRate { get; set; } = "";
    public string AgeAdjustedDeathRate { get; set; } = "";

    public override string ToString() =>
        $"[{Year}] {LeadingCause} | {Sex} | {RaceEthnicity} | Deaths: {Deaths} | Rate: {DeathRate} | Adj.Rate: {AgeAdjustedDeathRate}";
}
