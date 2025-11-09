using H_application.DTOs.GuestDto;
using H_application.DTOs.HotelDto;
using H_Domain.Models;

namespace H_application.Service
{
    public interface IHotelServicecs
    {
        Task<bool> CreatehotelAsync(HotelDtoCreate hotelDotCreate, CancellationToken cancellationToken);
        Task<bool> UpdatehotelAsync(HotelDtoUpdate hotelDtoUpdate, CancellationToken cancellationToken);
        Task<bool> DeletehotelAsync(HotelDtoUpdate hotelDtoUpdate, CancellationToken cancellation);

        Task<IEnumerable<HotelResponse>> GetAllHotelAsync(CancellationToken cancellationToken);
        Task<HotelDtoUpdate> GetHotelByIdAsync(int Id, CancellationToken cancellationToken);
       
    }
}
