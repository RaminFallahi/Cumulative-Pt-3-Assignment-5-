using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Assignment3_try2_RaminFallahi.Models; //2-try this line as Comment to see where you get error //added
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Assignment3_try2_RaminFallahi.Controllers
{
    public class TeacherController : Controller
    {

        // GET: /Teacher/List?SearchKey=Simon
        public ActionResult List(string SearchKey = null)
        {
            // want to recieve search key and print it out
            Debug.WriteLine("The user is tring to search for "+ SearchKey);

            //I want to recieve all teachers in the system
            TeacherDataController MyController = new TeacherDataController();
            
            IEnumerable<teacher> MyTeacher = MyController.ListTeachers(SearchKey);

            Debug.WriteLine("I have access " + MyTeacher.Count());

            return View(MyTeacher);
        }
        //GET : /Author /Show/{id}
        public ActionResult Show(int id)
        {
            TeacherDataController MyController = new TeacherDataController();
            teacher SelectedTeacher = MyController.FindTeacher(id);

            return View(SelectedTeacher);
        }

        // GET /teacher/new
        public ActionResult New()
        {
            return View();
        }

        //POST: /Teacher/Create
        [HttpPost]
        public ActionResult Create(string teacherfname, string teacherlname, string employeenumber, DateTime hiredate, Decimal salary)
        {
            Debug.WriteLine("I have accessed the Create Method!");
            Debug.WriteLine(teacherfname);
            Debug.WriteLine(teacherlname);
            Debug.WriteLine(employeenumber);
            Debug.WriteLine(hiredate);
            Debug.WriteLine(salary);

            teacher NewTeacher = new teacher();
            NewTeacher.teacherfname = teacherfname;
            NewTeacher.teacherlname = teacherlname;
            NewTeacher.employeenumber = employeenumber;
            NewTeacher.hiredate = hiredate;
            NewTeacher.salary = salary;

            TeacherDataController MyController = new TeacherDataController();

            MyController.AddTeacher(NewTeacher);

            return RedirectToAction("List");
        }

        //GET: /Teacher/DeleteConfirm/{id}

        public ActionResult DeletConfirm(int id)
        {
            //get info about the teacher
            //Navigate to / View/Teacher/deleteConfirm.cshtml
            TeacherDataController MyController = new TeacherDataController();
            teacher NewTeacher = MyController.FindTeacher(id);
            return View(NewTeacher);
        }

        //POST: /Teacher/Delete/{id}
        [HttpPost]
        
        public ActionResult Delete(int id)
        {
            TeacherDataController MyController = new TeacherDataController();
            MyController.DeleteTeacher(id);
            return RedirectToAction("List");
        }

        //GET: /Teacher/Edit/{id}
        public ActionResult Edit(int id)
        {
            //get info about the teacher
            //Navigate to / View/Teacher/edit.cshtml
            TeacherDataController MyController = new TeacherDataController();
            teacher SelectedTeacher = MyController.FindTeacher(id);
            return View(SelectedTeacher);
        }

        [HttpPost]
        public ActionResult Update(int id, string teacherfname, string teacherlname, string employeenumber, DateTime hiredate, Decimal salary)
        {
            teacher teacherInfo = new teacher();
            teacherInfo.teacherfname = teacherfname;
            teacherInfo.teacherlname = teacherlname;
            teacherInfo.employeenumber = employeenumber;
            teacherInfo.hiredate = hiredate;
            teacherInfo.salary = salary;

            TeacherDataController MyController = new TeacherDataController();
            MyController.UpdateTeacher(id, teacherInfo);

            return RedirectToAction("Show/" + id);
        }

    }
}