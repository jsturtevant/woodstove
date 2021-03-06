﻿using Dashboard.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WoodStove.Core;

namespace Dashboard.Controllers
{
    [Authorize]
    public class TemperatureController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: Temperature
        public async Task<ActionResult> Index()
        {
            var dvm = new DevicesViewModel();

            dvm.Devices = await GetUserDevices().ToListAsync();

            return View(dvm);
        }

        private IQueryable<Device> GetUserDevices()
        {
            var userId = this.User.Identity.GetUserId();
            IQueryable<Device> devices = this.db.Users.Include(u => u.Devices).Where(u => u.Id == userId).SelectMany(u => u.Devices);
            return devices;
        }

        // GET: Temperature
        [AllowAnonymous]
        public async Task<ActionResult> Demo()
        {
            var dvm = new DevicesViewModel();

            var demoDevices = new List<Device>()
            {
                new Device()
                {
                    Id= "demo-1",
                    Name= "Demo",
                    ZipCode="92118",
                    Display = true
                }
            };

            dvm.Devices = demoDevices;

            return View("Index", dvm);
        }
    }

   
}