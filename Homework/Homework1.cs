// Homework 1 (To-Do List) - rename to Homework1_nmxxxxx.cs and copy to StudentWork (then delete this)
// Create a To-Do list
//      - create a class called "Task" with properties for the task name, description, and completion status
//      - create a class called "ToDoList" that contains a list of Task objects and methods to add, remove, and mark tasks as complete
//      - create a command to add a task to the ToDoList, remove a task from the ToDoList, and mark a task as complete
//      - create a command to display the list of tasks and their completion status
//      - after updating the list, save a json file of the list (load file when necessary)

namespace StudentWork;

public class ToDoTask(string name, string description)
{
    public string Name { get; set; } = name;
    public string Description { get; set; } = description;
    public bool IsComplete { get; set; } = false;

    public void MarkComplete()
    {
        IsComplete = true;
    }

    public override string ToString()
    {
        return $"{Name}\n{Description}\n{(IsComplete ? "Complete" : "Incomplete")}";
    }
}