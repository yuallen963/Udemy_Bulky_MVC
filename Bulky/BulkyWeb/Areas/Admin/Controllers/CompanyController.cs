using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers
{
    public class CompanyController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            return View(objCompanyList);
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            Company companyObj = new Company();
            if (id == null || id == 0) 
            {
                return View(companyObj);
            }
            else
            {
                companyObj = _unitOfWork.Company.Get(u => u.Id == id);
                return View(companyObj);
            }
        }

        [HttpPost]
        public IActionResult Upsert(Company companyObj)
        {
            if (ModelState.IsValid)
            {
                if (companyObj.Id == 0)
                    _unitOfWork.Company.Add(companyObj);
                else
                    _unitOfWork.Company.Update(companyObj);
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index", "Company");
            }
            else
                return View();
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Company obj = _unitOfWork.Company.Get(u => u.Id == id);
            if (obj == null)
                return NotFound();
            _unitOfWork.Company.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction("Index", "Company");
        }

        #region API Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            return Json(new { data = objCompanyList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _unitOfWork.Company.Get(u => u.Id == id);
            if (productToBeDeleted == null)
                return Json(new { success = false, message = "Error while deleting" });
            _unitOfWork.Company.Remove(productToBeDeleted);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successfully" });
        }
        #endregion
    }
}
