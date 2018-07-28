using KeysPlus.Service.Models;
using KeysPlus.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeysPlus.Data;
using System.Threading.Tasks;
using System.Security.Principal;
using System.Security.Claims;
using System.Web.Security;
using KeysPlus.Website.Models;

namespace KeysPlus.Website.Controllers
{
    public class HomeController : Controller
    {
        KeysEntities db = new KeysEntities();
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                //return RedirectToAction("Dashboard", "Home", new { area = "PropertyOwners" });
                return RedirectToAction("Dashboard", "Home", new { area = "" }); //Bug Fix #2031
            }

            return RedirectToAction("Login","Account");
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        [Authorize]
        public ActionResult Dashboard()
        {
            var user = User.Identity.Name;
            //var gp = User as GenericPrincipal;
            //var roles = gp.Claims
            //         .Where(c => c.Type == ClaimTypes.Role)
            //         .Select(c => c.Value);
            var login = AccountService.GetLoginByEmail(user);
            var isOwner = PropertyOwnerService.IsLoginOwner(login);
            var isTenant = TenantService.IsLoginATenant(login);
            var isServiceSupplier = CompanyService.IsServiceSupplier(login);
            var isPropertyManager = PropertyOwnerService.IsLoginPropertyManager(login);
            ViewBag.IsOwner = false;
            ViewBag.IsTenant = false;
            ViewBag.IsServiceSupplier = false;
            ViewBag.IsPropertyManager = false;

            if (isOwner)
            {
                ViewBag.isOwner = true;
            }

            if (isTenant)
            {
                ViewBag.IsTenant = true;
            }

            if (isServiceSupplier)
            {
                ViewBag.IsServiceSupplier = true;
            }

            if (isPropertyManager)
            {
                ViewBag.IsPropertyManager = true;
            }
            TempData["CurrentLink"] = "Dashboard;";
            return View();
        }

        public ActionResult PODashboard()
        {
            var user = User.Identity.Name;
            var login = AccountService.GetLoginByEmail(user);
            var props = PropertyService.GetAllPropertiesByOwner(login.Id);
            var totalProps = props.Count();
            var ownerOccupied = db.OwnerProperty.Where(x => x.OwnerId == login.Id && x.Property.IsActive && (x.Property.IsOwnerOccupied ?? false)).Count();
            var tenantOccupied = db.OwnerProperty.Where(x => x.OwnerId == login.Id && x.Property.IsActive
                                && x.Property.TenantProperty.Where(y => y.IsActive ?? false).Count() > 0).Count();
            var vacant = totalProps - (ownerOccupied + tenantOccupied);

            var propIds = props.Select(x => x.Id);
            
            var model = new PODashBoardModel
            {
                PropDashboardData = new PropDashboardModel
                {
                    Occupied = ownerOccupied + tenantOccupied,
                    Vacant = vacant
                },
                

            };
            return PartialView("_PODashboard", model);
        }

        public ActionResult TenantDashboard(int? steps)
        {
            var user = User.Identity.Name;
            var login = AccountService.GetLoginByEmail(user);

            var props = db.TenantProperty.Where(x => x.TenantId == login.Id && (x.IsActive ?? true));
            var propIds = props.Select(x => x.PropertyId);

            //ApplicationStatusId {1 : Applied, 2 : Accepted, 3: Declined}
            var rentApps = db.RentalApplication.Where(x => x.PersonId == login.Id && x.IsActive);
            var newApps = rentApps.Where(x => x.ApplicationStatusId == 1 && !(x.IsViewedByOwner ?? false)).Count();
            var acceptedApps = rentApps.Where(x => x.ApplicationStatusId == 2).Count();
            var pendingApps = rentApps.Where(x => x.ApplicationStatusId == 1 && (x.IsViewedByOwner ?? false)).Count();
            var declinedApps = rentApps.Where(x => x.ApplicationStatusId == 3).Count();

            // Get Landlord request's statistics
            var landlordreqs = db.PropertyRequest.Where(x => propIds.Contains(x.Property.Id) && x.IsActive && !x.ToOwner && x.ToTenant);
            var newLandlordRequests = landlordreqs.Where(x => x.RequestStatusId == 1).Count();
            var acceptedLandlordRequests = landlordreqs.Where(x => x.RequestStatusId == 2).Count();
            var rejectedLandlordRequests = landlordreqs.Where(x => x.RequestStatusId == 5).Count();

            var tenRequests = db.PropertyRequest.Where(x => propIds.Contains(x.Property.Id) && x.IsActive && x.ToOwner && !x.ToTenant);
            var newRequests = tenRequests.Where(x => !x.IsViewed).Count();
            var acceptedRequests = tenRequests.Where(x => x.RequestStatusId == 2).Count();

            var model = new TenantDashBoardModel
            {
                TenantRentalDashboardData = TenantService.GetTenantRentals(login.Id),
                

            };
            model.IntroSteps = steps.HasValue ? steps.Value : 0;
            return PartialView("_TenantDashboard", model);
        }

       
        [Authorize]
        public ActionResult SideNavBar()
        {
            var user = User.Identity.Name;
            var login = AccountService.GetLoginByEmail(user);
            var userRoles = AccountService.GetUserRolesbyEmail(user);
            var isOwner = PropertyOwnerService.IsLoginOwner(login);
            var isTenant = TenantService.IsLoginATenant(login);
            var isServiceSupplier = CompanyService.IsServiceSupplier(login);
            var isAdmin = AccountService.IsAdmin(login);
            var model = new SideBarViewModel
            {
                Roles = userRoles,
                IsPropertyOwner = isOwner,
                IsTenant = isTenant,
                IsServiceSupplier = isServiceSupplier,
                IsAdmin=isAdmin
            };
            return PartialView(model);
        }
        [Authorize]
        public ActionResult TopNavBar()
        {
            var user = User.Identity.Name;
            var login = AccountService.GetLoginByEmail(user);
            if (login == null) return View("Error");
            var userRoles = AccountService.GetUserRolesbyEmail(user);
            var isOwner = PropertyOwnerService.IsLoginOwner(login);
            var isTenant = TenantService.IsLoginATenant(login);
            var isServiceSupplier = CompanyService.IsServiceSupplier(login);
            var isAdmin = AccountService.IsAdmin(login);
            var model = new SideBarViewModel
            {
                Roles = userRoles,
                IsPropertyOwner = userRoles.Contains(4),
                IsTenant = userRoles.Contains(5),
                IsServiceSupplier = userRoles.Contains(6),
                IsAdmin = userRoles.Contains(2),
                IsPropManager = userRoles.Contains(3)
            };
            return PartialView(model);
        }
    }
}