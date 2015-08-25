using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dashboard.Models;
using WoodStove.Core;
using Microsoft.AspNet.Identity;

namespace Dashboard.Controllers
{
    [Authorize]
    public class DevicesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Devices
        public async Task<ActionResult> Index()
        {
            var devices = this.GetUserDevices();
            return View(await devices.ToListAsync());
        }

        // GET: Devices/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var devices = this.GetUserDevices();
            Device device = await devices.FirstOrDefaultAsync(x => x.Id == id);
            if (device == null)
            {
                return HttpNotFound();
            }
            return View(device);
        }

        // GET: Devices/Create 
        public ActionResult Create()
        {
            return View();
        }

        // POST: Devices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name")] Device device)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var currentUser = await db.Users.FirstAsync(u => u.Id == userId);

                if (currentUser.Devices.Any(x => x.Id == device.Id))
                {
                    ModelState.AddModelError("", "Device already exists.");
                    return this.View(device);
                }
                currentUser.Devices.Add(device);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(device);
        }

        // GET: Devices/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userDevices = this.GetUserDevices();
            Device device = await userDevices.FirstOrDefaultAsync(x => x.Id == id);
            if (device == null)
            {
                return HttpNotFound();
            }

            DeviceViewModel dvm = new DeviceViewModel();
            dvm.Id = device.Id;
            dvm.Name = device.Name;
            dvm.NewId = device.Id;
            dvm.ZipCode = device.ZipCode;
            return View(dvm);
        }

        // POST: Devices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,NewId,Name,ZipCode")] DeviceViewModel device)
        {
            if (ModelState.IsValid)
            {
                var userDevices = this.GetUserDevices();
                Device databaseDevice = await userDevices.FirstOrDefaultAsync(x => x.Id == device.Id);
                if (databaseDevice != null)
                {
                    if (databaseDevice.Id != device.NewId)
                    {
                        db.Devices.Remove(databaseDevice);

                        var newdevice = new Device()
                        {
                            Id = device.NewId,
                            Name = device.Name,
                            ZipCode = device.ZipCode
                        };

                        var userId = User.Identity.GetUserId();
                        var currentUser = await db.Users.FirstAsync(u => u.Id == userId);
                        currentUser.Devices.Add(newdevice);
                        await db.SaveChangesAsync();
                    }
                    else
                    {
                        databaseDevice.Name = device.Name;
                        databaseDevice.ZipCode = device.ZipCode;
                        await db.SaveChangesAsync();
                    }


                    return RedirectToAction("Index");
                }

                return this.HttpNotFound();
            }
            return View(device);
        }

        // GET: Devices/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userDevices = this.GetUserDevices();
            Device device = await userDevices.FirstOrDefaultAsync(x => x.Id == id);
            if (device == null)
            {
                return HttpNotFound();
            }
            return View(device);
        }

        // POST: Devices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            var userDevices = this.GetUserDevices();
            Device device = userDevices.FirstOrDefault(x => x.Id == id);
            if (device != null)
            {
                db.Devices.Remove(device);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return this.HttpNotFound();

        }


        private IQueryable<Device> GetUserDevices()
        {
            var userId = this.User.Identity.GetUserId();
            IQueryable<Device> devices = this.db.Users.Include(u => u.Devices).Where(u => u.Id == userId).SelectMany(u => u.Devices);
            return devices;
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }


}
