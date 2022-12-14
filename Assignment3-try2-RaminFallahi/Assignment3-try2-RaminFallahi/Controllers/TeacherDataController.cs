//this web api controller will listen to the http requests on the web server

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Assignment3_try2_RaminFallahi.Models; //2-try this line as Comment to see where you get error
using MySql.Data.MySqlClient;//3-try this line as Comment to see where you get error
//using Renci.SshNet.Security.Cryptography;

namespace Assignment3_try2_RaminFallahi.Controllers
{
    public class TeacherDataController : ApiController
    {
        private SchoolDbContext School = new SchoolDbContext();


        ///<summary>
        ///returns a list of teachers in the system. 
        ///If a search key is included we will match the teachers title to the search key.
        ///</summary>
        ///<example>
        ///GET/api/teacherdata/listteachers -> {teacherid: 1, teachertitle: teacher Simon ..."},
        ///{teacher:2 teacherfname OR teacherlname:" teacher Simon ..."}
        ///</example>
        ///<param name="SearchKey">The text to search against</param>
        /// 
        /// <summary>
        /// Returns a list of Teacher in the system
        /// </summary>
        /// <example>GET api/TeacherData/ListTeachers</example>
        /// <returns>
        /// A list of Teachers (teacher first names,employeenumber, teacherId, salary, hiredate last names)
        /// </returns>
        /// 

        [HttpGet]
        [Route("api/teacherdata/listteachers/{SearchKey}")]
        public IEnumerable<teacher> ListTeachers(string SearchKey = null)
        {
            //1-Create an instance of a connection to database
            MySqlConnection Conn = School.AccessDatabase();

            //open the connection between the web server and database.
            Conn.Open();

            Debug.WriteLine("The search key is " + SearchKey);
            MySqlCommand cmd = Conn.CreateCommand();

            //excute the query
            string query = "select * from teachers where teacherfname like @key OR teacherlname like @key or employeenumber like @key or hiredate like @key or salary like @key";
            Debug.WriteLine("The query is" + query);

            cmd.CommandText = query;

            cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");//sanetising the inpute
            cmd.Prepare();//preparitising the input


            MySqlDataReader Resultset = cmd.ExecuteReader();

            List<teacher> Teachers = new List<teacher>();

            while (Resultset.Read())
            {
                int teacherid = Convert.ToInt32(Resultset["teacherid"]);
                string teacherfname = Resultset["teacherfname"].ToString();
                string teacherlname = Resultset["teacherlname"].ToString();
                string employeenumber = Resultset["employeenumber"].ToString();
                DateTime hiredate = Convert.ToDateTime(Resultset["hiredate"]);
                decimal salary = Convert.ToDecimal(Resultset["salary"].ToString());

                teacher Newteacher = new teacher();

                Newteacher.teacherid = teacherid;
                Newteacher.teacherfname = teacherfname;
                Newteacher.teacherlname = teacherlname;
                Newteacher.employeenumber = employeenumber;
                Newteacher.hiredate = hiredate;
                Newteacher.salary = salary;

                Teachers.Add(Newteacher);
            }
            Conn.Close();
            return Teachers;


        }

        [HttpGet]
        [Route("api/teacherdate/FindTeacher/{teacherid}")]

        public teacher FindTeacher(int id)
        {
            teacher Newteacher = new teacher();

            MySqlConnection Conn = School.AccessDatabase();

            Conn.Open();

            string query = "select * from teachers where teacherId = " + id;

            MySqlCommand cmd = Conn.CreateCommand();
            cmd.CommandText = query;

            cmd.Parameters.AddWithValue("@id", id);//sanetising the inpute


            MySqlDataReader ResultSet = cmd.ExecuteReader();
            cmd.CommandText = query;

           // cmd.Prepare();//preparitising the input

            teacher SelectedTeacher = new teacher();
            while (ResultSet.Read())
            {
                SelectedTeacher.teacherid = Convert.ToInt32(ResultSet["teacherid"]);
                SelectedTeacher.teacherfname = ResultSet["teacherfname"].ToString();
                SelectedTeacher.teacherlname = ResultSet["teacherlname"].ToString();
                SelectedTeacher.employeenumber = ResultSet["employeenumber"].ToString();
                SelectedTeacher.hiredate = Convert.ToDateTime(ResultSet["hiredate"]);
                SelectedTeacher.salary = Convert.ToDecimal(ResultSet["salary"].ToString());
            }
            Conn.Close();
            return SelectedTeacher;
        }


        /// <summary>
        /// Adds a new teacher to our system
        /// </summary>
        /// <param name="NewTeacher">Teacher Object</param>
        /// <example>
        /// api/TeacherData/TeacherArticle
        /// POST api/TeacherData/AddTeacher 
        /// </example>
        [HttpPost]
        public void AddTeacher ([FromBody]teacher NewTeacher)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            string query = "insert into teachers (teacherfname, teacherlname, employeenumber, hiredate, salary) values (@teacherfname, @teacherlname, @employeenumber, @hiredate, @salary)";
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@teacherfname", NewTeacher.teacherfname);
            cmd.Parameters.AddWithValue("@teacherlname", NewTeacher.teacherlname);
            cmd.Parameters.AddWithValue("@employeenumber", NewTeacher.employeenumber);
            cmd.Parameters.AddWithValue("@hiredate", NewTeacher.hiredate);
            cmd.Parameters.AddWithValue("@salary", NewTeacher.salary);

            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();
        }

        /// <summary>
        /// this method will delete a teacher in the database
        /// </summary>
        /// <param name="teacherid">The teacher id primary key</param>
        /// <returns></returns>
        /// <example> 
        /// POST : api/teacherdata/deleteteacher/100
        /// </example>
        [HttpPost]
        [Route("api/teacherData/DeleteTeacher/{teacherid}")]
        public void DeleteTeacher (int teacherid)
        {
            ///WEHNR useing void insted of the string no return is needed.
            ///return "deleting teacher"; 

            
            ///Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            string query = "delete from teachers where teacherid=@id";

            cmd.CommandText = query;

            cmd.Parameters.AddWithValue("@id", teacherid);

            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();
        }

        /// <summary>
        /// Updates an Teacher on the MySQL Database.
        /// </summary>
        /// <param name="TeacherInfo">An object with fields that map to the columns of the Teacher's table.</param>
        /// <example>
        /// POST api/TEacherData/UpdateTeacher/208 
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"Teacherfname":"Christine",
        ///	"Teacherlname":"Bittle",
        ///	"employeenumber":"123g!",
        ///	"hiredate":"christine@test.ca",
        ///	"salary":10,000"
        /// }
        /// </example>
        [HttpPost]
        
        public void UpdateTeacher(int teacherid, [FromBody] teacher TeacherInfo)
        {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();
            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();
            //SQL QUERY
            string query = "update teachers set teacherfname=@TeacherFname, teacherlname=@TeacherLname, employeenumber=@employeenumber, hiredate=@hiredate, salary=@salary  where teacherid=@TeacherId";
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@TeacherFname", TeacherInfo.teacherfname);
            cmd.Parameters.AddWithValue("@TeacherLname", TeacherInfo.teacherlname);
            cmd.Parameters.AddWithValue("@employeenumber", TeacherInfo.employeenumber);
            cmd.Parameters.AddWithValue("@hiredate", TeacherInfo.hiredate);
            cmd.Parameters.AddWithValue("@salary", TeacherInfo.salary);
            cmd.Parameters.AddWithValue("@TeacherId", teacherid);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            Conn.Close();

        }
    }
}

///curl -H "Content-Type:application/json" -d "{\"Teachertitel\":\"Test Teacher
