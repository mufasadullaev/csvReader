using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using synelApp.Data;
using synelApp.Models;

namespace synelApp.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context) // constructor to pass database connection to this controller
        {
            _context = context;
        }

        public IActionResult Index() // action for routing to Index.cshtml
        {
            var employees = _context.Employees.OrderBy(e => e.Surname).ToList(); // sort by surname
            return View(employees); // display the list
        }

        [HttpPost]
        public IActionResult Import(IFormFile file) // that's for Import button
        {
            if (file == null || file.Length == 0)
            {
                TempData["Error"] = "File not chosen."; 
                return RedirectToAction("Index"); // In case nothing is chosen or empty file
            }

            using (var reader = new StreamReader(file.OpenReadStream())) // "using" makes sure its closed after opening and reading the file with OpenReadStream()
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture))) // CsvHelper parses the csv file
            {
                csv.Context.RegisterClassMap<EmployeeMap>(); 

                var records = csv.GetRecords<Employee>().ToList(); // get all rows into "records" list

                foreach (var record in records)
                {
                    if (!_context.Employees.Any(e => e.Payroll_Number == record.Payroll_Number)) // making sure new payroll numbers don't match existing ones
                    {
                        _context.Employees.Add(record); // if no duplicates, then insert rows
                    }
                }

                _context.SaveChanges(); // update the db
                TempData["Success"] = $"{records.Count} rows were added successfully.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateField(int id, string column, string value) // parameters are: row id, column name and the new value
        {
            var employee = _context.Employees.Find(id); // get row Id
            if (employee == null) return NotFound();  // error if no Id found

            var property = typeof(Employee).GetProperty(column);
            if (property == null) return BadRequest("Invalid column");

            // convert date fields correctly
            if (property.PropertyType == typeof(DateTime)) // check if its a datetime type, because we need to make sure its correctly formatted
            {
                if (DateTime.TryParse(value, out DateTime dateValue)) // if the input is valid, then insert into the Db
                {
                    property.SetValue(employee, dateValue);
                }
                else
                {
                    return BadRequest("Invalid date format"); // otherwise throw err
                }
            }
            else
            {
                property.SetValue(employee, value); // if not a date input, then just make the changes
            }

            _context.SaveChanges(); // update the db
            return Ok();
        }
    }
}
