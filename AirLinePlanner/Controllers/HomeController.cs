//
// using Microsoft.AspNetCore.Mvc;
// using AirLinePlanner.Models;
// using System;
// using System.Collections.Generic;
//
// namespace AirLinePlanner.Controllers
// {
//   public class HomeController : Controller
//   {
//     [HttpGet("/")]
//   public ActionResult Index()
//   {
//       return View();
//   }
//   [HttpGet("/tasks")]
//   public ActionResult Tasks()
//   {
//       var model = new Dictionary<string, object>();
//       List<Task> allTasks = Task.GetAll();
//       List<Task> finishedTasks = Task.GetFinished();
//       model.Add("unfinished", allTasks);
//       model.Add("finished", finishedTasks);
//       return View(model);
//   }
//   [HttpGet("/categories")]
//   public ActionResult Categories()
//   {
//       List<Category> allCategories = Category.GetAll();
//       return View(allCategories);
//   }
// //NEW TASK
//     [HttpGet("/tasks/new")]
//     public ActionResult TaskForm()
//     {
//         return View();
//     }
//     [HttpPost("/tasks/new")]
//     public ActionResult TaskCreate()
//     {
//       DateTime dueDate = DateTime.Parse(Request.Form["task-duedate"]);
//         Task newTask = new Task(Request.Form["task-description"], dueDate);
//
//         newTask.Save();
//         return View("Success");
//     }
//
// //NEW CATEGORY
//     [HttpGet("/categories/new")]
//     public ActionResult CategoryForm()
//     {
//         return View();
//     }
//     [HttpPost("/categories/new")]
//     public ActionResult CategoryCreate()
//     {
//         Category newCategory = new Category(Request.Form["category-name"]);
//         newCategory.Save();
//         return View("Success");
//     }    // [HttpGet("/showCategory")]
// //ONE TASK
//     [HttpGet("/tasks/{id}")]
//     public ActionResult TaskDetail(int id)
//     {
//         Dictionary<string, object> model = new Dictionary<string, object>();
//         Task selectedTask = Task.Find(id);
//         List<Category> TaskCategories = selectedTask.GetCategories();
//         List<Category> AllCategories = Category.GetAll();
//         model.Add("task", selectedTask);
//         model.Add("taskCategories", TaskCategories);
//         model.Add("allCategories", AllCategories);
//         return View( model);
//
//     }
//
//
//
// //ONE CATEGORY
//     [HttpGet("/categories/{id}")]
//     public ActionResult CategoryDetail(int id)
//     {
//         Dictionary<string, object> model = new Dictionary<string, object>();
//         Category SelectedCategory = Category.Find(id);
//         List<Task> CategoryTasks = SelectedCategory.GetTasks();
//         List<Task> AllTasks = Task.GetAll();
//         model.Add("category", SelectedCategory);
//         model.Add("categoryTasks", CategoryTasks);
//         model.Add("allTasks", AllTasks);
//         return View(model);
//     }
//     [HttpPost("task/add_category")]
//     public ActionResult TaskAddCategory()
//     {
//         Category category = Category.Find(Int32.Parse(Request.Form["category-id"]));
//         Task task = Task.Find(Int32.Parse(Request.Form["task-id"]));
//         task.AddCategory(category);
//         return View("Success");
//     }
//
//     //ADD TASK TO CATEGORY
//     [HttpPost("category/add_task")]
//     public ActionResult CategoryAddTask()
//     {
//         Category category = Category.Find(Int32.Parse(Request.Form["category-id"]));
//         Task task = Task.Find(Int32.Parse(Request.Form["task-id"]));
//         category.AddTask(task);
//         return View("Success");
//     }
//     [HttpGet("/task/{id}/finished")]
//     public ActionResult FinishedTask(int id)
//     {
//       Task.Find(id).FinishTask();
//
//       return View("Success");
//     }
//   }
// }
