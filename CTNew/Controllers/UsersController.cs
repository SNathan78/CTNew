using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CTNew.Models;
using Azure;
using SQLitePCL;

namespace CTNew.Controllers
{
    public class UsersController : Controller
    {
        private readonly CodeTestContext _context;
       

        public UsersController(CodeTestContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var codeTestContext = _context.Users
                .Include(u => u.City)
            .ThenInclude(u => u.State);

            return View(await codeTestContext.ToListAsync());
        }


        public JsonResult GetUsers()
        {
            var users = _context.Users
                .Include(u => u.City);
            //.ThenInclude(u => u.State);
            List<Users> u = new List<Users>();
            using (var dt = new CodeTestContext())
            {
                u = (from a in _context.Users
                     join b in _context.City on a.CityId equals b.CityId
                     join c in _context.State on b.StateId equals c.StateId
                     select new { a, b, c }).ToList().Select(p => new Users()
                     {
                         UserId = (int)p.a.UserId,
                         FirstName = p.a.FirstName,
                         LastName = p.a.LastName,
                         StateName = p.c.StateName,
                         CityName = p.b.CityName,
                         CityId = p.a.CityId,
                         StateId = p.b.StateId,
                         Gender = (p.a.Gender == "1") ? "Female" : "Male",
                         GenderId = p.a.Gender
                     })
      .ToList();
            }
                return Json(u);
            //return Json(users, new System.Text.Json.JsonSerializerOptions());
            
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
       // public IActionResult Details()
        {
            //if (id == null || _context.Users == null)
            //{
            //     return NotFound();
            //}

            //    var users = _context.Users;
            //var users = await _context.Users
            //.Include(u => u.City)
            //.FirstOrDefaultAsync(m => m.UserId == id);
            var users = _context.Users
            .Include(u => u.City)
            .ThenInclude(u => u.State)
            .FirstOrDefaultAsync(m => m.UserId == id);

            if (users == null)
            {
                return NotFound();
            }
            
            return View(await users);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            


            Users model = new Users();
            //get all country  
            var UsersList = _context.Users.ToList();
            //get all states according to defaultCountryId  
            var allStatelist = _context.State.ToList();
            //set defaultStateId  
            var defaultStateId = allStatelist.Select(m => m.StateId).FirstOrDefault();
            //Get all cities according to defaultStateId  
            var allCitylist = _context.City.Where(m => m.StateId == defaultStateId).ToList();
            //set defaultCityId   
            var defaultCityId = allCitylist.Select(m => m.CityId).FirstOrDefault();

            //model.FirstName = UsersList[0].FirstName; model.LastName = UsersList[0].LastName;

            //model.Gender= (UsersList[0].Gender == "2") ? "Male" : "Female";

            model.States = new SelectList(allStatelist, "StateId", "StateName", defaultStateId);
            model.Cities = new SelectList(allCitylist, "CityId", "CityName", defaultCityId);
            ViewBag.States = model.States;
            return View(model);

            //Users objU = new Users();
            //List<Users> u = new List<Users>();
            //// List<tblUser> userlist = new List<tblUser>();

            //using (var dtc = new CodeTestContext()) {
            //    u = (from p in _context.Users
            //               select new Users
            //               {
            //                   UserId = p.UserId,
            //                   FirstName = p.FirstName
            //                   , LastName = p.LastName
            //                   , CityId = p.CityId
            //                   , StateId = p.StateId
            //                   , CityName = p.CityName
            //                   , StateName = p.StateName

            //               }).ToList();
            //}

            //      using (var dt = new CodeTestContext())
            //      {
            //          u = (from a in _context.Users
            //               join b in _context.City on a.CityId equals b.CityId
            //               join c in _context.State on b.StateId equals c.StateId
            //               select new { a, b, c }).ToList().Select(p => new Users()
            //               {
            //                   UserId = (int)p.a.UserId,
            //                   FirstName = p.a.FirstName,
            //                   LastName = p.a.LastName,
            //                   StateName = p.c.StateName,
            //                   CityName = p.b.CityName,
            //                   CityId = p.a.CityId,
            //                   StateId = p.b.StateId,
            //                   Gender = (p.a.Gender == "1") ? "Female" : "Male",
            //                   GenderId = p.a.Gender
            //               })
            //.ToList();
            //}
            //var dt = (from a in _context.Users
            //     join b in _context.City on a.CityId equals b.CityId
            //     join c in _context.State on b.StateId equals c.StateId
            //     select new { a, b, c }).ToList().Select(p => new Users()
            //     {
            //         UserId = (int)p.a.UserId,
            //         FirstName = p.a.FirstName,
            //         LastName = p.a.LastName,
            //         StateName = p.c.StateName,
            //         CityName = p.b.CityName,
            //         CityId = p.a.CityId,
            //         StateId = p.b.StateId,
            //         Gender = (p.a.Gender == "1") ? "Female" : "Male",
            //         GenderId = p.a.Gender
            //     })
            //     .ToList();



            //  u.Cast<List<object>>().Select(x => new Users()).ToList();




        }

        [HttpPost]

        public JsonResult setDropDrownList(int value)
        {
            Users model = new Users();
            
                    var statesList = _context.State.ToList();
                    model.States = new SelectList(statesList, "StateId", "StateName");
                    var defaultStateId = statesList.Select(m => m.StateId).FirstOrDefault();
                    model.Cities = new SelectList(_context.City.Where(m => m.StateId == defaultStateId).ToList(), "CityId", "CityName");
                    model.Cities = new SelectList(_context.City.Where(m => m.StateId == value).ToList(), "CityId", "CityName");
                    
            return Json(model.Cities);
        }

        [HttpGet]
        public JsonResult GetCities(int id)
        {
            Users model = new Users();

            var statesList = _context.State.ToList();
            model.States = new SelectList(statesList, "StateId", "StateName");
            var defaultStateId = statesList.Select(m => m.StateId).FirstOrDefault();
            model.Cities = new SelectList(_context.City.Where(m => m.StateId == defaultStateId).ToList(), "CityId", "CityName");
            model.Cities = new SelectList(_context.City.Where(m => m.StateId == id).ToList(), "CityId", "CityName");

            return Json(model.Cities);
        }

        [HttpPost]
        public ActionResult Index(Users model)
        {
            
            model.States = new SelectList(_context.State.ToList(), "StateId", "StateName", model.StateId);
            model.Cities = new SelectList(_context.City.Where(m => m.StateId == model.StateId).ToList(), "CityId", "CityName", model.CityId);
            return View(model);
        }
        //ViewData["CityId"] = new SelectList(_context.City, "CityId", "CityId");
        //ViewData["City"] = new SelectList(_context.City, "CityId", "City");
        // string gender;
        // using (var ctx= new CodeTestContext())
        // {
        //         gender = ctx.Users
        //                   .Where(u => u.UserId == 1)
        //                   .Select(u => u.Gender)
        //                   .SingleOrDefault();
        // }
        //ViewBag.Gender=(gender=="2") ? "Male":"Female";
        //var users = _context.Users
        //     .Include(u => u.City)
        //     .FirstOrDefault() ;
        //var stateslist = new SelectList(_context.State.OrderBy(l => l.State1)
        //.ToDictionary(us => us.StateId, us => us.State1), "Key", "Value");

        //var defaultstateslist = _context.State.ToList();
        // var defaultStateId = defaultstateslist.Select(m => m.StateId).FirstOrDefault();

        //Get all cities according to defaultStateId  

        //var allCitylist = _context.City.Where(c =>c.StateId.Equals(defaultStateId)).ToList().Select(u => new SelectListItem
        //{
        //    Text  = u.City1,
        //    Value = u.CityId.ToString()
        //}); ;      //set defaultCityId   

        // ViewBag.stateslist = stateslist;
        //ViewBag.CitylistByState=allCitylist;

        // return View();
        //}
       // [HttpPost]
        //    public JsonResult setDropDrownList(string type, int value)
        //    {
        //        Users model = new Users();

        //                model.City = (_context.City => c.StateId == value).ToList();


        //        return Json(model);
        //    }
        //    [HttpPost]
        //    public ActionResult Index(CodeTestContext model)
        //    {
        //        model.Countries = new SelectList(db.Countrys.ToList(), "CountryId", "CountryName", model.CountryId);
        //        model.States = new SelectList(db.States.Where(m => m.CountryId.ToString() == model.CountryId).ToList(), "StateId", "StateName", model.StateId);
        //        model.Cities = new SelectList(db.Citys.Where(m => m.StateId.ToString() == model.StateId).ToList(), "CityId", "CityName", model.CityId);
        //        return View(model);
        //    }
        //}

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.


        [HttpPost]
        public async Task<IActionResult> Create([Bind("UserId,FirstName,LastName,CityId,Gender")] Users users)
        {
            if (ModelState.IsValid)
            {
                _context.Add(users);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityId"] = new SelectList(_context.City, "CityId", "CityId", users.CityId);
            
            return View(users);
        }

        // GET: Users/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null || _context.Users == null)
        //    {
        //        return NotFound();
        //    }

        //    var users = await _context.Users.FindAsync(id);
        //    if (users == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CityId"] = new SelectList(_context.City, "CityId", "CityId", users.CityId);
        //    return View(users);
        //}

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
       // [HttpPost]
      //  [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("UserId,FirstName,LastName,CityId,Gender")] Users users)
        //{
        //    if (id != users.UserId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(users);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!UsersExists(users.UserId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["CityId"] = new SelectList(_context.City, "CityId", "CityId", users.CityId);
        //    return View(users);
        //}
        //GET: Users/Edit/5
        public IActionResult Edit(int id)
        {
            Users model = new Users();
           // var allStatelist = _context.State.ToList();
            ////set defaultStateId  
            
            //var defaultStateId = allStatelist.Select(m => m.StateId).FirstOrDefault();
            ////Get all cities according to defaultStateId  
            //var allCitylist = _context.City.Where(m => m.StateId == defaultStateId).ToList();
            ////set defaultCityId   
            //var defaultCityId = allCitylist.Select(m => m.CityId).FirstOrDefault();

            ////model.FirstName = UsersList[0].FirstName; model.LastName = UsersList[0].LastName;

            ////model.Gender= (UsersList[0].Gender == "2") ? "Male" : "Female";

            //model.States = new SelectList(allStatelist, "StateId", "StateName", defaultStateId);
            //model.Cities = new SelectList(allCitylist, "CityId", "CityName", defaultCityId);
            
            
            var users = _context.Users
                        .Include(u => u.City)
            .ThenInclude(u => u.State)
            .Where(u => u.UserId == id).FirstOrDefault();

            var SelectedCityId = users.CityId;
            var SelectedStateId=users.City.StateId;
            var allStatelist = _context.State.ToList();
            //model.States = new SelectList(allStatelist, "StateId", "StateName", SelectedStateId);
            var allCitylist = _context.City.Where(m => m.StateId == SelectedStateId).ToList();
            //model.Cities = new SelectList(allCitylist, "CityId", "CityName", SelectedCityId);
            
            users.States = new SelectList(allStatelist, "StateId", "StateName", SelectedStateId);
            users.Cities = new SelectList(allCitylist, "CityId", "CityName", SelectedCityId);

            return PartialView("EditUserModelPartial", users);


        }
        [HttpGet]
        public JsonResult EditUser(int id)
        {


            //var users = _context.Users.Find(id);

            //var SelectedCityId = users.CityId;
            //var SelectedCityName = _context.City.Find(SelectedCityId);

            //var SelectedState = _context.City.Find(SelectedCityId);
            //var SelectedStateId = SelectedState.StateId;

            //var State = _context.State.Find(SelectedStateId);
            //var StateId = State.StateId;

            //  users.StateId = StateId;
            //-------------------------

            List<Users> u = new List<Users>();
            using (var dt = new CodeTestContext())
            {
                u = (from a in _context.Users
                     join b in _context.City on a.CityId equals b.CityId
                     join c in _context.State on b.StateId equals c.StateId
                     select new { a, b, c }).ToList().Select(p => new Users()
                     {
                         UserId = (int)p.a.UserId,
                         FirstName = p.a.FirstName,
                         LastName = p.a.LastName,
                         StateName = p.c.StateName,
                         CityName = p.b.CityName,
                         CityId = p.a.CityId,
                         StateId = p.b.StateId,
                         Gender = (p.a.Gender == "1") ? "Female" : "Male",
                         GenderId = p.a.Gender
                     })
      .ToList();
            }

            //----------------------------

            var ul = u.FirstOrDefault(m => m.UserId==id);

           // ul.StateId = StateId;

            return Json(ul);
        }

        [HttpPost]
        public IActionResult Edit(Users users)
        {
            _context.Users.Update(users);
            _context.SaveChanges();
            //return PartialView("EditUserModelPartial", users); ;
            return View("UsersList");
        }
        // GET: Users/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.Users == null)
        //    {
        //        return NotFound();
        //    }

        //    var users = await _context.Users
        //        .Include(u => u.City)
        //         .ThenInclude(u => u.State)
        //        .FirstOrDefaultAsync(m => m.UserId == id);
        //    if (users == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(users);
        //}

        [HttpPost]
        public JsonResult DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user !=null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
                return Json("User deleted Successfully");
            }

            return Json("User not deleted..!!");
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'CodeTestContext.Users'  is null.");
            }
            var users = await _context.Users.FindAsync(id);
            if (users != null)
            {
                _context.Users.Remove(users);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public ActionResult UsersList()
        {
            Users model = new Users();
            //get all country  
            var UsersList = _context.Users.ToList();
            //get all states according to defaultCountryId  
            var allStatelist = _context.State.ToList();
            //set defaultStateId  
            var defaultStateId = allStatelist.Select(m => m.StateId).FirstOrDefault();
            //Get all cities according to defaultStateId  
            var allCitylist = _context.City.Where(m => m.StateId == defaultStateId).ToList();
            //set defaultCityId   
            var defaultCityId = allCitylist.Select(m => m.CityId).FirstOrDefault();

            //model.FirstName = UsersList[0].FirstName; model.LastName = UsersList[0].LastName;

            //model.Gender= (UsersList[0].Gender == "2") ? "Male" : "Female";

            model.States = new SelectList(allStatelist, "StateId", "StateName", defaultStateId);
            model.Cities = new SelectList(allCitylist, "CityId", "CityName", defaultCityId);
            ViewBag.States = model.States;
            return View(model);
            
        }
        private bool UsersExists(int id)
        {
          return _context.Users.Any(e => e.UserId == id);
        }
    }
}
