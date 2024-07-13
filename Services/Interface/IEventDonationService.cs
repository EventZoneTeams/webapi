using Services.DTO.EventDonationDTOs;
using Services.DTO.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IEventDonationService
    {
        Task<ResponseGenericModel<EventDonationDetailDTO>> AddDonationToCampaign(EventDonationCreateDTO data);
        Task<List<EventDonationDetailDTO>> GetAllDonationOfCampaign(int id);
    }
}