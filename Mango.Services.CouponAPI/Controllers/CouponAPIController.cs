using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/coupon")]
    [ApiController]
    //[Authorize]
    public class CouponAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationdbcontext;
        private ResponseDTO _responseDTO;
        private IMapper _mapper;
        public CouponAPIController(ApplicationDbContext applicationdbcontext, IMapper mapper)
        {
            _applicationdbcontext = applicationdbcontext;
            _responseDTO = new ResponseDTO();
            _mapper = mapper;
        }

        [HttpGet("getall")]
        public object Get()
        {
            try
            {
                IEnumerable<Coupon> objList =_applicationdbcontext.coupons.ToList();
                IEnumerable<CouponDTO> couponDTOs = _mapper.Map<IEnumerable<CouponDTO>>(objList);
                _responseDTO.Result = couponDTOs;
                _responseDTO.Message = "Successfully";
            }
            catch (Exception)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = "failed to get data";

            }
            return _responseDTO;
        }

        [HttpGet("getbyid/{id}")]
        public object Get(int id)
        {
            try
            {
                Coupon obj = _applicationdbcontext.coupons.First(x => x.CouponId == id);
                _responseDTO.Result = _mapper.Map<CouponDTO>(obj);
                _responseDTO.Message = "Successfully get data by id";
            }
            catch(Exception)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message="Failed to get data";

            }
            return _responseDTO;
        }

        [HttpGet("getbycode")]
        public object Get(string couponcode)
        {
            try
            {
                Coupon obj = _applicationdbcontext.coupons.First(x => x.CouponCode == couponcode);
                _responseDTO.Result = _mapper.Map<CouponDTO>(obj);
                _responseDTO.Message = "Success";
               
            }
            catch (Exception)
            {
                _responseDTO.IsSuccess=false;
                _responseDTO.Message= "Failed to getbycode";
            }
            return _responseDTO;
        }

        [HttpPost]
        public ResponseDTO Post([FromBody] CouponDTO couponDTO)
        {
            try
            {
                Coupon obj = _mapper.Map<Coupon>(couponDTO);
                _applicationdbcontext.coupons.Add(obj);
                _applicationdbcontext.SaveChanges();
                _responseDTO.Result = _mapper.Map<CouponDTO>(obj);
                _responseDTO.Message = "Created Successfully";

            }
            catch (Exception)
            {
                _responseDTO.IsSuccess=false;
                _responseDTO.Message= "Failed to create";
            }
            return _responseDTO;
        }

        [HttpPut]
        public ResponseDTO Put([FromBody] CouponDTO couponDTO)
        {
            try
            {
                Coupon obj = _mapper.Map<Coupon>(couponDTO);
                _applicationdbcontext.coupons.Update(obj);
                _applicationdbcontext.SaveChanges();
                _responseDTO.Message = "Updated Successfully";
            }
            catch(Exception) 
            {
          
                _responseDTO.IsSuccess=false;
                _responseDTO.Message= "Failed to Update";
            }
            return _responseDTO;
        }

        [HttpDelete]
        [Route("{id:int}")]
        public ResponseDTO Delete(int id)
        {
            try
            {
                Coupon obj=_applicationdbcontext.coupons.First(d=>d.CouponId==id);
                _applicationdbcontext.coupons.Remove(obj);
                _applicationdbcontext.SaveChanges();
                _responseDTO.Message = "Deleted Successfully";

            }
            catch(Exception)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message= "Failed to Delete";
            }
            return _responseDTO;
        }
        

    }
}
