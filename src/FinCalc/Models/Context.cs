namespace FinCalc.Models;

public class CustomContext
{
	private readonly List<string> Notes = [];

	public void AddNote(string message)
	{
		Notes.Add(message);
	}

	public IReadOnlyList<string> GetNotes()
	{
		return Notes;
	}

	public void AddWarning(string message) => AddNote($"Warning: {message}");
	public void AddInfo(string message) => AddNote($"Info: {message}");
}