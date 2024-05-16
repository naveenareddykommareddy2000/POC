﻿using Mango.Web.Models;
using Mango.Web.Models.DTO;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;
        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }
        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDTO>? list = new List<CouponDTO>();
            ResponseDto? response = await _couponService.GetAllCouponAsync();
            if(response != null && response.IsSuccess==true)
            {
                list=JsonConvert.DeserializeObject<List<CouponDTO>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(list);
        }

        public async Task<IActionResult> CouponCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CouponCreate(CouponDTO model)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? response=await _couponService.CreateCouponAsync(model);
                if(response!=null && response.IsSuccess==true)
                {
                    TempData["success"] = "Coupon Created Successfully";
                    return RedirectToAction(nameof(CouponIndex));
                }
                else
                {
                    TempData["error"]=response?.Message;
                }
            }
            return View(model);
        }

     
        public async Task<IActionResult> CouponDelete(int couponId)
        {
            ResponseDto? response = await _couponService.DeleteCouponAsync(couponId);
            if (response != null && response.IsSuccess == true)
            {
                CouponDTO model = JsonConvert.DeserializeObject<CouponDTO>(Convert.ToString(response.Result));
                return View(model);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }


        [HttpPost]
        public async Task<IActionResult> CouponDelete(CouponDTO couponDTO)
        {
            ResponseDto? response = await _couponService.DeleteCouponAsync(couponDTO.CouponId);
            if (response != null && response.IsSuccess == true)
            {
                TempData["success"] = "Coupon Deleted Successfully";
                return RedirectToAction(nameof(CouponIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(couponDTO);
        }
    }  

}
