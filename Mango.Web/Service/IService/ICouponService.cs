using Mango.Web.Models;
using Mango.Web.Models.DTO;

namespace Mango.Web.Service.IService
{
    public interface ICouponService
    {
        Task<ResponseDto?> GetCouponAsync(string couponcode);
        Task<ResponseDto?> GetAllCouponAsync();
        Task<ResponseDto?> GetCouponByIdAsync(int id);
        Task<ResponseDto?> CreateCouponAsync(CouponDTO couponDTO);
        Task<ResponseDto?> UpdateCouponAsync(CouponDTO couponDTO);
        Task<ResponseDto?> DeleteCouponAsync(int id);
    }
}
