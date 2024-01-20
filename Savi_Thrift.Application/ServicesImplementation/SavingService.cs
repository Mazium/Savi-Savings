using AutoMapper;
using Microsoft.AspNetCore.Http;
using Savi_Thrift.Application.DTO.Saving;
using Savi_Thrift.Application.Interfaces.Repositories;
using Savi_Thrift.Application.Interfaces.Services;
using Savi_Thrift.Domain.Entities;
using Savi_Thrift.Domain;

namespace Savi_Thrift.Application.ServicesImplementation
{
    public class SavingService : ISavingService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public SavingService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<ApiResponse<GoalResponseDto>> CreateGoal(CreateGoalDto createGoalDto)
		{

			var existingBoard = await _unitOfWork.SavingRepository.GetByIdAsync(createGoalDto.Title);
			if (existingBoard != null)
			{
				return ApiResponse<GoalResponseDto>.Failed("Goal already exists with this title",400, new List<string>());
			}

			try
			{
				var saving = _mapper.Map<Saving>(createGoalDto);
				await _unitOfWork.SavingRepository.AddAsync(saving);
				await _unitOfWork.SaveChangesAsync();

				var reponseDto = _mapper.Map<GoalResponseDto>(saving);
				return ApiResponse<GoalResponseDto>.Success(reponseDto, "Goal Created Successfully",StatusCodes.Status200OK);

			}
			catch (Exception ex)
			{
				return ApiResponse<GoalResponseDto>.Failed("Error occurred while creating a goal", StatusCodes.Status500InternalServerError, new List<string> { ex.Message });
			}
		}

        public async Task<ApiResponse<List<Saving>>> GetListOfAllUserGoals(string UserId)
        {
            
            try
            {
                var listOfTargets = await _unitOfWork.SavingRepository.FindAsync(u => u.Id == UserId);
                if (listOfTargets.Any())
                {
                    return ApiResponse<List<Saving>>.Success(listOfTargets, " Goals Retrived Successfully", StatusCodes.Status200OK);
                    
                }
                return ApiResponse<List<Saving>>.Failed("Goal not found", StatusCodes.Status400BadRequest, new List<string>());
            
            }

            catch (Exception ex)
            {
                return ApiResponse<List<Saving>>.Failed("Error occurred while creating a goal", StatusCodes.Status500InternalServerError, new List<string> { ex.Message });

            }
        }

        public async Task<ApiResponse<List<GoalResponseDto>>> ViewGoals()
		{
			var wallets = await _unitOfWork.SavingRepository.GetAllAsync();
			List<GoalResponseDto> result = new();	
			foreach (var wallet in wallets)
			{
				var reponseDto = _mapper.Map<GoalResponseDto>(wallet);
				result.Add(reponseDto);
			}
			return ApiResponse<List<GoalResponseDto>>.Success(result, "Goals retrieved successfully",StatusCodes.Status200OK); 
		}
	}
}
