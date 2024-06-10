using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Repositories.Interfaces;
using Services.DTO.EventDTOs;
using Services.DTO.EventFeedbackModel;
using Services.DTO.ResponseModels;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class EventFeedbackService : IEventFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EventFeedbackService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseGenericModel<EventFeedbackDetailModel>> CreateFeedBackForEvent(CreateFeedbackModel inputFeedback, FeedbackTypeEnums type)
        {
            var checkEvent = await _unitOfWork.EventRepository.GetByIdAsync(inputFeedback.EventId);
            if (checkEvent == null)
            {
                return new ResponseGenericModel<EventFeedbackDetailModel>
                {
                    Status = false,
                    Data = null,
                    Message = "This event is no longer existing, please try again"
                };
            }

            EventFeedback newFeedback = new EventFeedback
            {
                EventId = inputFeedback.EventId,
                Content = inputFeedback.Content,
            };

            newFeedback = await _unitOfWork.EventFeedbackRepository.CreateFeedbackAsync(newFeedback);
            checkEvent.Status = type.ToString();
            bool updateStatus = await _unitOfWork.EventRepository.Update(checkEvent);

            var saveCheck = await _unitOfWork.SaveChangeAsync();
            if (updateStatus || saveCheck > 0)
            {
               
                return new ResponseGenericModel<EventFeedbackDetailModel>
                {
                    Status = true,
                    Data = _mapper.Map<EventFeedbackDetailModel>(newFeedback),
                    Message = "Added and updated event successfully"
                };
            }

            return new ResponseGenericModel<EventFeedbackDetailModel>
            {
                Status = false,
                Data = null,
                Message = "Added failed"
            };
        }

        public async Task<List<EventFeedbackDetailModel>> GettAllFeedbacksAsync()
        {
            try
            {
                var data = _mapper.Map<List<EventFeedbackDetailModel>>(await _unitOfWork.EventFeedbackRepository.GettAllFeedbacksAsync());
                return data;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }
    }
}